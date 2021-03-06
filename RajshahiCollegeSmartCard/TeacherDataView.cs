using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using RCDESFireAPI;
using System.IO;

namespace RajshahiCollegeSmartCard
{
    public partial class TeacherDataView : Form
    {
        private string cs = ConfigurationManager.ConnectionStrings["DbConnection"].ToString();
        public TeacherDataView()
        {
            InitializeComponent();
            this.ActiveControl = teacherIdTextBox;
            teacherIdTextBox.Focus();
        }

        RCFILE[] rcfiles = new RCFILE[5];
        RCKEY[] keys = new RCKEY[10];

        private bool set_keys()
        {
            for (int i = 0; i < 10; i++)
            {
                byte last_byte = Convert.ToByte((i << 4) | i);
                //All keys should have a length of 16 byte
                byte[] key = new byte[16] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, last_byte };
                //Key number should be between 0x01 to 0x0D                
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
            int[] filesizes = new int[5]  {32, 512, 256, 5120, 32};// Total Size should be within 1024*7 (7KB)
            
            //Key number should be between 1 to 14. 
            //Key number 14 means open access file for specific read/write/read-write operation
            short[] rdkeys = new short[5] { 14, 14, 2, 2, 14};
            short[] rwkeys = new short[5] { 1, 1, 1, 1, 1 };
            short[] wrkeys = new short[5] { 1, 1, 1, 1, 1};
            
            //File Number should be between 0x01 to 0x1E
            for (int i = 0; i < 5; i++)
            {                
                rcfiles[i].Set(Convert.ToByte(i + 1), filesizes[i], rwkeys[i], wrkeys[i], rdkeys[i]);                
            }
            return true;
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
										//oda oda = new oda();
										//oda.InsertSamrtCardUIS(BitConverter.ToString(cardUID), rollTextBox.Text, DateTime.Now.ToString());
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
                    //imageBox.Visible = false;
                    //MessageBox.Show(RCDESFire.GetDESFireErrorMessage(cardresponse));
                    return;
                }



                if (RCDESFire.RCReadFile(0x02, ref ReadBuffer, ref cardresponse)) //Basic
                {
                    String bytearrstring = Program.ByteArrayToString(ReadBuffer);
                    string[] splt = bytearrstring.Split('\n');
                    fullNameTextBox.Text = splt[0].ToString();
                    designationTextBox.Text = splt[1].ToString();
                    departmentTextBox.Text = splt[2].ToString();
                    textBoxSpouse.Text = splt[3].ToString();
                    txtBoxPhone_Offfice.Text = splt[4].ToString();
                    teacherIdTextBox.Text = splt[5].ToString();
                    textBloodGroup.Text = splt[6].ToString();

                    //MessageBox.Show("Open Data:\r\n" + Program.ByteArrayToString(ReadBuffer));
                }
                else
                {
                    // MessageBox.Show(RCDESFire.GetDESFireErrorMessage(cardresponse));
                    return;
                }



                if (RCDESFire.RCReadFile(0x03, keys[2], ref ReadBuffer, ref cardresponse)) //SECURE
                {
                    String bytearrstring = Program.ByteArrayToString(ReadBuffer);
                    string[] splt = bytearrstring.Split('\n');

                    contNumberTextBox.Text = splt[0].ToString();
                    //textBloodGroup.Text = splt[1].ToString();
                    textBoxSpouseNo.Text = splt[1].ToString();

                    //MessageBox.Show("Secure Data:\r\n"+Program.ByteArrayToString(ReadBuffer));
                }
                else
                {
                    // MessageBox.Show(RCDESFire.GetDESFireErrorMessage(cardresponse));
                    return;
                }


                if (RCDESFire.RCReadFile(0x05, ref ReadBuffer, ref cardresponse))
                {
                    //MessageBox.Show("Test Data:\r\n" + Program.ByteArrayToString(ReadBuffer));
                }
                else
                {
                    // MessageBox.Show(RCDESFire.GetDESFireErrorMessage(cardresponse));
                    return;
                }
            }
            catch { }
		}

        private void findTeacherDataButton_Click(object sender, EventArgs e)
        {
			Teacher teacher = Program.GetResponseDataTeacher(teacherIdTextBox.Text);
			fullNameTextBox.Text = teacher.name;
			designationTextBox.Text = teacher.designation;
			txtBoxPhone_Offfice.Text = teacher.phone_office;
			contNumberTextBox.Text = teacher.mobile_no;
			textBloodGroup.Text = teacher.blood_group;
			textBoxSpouse.Text = teacher.spouse_name;
			textBoxSpouseNo.Text = teacher.spouse_mobile;
			//textBoxAddress.Text = teacher.address;
			departmentTextBox.Text = teacher.department;
			string data = teacher.b64_image;
			if (teacher.b64_image != null)
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

		private void groupBox1_Enter(object sender, EventArgs e)
		{

		}

		private void label14_Click(object sender, EventArgs e)
		{

		}

		private void label8_Click(object sender, EventArgs e)
		{

		}

		private void label12_Click(object sender, EventArgs e)
		{

		}

		


		private string SetBasicInfo()  //Basic 1
		{
			return   fullNameTextBox.Text+Environment.NewLine + designationTextBox.Text+Environment.NewLine + departmentTextBox.Text+Environment.NewLine+ textBoxSpouse.Text+Environment.NewLine + txtBoxPhone_Offfice.Text+Environment.NewLine + teacherIdTextBox.Text+Environment.NewLine + textBloodGroup.Text + Environment.NewLine ;
		}
		private string SetSecureInfo()  //SECURE
		{
			return contNumberTextBox.Text+Environment.NewLine /*+ textBloodGroup.Text + Environment.NewLine*/ + textBoxSpouseNo.Text+Environment.NewLine;

		}

		private void burnCardButton_Click(object sender, EventArgs e)
		{
			format_and_write();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			readCard();
		}







		private void Reset_Click(object sender, EventArgs e)
		{

			Teacher teacher = new Teacher();
			teacherIdTextBox.Text = teacher.teacher_id;
			fullNameTextBox.Text = teacher.name;
			designationTextBox.Text = teacher.designation;
			txtBoxPhone_Offfice.Text = teacher.phone_office;
			contNumberTextBox.Text = teacher.mobile_no;
			textBloodGroup.Text = teacher.blood_group;
			textBoxSpouse.Text = teacher.spouse_name;
			textBoxSpouseNo.Text = teacher.spouse_mobile;
			//textBoxAddress.Text = teacher.address;
			departmentTextBox.Text = teacher.department;
			imageBox.Image = null;

		}

        private void TeacherDataView_Load(object sender, EventArgs e)
        {
            
            Timer MyTimer = new Timer();
            MyTimer.Interval = 5 * 1000;//1000;//10*60*1000 ;//15*60*1000;
            //MyTimer.Interval = 10 * 1000;//1000;//10*60*1000 ;//15*60*1000;
            MyTimer.Tick += new EventHandler(MyTimer_Tick);
            MyTimer.Start();
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            Reset_Click(sender, e);
            try
            {
                readCard();
            }
            catch { }
        }
    }


	

	//private void textBoxRealationShip_TextChanged(object sender, EventArgs e)
	//{

	//}
}

