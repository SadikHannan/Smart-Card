using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RajshahiCollegeSmartCard
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Reso
            DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            //dt.Columns.Add("username");
            //dt.Columns.Add("password");

            //DataRow dr = dt.NewRow();
            //dr["username"] = "blmadmin";
            //dr["password"] = "blmadmin";

            //dt.Rows.Add(dr);

            //ds.Tables.Add(dt);
            string directory = System.IO.Directory.GetCurrentDirectory();
            string installedDirectory = System.AppDomain.CurrentDomain.BaseDirectory+"\\Resources";
            string file_Path = Path.Combine(System.IO.Path.GetFullPath(@"..\..\"), "Resources");
           
            string filepath = installedDirectory + "\\db.xml";

            if(!Directory.Exists(installedDirectory))
            {
                Directory.CreateDirectory(installedDirectory);
            }
            //MessageBox.Show(filepath);
            //string directory2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //MessageBox.Show(directory);
            //MessageBox.Show(directory2);
            if (!File.Exists(filepath))
            {
                ds.WriteXml(filepath);
            }
            ds.ReadXml(filepath);
            DataRow[] result = ds.Tables[0].Select("username = '"+txtUserName.Text+"' AND password = '"+txtPwd.Text+"'");
            if(result.Length>0)
            {
                EnumClass.UserName = txtUserName.Text;
                EnumClass.isSchool = (result[0]["is_school"].ToString() == ""+1 ? true : false);
                EnumClass.ins_name = result[0]["Ins_Name"].ToString();
                EnumClass.icon = result[0]["ins_logo"].ToString();
                EnumClass.dbName = result[0]["db_name"].ToString();
                this.Hide();
                RajshahiCollegeSmartCard frm = new RajshahiCollegeSmartCard();
                frm.ShowDialog();
                this.Close();
            }
            else
            {
                lblMsg.Text = "Wrong Credentials";
            }

            //using (var contxt = new SmartCardDBEntities())
            //{
            //    if(contxt.tbl_InsUser.ToList().FindAll(x=>x.username==txtUserName.Text && x.password==txtPwd.Text).Count>0)
            //    {
            //        var result = contxt.tbl_InsUser.ToList().Find(x => x.username == txtUserName.Text && x.password == txtPwd.Text);
            //        EnumClass.UserName = txtUserName.Text;
            //        EnumClass.isSchool = (result.is_school == 1 ? true : false);
            //        EnumClass.ins_name = result.Ins_Name;
            //        EnumClass.icon = result.Ins_Logo;
            //        EnumClass.dbName = result.ins_dnname;
            //        this.Hide();
            //        RajshahiCollegeSmartCard frm = new RajshahiCollegeSmartCard();
            //        frm.ShowDialog();
            //        this.Close();
            //    }

            //    else
            //    {
            //        lblMsg.Text = "Wrong Credentials";
            //    }

        }
    }
}

           
