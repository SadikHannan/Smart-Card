using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RajshahiCollegeSmartCard
{
    public partial class RajshahiCollegeSmartCard : Form
    {
        public RajshahiCollegeSmartCard()
        {
            InitializeComponent();
        }

        private void teacherDataViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TeacherDataView teacherDataView=new TeacherDataView();
            teacherDataView.ShowDialog();
        }

        private void studentDataViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StudentDataView studentDataView=new StudentDataView();
            studentDataView.ShowDialog();
        }

   

        private void blmrsstudentSchoolViewtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            BlmRsStudentDataView blmsrsStudentView = new BlmRsStudentDataView();
            blmsrsStudentView.ShowDialog();
        }

        private void RajshahiCollegeSmartCard_Load(object sender, EventArgs e)
        {

            teacherDataViewToolStripMenuItem.Visible = false;
            studentDataViewToolStripMenuItem.Visible = false;
            blmrsstudentSchoolViewtoolStripMenuItem.Visible = false;
            String uName = EnumClass.UserName;
            if(!EnumClass.isSchool)
            {
                teacherDataViewToolStripMenuItem.Visible = true;
                studentDataViewToolStripMenuItem.Visible = true;
            }
            else
            {
                blmrsstudentSchoolViewtoolStripMenuItem.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login frm = new Login();
            frm.ShowDialog();
            this.Close();
        }
    }
}
