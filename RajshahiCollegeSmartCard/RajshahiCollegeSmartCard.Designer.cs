namespace RajshahiCollegeSmartCard
{
    partial class RajshahiCollegeSmartCard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dataViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.studentDataViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.teacherDataViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blmrsstudentSchoolViewtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataViewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1252, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dataViewToolStripMenuItem
            // 
            this.dataViewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.studentDataViewToolStripMenuItem,
            this.teacherDataViewToolStripMenuItem,
            this.blmrsstudentSchoolViewtoolStripMenuItem});
            this.dataViewToolStripMenuItem.Name = "dataViewToolStripMenuItem";
            this.dataViewToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.dataViewToolStripMenuItem.Text = "Data View";
            // 
            // studentDataViewToolStripMenuItem
            // 
            this.studentDataViewToolStripMenuItem.Name = "studentDataViewToolStripMenuItem";
            this.studentDataViewToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.studentDataViewToolStripMenuItem.Text = "Rajshahi College Student Data View";
            this.studentDataViewToolStripMenuItem.Click += new System.EventHandler(this.studentDataViewToolStripMenuItem_Click);
            // 
            // teacherDataViewToolStripMenuItem
            // 
            this.teacherDataViewToolStripMenuItem.Name = "teacherDataViewToolStripMenuItem";
            this.teacherDataViewToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.teacherDataViewToolStripMenuItem.Text = "Rajshahi College Teacher Data View";
            this.teacherDataViewToolStripMenuItem.Click += new System.EventHandler(this.teacherDataViewToolStripMenuItem_Click);
            // 
            // blmrsstudentSchoolViewtoolStripMenuItem
            // 
            this.blmrsstudentSchoolViewtoolStripMenuItem.Name = "blmrsstudentSchoolViewtoolStripMenuItem";
            this.blmrsstudentSchoolViewtoolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.blmrsstudentSchoolViewtoolStripMenuItem.Text = "Student View";
            this.blmrsstudentSchoolViewtoolStripMenuItem.Click += new System.EventHandler(this.blmrsstudentSchoolViewtoolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1172, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Logout";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // RajshahiCollegeSmartCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1252, 508);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "RajshahiCollegeSmartCard";
            this.Text = "Smart Card Maintanance";
            this.Load += new System.EventHandler(this.RajshahiCollegeSmartCard_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dataViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem studentDataViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem teacherDataViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blmrsstudentSchoolViewtoolStripMenuItem;
        private System.Windows.Forms.Button button1;
    }
}

