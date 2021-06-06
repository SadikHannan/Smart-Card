using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using RCDESFireAPI;
using System.IO;
using System.Data.SqlClient;
using RajshahiCollegeSmartCard;

namespace RajshahiCollegeSmartCard
{
    public partial class BlmRsStudentDataView : Form
    {
        private string cs = ConfigurationManager.ConnectionStrings["DbConnection"].ToString();
        public BlmRsStudentDataView()
        {
            InitializeComponent();
            this.ActiveControl = rollTextBox;
            rollTextBox.Focus();
        }

        RCFILE[] rcfiles = new RCFILE[5];
        RCKEY[] keys = new RCKEY[10];

        private bool set_keys()
        {

            for (int i = 0; i < 10; i++)
            {
                byte last_byte = Convert.ToByte((i << 4) | i);
                //All keys should have a length of 16 byte
                byte[] key = new byte[16] {
                    0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                    0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, last_byte };
                //Key number should be between 0x01 to 0x0D
                //MessageBox.Show((i+1).ToString()+Environment.NewLine+BitConverter.ToString(key));                
                if (keys[i].Set(Convert.ToByte(i + 1), key))
                    continue;
                else
                    return false;
            }
            return true;
        }

        private bool set_files()
        {
            // Total Size should be within 1024*7 (7KB)
            int[] filesizes = new int[5] { 32, 512, 256, 5120, 32 };// Total Size should be within 1024*7 (7KB)

            //Key number should be between 1 to 14. 
            //Key number 14 means open access file for specific read/write/read-write operation
            short[] rdkeys = new short[5] { 14, 14, 2, 2, 14 };
            short[] rwkeys = new short[5] { 1, 1, 1, 1, 1 };
            short[] wrkeys = new short[5] { 1, 1, 1, 1, 1 };

            //File Number should be between 0x01 to 0x1E
            for (int i = 0; i < 5; i++)
            {
                rcfiles[i].Set(Convert.ToByte(i + 1), filesizes[i], rwkeys[i], wrkeys[i], rdkeys[i]);
            }
            return true;
        }


        private void findDataButton_Click(object sender, EventArgs e)
        {
            ESMStudent student = Program.GetResponseDataEasySchoolmatetudent(EnumClass.dbName, rollTextBox.Text);
            fullNameTextBox.Text = student.StudentName;
            departmentTextBox.Text = student.Department;
            academicYearTextBox.Text = student.academic_year;
            classTextBox.Text = student.Class;
            sectionTxtbox.Text = student.section;
            shiftTextBox.Text = student.Shift;
            classRollTextBox.Text = student.Class;
            fathersNameTextBox.Text = student.father_name;
            mothernameTextBox.Text = student.mother_name;
            dobTextBox.Text = student.birth_date;
            genderTextBox.Text = (student.gender==""+1?"Male":"Female");
            gurdianNumberTextBox.Text = student.guardian_mobile;
            religionTextBox.Text = student.religion;
            string data = student.b64_image;
            try
            {
                if (student.b64_image != null)
                {
                    burnCardButton.Visible = true;
                    string base64string = data.Split(new string[] { "base64," }, StringSplitOptions.None)[1].ToString();
                    byte[] byteBuffer = Convert.FromBase64String(base64string);
                    MemoryStream memoryStream = new MemoryStream(byteBuffer);
                    using (MemoryStream ms = new MemoryStream(byteBuffer))
                    {
                        imageBox.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    MessageBox.Show("This Student No Picture available");
                    burnCardButton.Visible = false;

                }
            }
            catch { }


        }

        private void qrCodeBox_Click(object sender, EventArgs e)
        {

        }

        private void burnCardButton_Click(object sender, EventArgs e)
        {
            format_and_write();
        }

        private void format_and_write()
        {
            set_keys(); set_files();
            byte[] ImageArray = new byte[5120];
            byte[] cardUID = new byte[7];
            byte cardresponse = 0x00;
            short cmp_ratio = 1;

            if (rbt2.Checked) cmp_ratio = 2;
            if (rbt3.Checked) cmp_ratio = 3;
            if (rbt4.Checked) cmp_ratio = 4;
            Program.ImageCopressSave(imageBox.Image, cmp_ratio, ref ImageArray);
            byte[] VersionArray = new byte[32];
            if (RCDESFire.FormatRCDESFire(ref cardUID, keys, rcfiles, rbtNewCard.Checked, ref cardresponse))
            {
                //Write VersionFile

                byte[] ImageSize = new byte[4];
                Array.Copy(Program.StringToByteArray("1.01"), 0, VersionArray, 0, 4);
                ImageSize = Program.GetSize(ImageArray.Length);
                Array.Copy(cardUID, 0, VersionArray, 4, 7);
                Array.Copy(ImageSize, 0, VersionArray, 11, 3);
                if (RCDESFire.RCWriteFile(0x01, keys[00], VersionArray, ref cardresponse))
                {
                    //Write Basic File
                    if (RCDESFire.RCWriteFile(0x02, keys[00], Program.StringToByteArray(SetBasicInfo()), ref cardresponse))
                    {
                        //Write Private File
                        if (RCDESFire.RCWriteFile(0x03, keys[00], Program.StringToByteArray(SetSecureInfo()), ref cardresponse))
                        {
                            //Write Image
                            if (RCDESFire.RCWriteFile(0x04, keys[00], ImageArray, ref cardresponse))
                            {
                                //Write Test File
                                if (RCDESFire.RCWriteFile(0x05, keys[00], Program.StringToByteArray("raj IT Solution Ltd 2017"), ref cardresponse))
                                {
                                    try
                                    {
                                        oda oda = new oda();
                                        oda.InsertSamrtCardUIS(BitConverter.ToString(cardUID), rollTextBox.Text, DateTime.Now.ToString());
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    };

                                    MessageBox.Show("Card Formatted Successfully." + Environment.NewLine + "Card UID: " + BitConverter.ToString(cardUID));
                                    return;
                                }
                                else MessageBox.Show(RCDESFire.GetDESFireErrorMessage(cardresponse));
                            }
                            else MessageBox.Show(RCDESFire.GetDESFireErrorMessage(cardresponse));
                        }
                        else MessageBox.Show(RCDESFire.GetDESFireErrorMessage(cardresponse));
                    }
                    else MessageBox.Show(RCDESFire.GetDESFireErrorMessage(cardresponse));
                }
                else MessageBox.Show(RCDESFire.GetDESFireErrorMessage(cardresponse));
            }
            else MessageBox.Show(RCDESFire.GetDESFireErrorMessage(cardresponse));
        }

        private void readCardButton_Click(object sender, EventArgs e)
        {
            readCard();

        }

        private void readCard()
        {
            try
            {
                set_keys();
                byte cardresponse = 0x00;
                byte[] ReadBuffer = new byte[1024];
                //Read Version
                byte[] Version = new byte[32];
                byte[] ImageFile = new byte[5120];
                if (RCDESFire.RCReadFile(0x01, ref Version, ref cardresponse))
                {
                    //Read Image
                    byte[] byte_Version_Number = new byte[4];
                    Array.Copy(Version, 0, byte_Version_Number, 0, 4);
                    //MessageBox.Show("Version#" + Program.ByteArrayToString(byte_Version_Number));
                    byte[] bimagesize = new byte[4]; Array.Copy(Version, 11, bimagesize, 0, 3);
                    int imagesize = Program.GetValue(bimagesize);
                    if (RCDESFire.RCReadFile(0x04, keys[1], ref ImageFile, ref cardresponse))
                    {
                        /////////////
                        byte[] ImageFileAr = new byte[imagesize];
                        Array.Copy(ImageFile, ImageFileAr, imagesize);
                        System.IO.MemoryStream strm = new System.IO.MemoryStream(ImageFileAr);
                        imageBox.Image = Bitmap.FromStream(strm);
                        imageBox.Visible = true;
                        /////////////
                    }
                    else imageBox.Visible = false;
                }
                else
                {
                    imagebox2.Visible = false;
                    //MessageBox.Show(RCDESFire.GetDESFireErrorMessage(cardresponse));
                    return;
                }
                if (RCDESFire.RCReadFile(0x02, ref ReadBuffer, ref cardresponse))
                {
                    String bytearrstring = Program.ByteArrayToString(ReadBuffer);

                    string[] splt = bytearrstring.Split('\n');
                    rollTextBox.Text = splt[0].ToString();
                    fullNameTextBox.Text = splt[1].ToString();
                    classTextBox.Text = splt[2].ToString();
                    genderTextBox.Text = splt[3].ToString();
                    religionTextBox.Text = splt[4].ToString();
                    academicYearTextBox.Text = splt[5].ToString();
                    departmentTextBox.Text = splt[6].ToString();

                    shiftTextBox.Text = (splt[7] != null ? splt[7].ToString() : "");
                    //regStatusTextBox.Text = splt[8].ToString();
                    //fathersNameTextBox.Text = splt[9].ToString();
                    //mothernameTextBox.Text = splt[10].ToString();
                    //contNumberTextBox.Text = splt[11].ToString();
                    //statusTxtbox.Text = splt[12].ToString();



                    //MessageBox.Show("Open Data:\r\n" + Program.ByteArrayToString(ReadBuffer));
                }
                else
                {
                    //MessageBox.Show(RCDESFire.GetDESFireErrorMessage(cardresponse));
                    return;
                }
                if (RCDESFire.RCReadFile(0x03, keys[2], ref ReadBuffer, ref cardresponse))
                {
                    String bytearrstring2 = Program.ByteArrayToString(ReadBuffer);

                    string[] splt = bytearrstring2.Split('\n');

                    dobTextBox.Text = splt[0].ToString();
                    gurdianNumberTextBox.Text = splt[1].ToString();
                    classRollTextBox.Text = splt[2].ToString();
                    fathersNameTextBox.Text = splt[3].ToString();



                    //MessageBox.Show("Secure Data:\r\n" + Program.ByteArrayToString(ReadBuffer));
                }
                else
                {
                    //MessageBox.Show(RCDESFire.GetDESFireErrorMessage(cardresponse));
                    return;
                }
                if (RCDESFire.RCReadFile(0x05, ref ReadBuffer, ref cardresponse))
                {
                    // MessageBox.Show("Test Data:\r\n" + Program.ByteArrayToString(ReadBuffer));
                }
                else
                {
                    // MessageBox.Show(RCDESFire.GetDESFireErrorMessage(cardresponse));
                    return;
                }
            }
            catch { }
        }

        private string SetBasicInfo()
        {
            return rollTextBox.Text + Environment.NewLine +
                fullNameTextBox.Text + Environment.NewLine +
                classTextBox.Text + Environment.NewLine +
                genderTextBox.Text + Environment.NewLine +
                religionTextBox.Text + Environment.NewLine +
                academicYearTextBox.Text + Environment.NewLine +
                departmentTextBox.Text + Environment.NewLine +
                shiftTextBox.Text + Environment.NewLine;
            //regStatusTextBox.Text + Environment.NewLine +
            //fathersNameTextBox.Text + Environment.NewLine +
            //mothernameTextBox.Text + Environment.NewLine +
            //contNumberTextBox.Text + Environment.NewLine+
            //statusTxtbox.Text + Environment.NewLine ;

        }
        private string SetSecureInfo()
        {
            return dobTextBox.Text + Environment.NewLine + gurdianNumberTextBox.Text + gurdianNumberTextBox.Text + Environment.NewLine + classRollTextBox.Text + Environment.NewLine + fathersNameTextBox.Text + Environment.NewLine + mothernameTextBox.Text + Environment.NewLine;
        }

        private void reset_Click(object sender, EventArgs e)
        {
            Student student = new Student();
            fullNameTextBox.Text = student.name;
            departmentTextBox.Text = student.dept_name;
            academicYearTextBox.Text = student.session;
            classTextBox.Text = student.faculty_name;
            sectionTxtbox.Text = student.status;
            shiftTextBox.Text = student.current_level;
            classRollTextBox.Text = student.registration_status;
            fathersNameTextBox.Text = student.father_name;
            mothernameTextBox.Text = student.mother_name;
            dobTextBox.Text = student.birth_date;
            genderTextBox.Text = student.gender;
            gurdianNumberTextBox.Text = student.contact_no;
            religionTextBox.Text = student.religion;
            imageBox.Image = null;
            rollTextBox.Text = "";
            imagebox2.Image = null;
        }

        private void StudentDataView_Load(object sender, EventArgs e)
        {
           
                lblHeader.Text = EnumClass.ins_name;
           
            //Timer MyTimer = new Timer();
            //MyTimer.Interval = 5 * 1000;//1000;//10*60*1000 ;//15*60*1000;
            ////MyTimer.Interval = 10 * 1000;//1000;//10*60*1000 ;//15*60*1000;
            //MyTimer.Tick += new EventHandler(MyTimer_Tick);
            //MyTimer.Start();
        }
        private void MyTimer_Tick(object sender, EventArgs e)
        {
            reset_Click(sender, e);
            readCard();
        }

    }
}
