using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CS311_3B_DATABASE_2024.frmAddNewStudentAccount;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CS311_3B_DATABASE_2024
{
    public partial class frmUpdateStudentAccount1 : Form
    {
        private string studentid, editlastname, editfirstname, editmiddlename, editstudentid, editlevel, editgradeyear, editstrandandcourse;
        private int errorcount;
        public frmUpdateStudentAccount1(string editstudentid, string editlastname, string editfirstname, string editmiddlename, string editlevel, string editstrandandcourse, string editgradeyear, string studentid)
        {
            InitializeComponent();

            this.studentid = studentid;
            this.editlastname = editlastname;
            this.editfirstname = editfirstname;
            this.editmiddlename = editmiddlename;
            this.editstudentid = editstudentid;
            this.editlevel = editlevel;
            this.editgradeyear = editgradeyear;
            this.editstrandandcourse = editstrandandcourse;

        }
        private string lastSelectedCourse = "";

        private void cmblevel1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmblevel.SelectedIndex == -1 || cmblevel.SelectedItem == null)
                return;

            string selectedLevel = cmblevel.SelectedItem.ToString();
            cmbgradeyear.Items.Clear();
            cmbgradeyear.Text = "";
            cmbstrandcourse.Items.Clear();
            cmbstrandcourse.Text = "";
            cmbstrandcourse.Enabled = false;
            cmbgradeyear.Enabled = false;

            if (selectedLevel == "ELEMENTARY")
            {
                cmbgradeyear.Items.AddRange(new string[] { "GRADE 1", "GRADE 2", "GRADE 3", "GRADE 4", "GRADE 5", "GRADE 6" });
                cmbgradeyear.Enabled = true;
            }
            else if (selectedLevel == "JUNIOR- HS")
            {
                cmbgradeyear.Items.AddRange(new string[] { "GRADE 7", "GRADE 8", "GRADE 9", "GRADE 10" });
                cmbgradeyear.Enabled = true;
            }
            else if (selectedLevel == "SENIOR- HS")
            {
                cmbgradeyear.Items.AddRange(new string[] { "GRADE 11", "GRADE 12" });
                cmbgradeyear.Enabled = true;
                cmbstrandcourse.Enabled = true;
                PopulateStrands("SENIOR- HS");
            }
            else if (selectedLevel == "COLLEGE")
            {
                cmbgradeyear.Items.AddRange(new string[] { "1ST YEAR", "2ND YEAR", "3RD YEAR", "4TH YEAR" });
                cmbgradeyear.Enabled = true;


                PopulateCourses("COLLEGE");
            }


        }




        private void btnclear_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            txtlastname.Clear();
            txtfirstname.Clear();
            txtmiddlename.Clear();
            cmblevel.SelectedIndex = -1;
            cmbgradeyear.SelectedIndex = -1;
            cmbstrandcourse.SelectedIndex = -1;
            cmbgradeyear.Enabled = false;
            cmbstrandcourse.Enabled = false;

        }
        private void SetGradeYearForElementary(string editgradeyear)
        {
            switch (editgradeyear)
            {
                case "GRADE 1 ELEMENTARY": cmbgradeyear.SelectedIndex = 0; break;
                case "GRADE 2 ELEMENTARY": cmbgradeyear.SelectedIndex = 1; break;
                case "GRADE 3 ELEMENTARY": cmbgradeyear.SelectedIndex = 2; break;
                case "GRADE 4 ELEMENTARY": cmbgradeyear.SelectedIndex = 3; break;
                case "GRADE 5 ELEMENTARY": cmbgradeyear.SelectedIndex = 4; break;
                case "GRADE 6 ELEMENTARY": cmbgradeyear.SelectedIndex = 5; break;
                default: cmbgradeyear.SelectedIndex = -1; break;
            }
        }

        private void SetGradeYearForJuniorHigh(string editgradeyear)
        {
            switch (editgradeyear)
            {
                case "GRADE 7 JUNIOR- HS": cmbgradeyear.SelectedIndex = 0; break;
                case "GRADE 8 JUNIOR- HS": cmbgradeyear.SelectedIndex = 1; break;
                case "GRADE 9 JUNIOR- HS": cmbgradeyear.SelectedIndex = 2; break;
                case "GRADE 10 JUNIOR- HS": cmbgradeyear.SelectedIndex = 3; break;
                default: cmbgradeyear.SelectedIndex = -1; break;
            }
        }

        private void SetGradeYearForCollege(string editgradeyear)
        {
            switch (editgradeyear)
            {
                case "1ST YEAR COLLEGE": cmbgradeyear.SelectedIndex = 0; break;
                case "2ND YEAR COLLEGE": cmbgradeyear.SelectedIndex = 1; break;
                case "3RD YEAR COLLEGE": cmbgradeyear.SelectedIndex = 2; break;
                case "4TH YEAR COLLEGE": cmbgradeyear.SelectedIndex = 3; break;
                default: cmbgradeyear.SelectedIndex = -1; break;
            }
        }

        private void validateform()
        {
            errorcount = 0;
            errorProvider1.Clear();


            if (string.IsNullOrEmpty(txtlastname.Text))
            {
                errorProvider1.SetError(txtlastname, "Input is Empty.");
                errorcount++;
            }


            if (string.IsNullOrEmpty(txtfirstname.Text))
            {
                errorProvider1.SetError(txtfirstname, "Input is Empty.");
                errorcount++;
            }


            if (cmbgradeyear.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbgradeyear, "Please select a level.");
                errorcount++;
            }
            else
            {
                string selectedLevel = cmblevel.SelectedItem.ToString();
                if (cmbgradeyear.SelectedIndex < 0)
                {
                    errorProvider1.SetError(cmbgradeyear, "Please select a grade year.");
                    errorcount++;
                }


                if (selectedLevel == "SENIOR- HS" || selectedLevel == "COLLEGE")
                {

                    if (cmbstrandcourse.SelectedIndex < 0)
                    {
                        errorProvider1.SetError(cmbstrandcourse, "Please select a strand/course.");
                        errorcount++;
                    }
                }
            }
        }
        private void frmUpdateStudentAccount1_Load(object sender, EventArgs e)
        {
            txtstudentid.Text = editstudentid;
            txtlastname.Text = editlastname;
            txtfirstname.Text = editfirstname;
            txtmiddlename.Text = editmiddlename;
            cmbstrandcourse.Items.Clear();
            cmbgradeyear.Items.Clear();
            cmbstrandcourse.Enabled = false;

            if (editlevel == "GRADE 1 ELEMENTARY" || editlevel == "GRADE 2 ELEMENTARY" || editlevel == "GRADE 3 ELEMENTARY"
                || editlevel == "GRADE 4 ELEMENTARY" || editlevel == "GRADE 5 ELEMENTARY" || editlevel == "GRADE 6 ELEMENTARY")
            {
                cmblevel.SelectedIndex = 0;
                cmbstrandcourse.SelectedIndex = -1;


                SetGradeYearForElementary(editgradeyear);
            }
            else if (editlevel == "GRADE 7 JUNIOR- HS" || editlevel == "GRADE 8 JUNIOR- HS" || editlevel == "GRADE 9 JUNIOR- HS" || editlevel == "GRADE 10 JUNIOR- HS")
            {
                cmblevel.SelectedIndex = 1;
                cmbstrandcourse.SelectedIndex = -1;


                SetGradeYearForJuniorHigh(editgradeyear);
            }
            else if (editlevel == "GRADE 11 SENIOR- HS" || editlevel == "GRADE 12 SENIOR- HS")
            {
                cmblevel.SelectedIndex = 2;


                cmbgradeyear.SelectedIndex = editgradeyear == "GRADE 11 SENIOR- HS" ? 0 : 1;


                PopulateStrands("SENIOR-HS");


                SetSelectedStrand(editstrandandcourse);
            }
            else
            {
                cmblevel.SelectedIndex = 3;


                SetGradeYearForCollege(editgradeyear);
                PopulateCourses("COLLEGE");
                SetSelectedCourse(editstrandandcourse);
            }

        }



        private void PopulateCourses(string level)
        {

            string query = "SELECT coursecode, description FROM tblcourses";
            DataTable coursesTable = updatestudentaccount.GetData(query);

            cmbstrandcourse.Items.Clear();

            foreach (DataRow row in coursesTable.Rows)
            {
                var item = new ComboBoxItem
                {
                    Description = row["description"].ToString(),
                    CourseCode = row["coursecode"].ToString()
                };

                cmbstrandcourse.Items.Add(item);
            }

            cmbstrandcourse.Enabled = true;
        }
        private void PopulateStrands(string level)
        {

            string query = "SELECT strandcode, description FROM tblstrands";
            DataTable strandsTable = updatestudentaccount.GetData(query);

            cmbstrandcourse.Items.Clear();

            foreach (DataRow row in strandsTable.Rows)
            {
                var item = new ComboBoxItem1
                {
                    Description = row["description"].ToString(),
                    StrandCode = row["strandcode"].ToString()
                };

                cmbstrandcourse.Items.Add(item);
            }

            cmbstrandcourse.Enabled = true;
        }
        private void SetSelectedCourse(string editstrandandcourse)
        {
            foreach (var item in cmbstrandcourse.Items)
            {
                var comboBoxItem = item as ComboBoxItem;
                if (comboBoxItem != null && comboBoxItem.CourseCode == editstrandandcourse)
                {
                    cmbstrandcourse.SelectedItem = comboBoxItem;
                    break;
                }
            }
        }
        private void SetSelectedStrand(string editstrandandcourse)
        {
            foreach (var item in cmbstrandcourse.Items)
            {
                var comboBoxItem = item as ComboBoxItem1;
                if (comboBoxItem != null && comboBoxItem.StrandCode == editstrandandcourse)
                {
                    cmbstrandcourse.SelectedItem = comboBoxItem;
                    break;
                }
            }
        }
        public class ComboBoxItem
        {
            public string Description { get; set; }
            public string CourseCode { get; set; }

            public override string ToString()
            {
                return Description;
            }
        }

        public class ComboBoxItem1
        {
            public string Description { get; set; }
            public string StrandCode { get; set; }

            public override string ToString()
            {
                return Description;
            }
        }
        Class1 updatestudentaccount = new Class1("127.0.0.1", "cs3113b2024", "Steve", "123456");
        public event Action CaseAdded;
        private void btnsave_Click(object sender, EventArgs e)
        {
            validateform();
            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to update this Student Account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        string selectedCode;
                        if (cmblevel.SelectedIndex == 2)
                        {
                            selectedCode = ((ComboBoxItem1)cmbstrandcourse.SelectedItem)?.StrandCode;
                        }
                        else
                        {
                            selectedCode = ((ComboBoxItem)cmbstrandcourse.SelectedItem)?.CourseCode;
                        }

                        updatestudentaccount.executeSQL("UPDATE tblstudents SET " + "lastname = '" + txtlastname.Text + "', " + "firstname = '" + txtfirstname.Text + "', " + "middlename = '" + txtmiddlename.Text + "', " + "level = '" + cmbgradeyear.Text.ToUpper() + " " + cmblevel.Text.ToUpper() + "', " +
                          "course = '" + selectedCode + "' " + "WHERE studentid = '" + txtstudentid.Text + "'");

                        if (updatestudentaccount.rowAffected > 0)
                        {
                            updatestudentaccount.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" +
                                DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() + "', 'UPDATE', 'Student Management', '" + txtstudentid.Text + "', '" + studentid + "')");

                            MessageBox.Show("Student Account Updated", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CaseAdded?.Invoke();
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
