using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS311_3B_DATABASE_2024
{
    public partial class frmAddNewStudentAccount : Form
    {
        private string studentid;
        public frmAddNewStudentAccount(string studentid)
        {
            InitializeComponent();

            this.studentid = studentid;
        }

        private int errorCount;
        Class1 newstudentaccount = new Class1("127.0.0.1", "cs3113b2024", "Steve", "123456");
        private void validateForm()
        {
            errorCount = 0;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtstudentid.Text))
            {
                errorProvider1.SetError(txtstudentid, "Input is Empty");
                errorCount++;
            }

            if (string.IsNullOrEmpty(txtlastname.Text))
            {
                errorProvider1.SetError(txtlastname, "Input is Empty");
                errorCount++;
            }
            if (string.IsNullOrEmpty(txtfirstname.Text))
            {
                errorProvider1.SetError(txtfirstname, "Input is Empty");
                errorCount++;
            }
            if (string.IsNullOrEmpty(txtfirstname.Text))
            {
                errorProvider1.SetError(txtfirstname, "Input is Empty");
                errorCount++;
            }

            if (cmblevel1.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmblevel1, "Select Grade/Year Level");
                errorCount++;
            }
            else
            {

                string selectedLevel = cmblevel1.SelectedItem.ToString();


                if (cmbgradeyear.SelectedIndex < 0)
                {
                    errorProvider1.SetError(cmbgradeyear, "Please select a grade year.");
                    errorCount++;
                }


                if (selectedLevel == "SENIOR- HS" || selectedLevel == "COLLEGE")
                {

                    if (cmbstrandcourse.SelectedIndex < 0)
                    {
                        errorProvider1.SetError(cmbstrandcourse, "Please select a strand/course.");
                        errorCount++;
                    }
                }
            }


            try
            {
                DataTable dt = newstudentaccount.GetData("SELECT * FROM tblstudents WHERE studentid = '" + txtstudentid.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtstudentid, "Student ID is already in use");
                    errorCount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on validating existing username", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            if (errorCount == 0)
            {

            }


        }
        public event Action CaseAdded;
        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();

            if (errorCount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to add this Student Account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        // Get the selected code (course or strand)
                        string selectedCode = ((ComboBoxItem)cmbstrandcourse.SelectedItem)?.Code;

                        newstudentaccount.executeSQL("INSERT INTO tblstudents (studentID, lastname, firstname, middlename, level, course, createdby, datecreated) VALUES ('" + txtstudentid.Text +
    "', '" + txtlastname.Text + "','" + txtfirstname.Text + "','" + txtmiddlename.Text + "', '" + cmbgradeyear.Text.ToUpper() + " " + cmblevel1.Text.ToUpper() + "' , '" + selectedCode + "' , '" + studentid + "' , '" + DateTime.Now.ToShortDateString() + "')");
                        if (newstudentaccount.rowAffected > 0)
                        {
                            newstudentaccount.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) " +
                                "VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() + "', 'ADD', 'Student Management', '" + txtstudentid.Text + "', '" + studentid + "')");

                            MessageBox.Show("New student account added!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CaseAdded?.Invoke();
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on saving the new student account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void frmAddNewStudentAccount_Load(object sender, EventArgs e)
        {
            cmbgradeyear.Enabled = false;
            cmbstrandcourse.Enabled = false;
            cmblevel1.Enabled = true;
            this.StartPosition = FormStartPosition.CenterParent;
        }
        private void PopulateComboBox(string type)
        {
            Class1 updatestrandform = new Class1("127.0.0.1", "cs3113b2024", "Steve", "123456");
            cmbstrandcourse.Items.Clear();

            string query = type == "course" ? "SELECT coursecode, description FROM tblcourses" : "SELECT strandcode, description FROM tblstrands";
            DataTable dataTable = updatestrandform.GetData(query);

            foreach (DataRow row in dataTable.Rows)
            {
                cmbstrandcourse.Items.Add(new ComboBoxItem
                {
                    Code = row[type == "course" ? "coursecode" : "strandcode"].ToString(),
                    Description = row["description"].ToString()
                });
            }

            cmbstrandcourse.Enabled = true;
        }
        private void cmblevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmblevel1.SelectedIndex == -1 || cmblevel1.SelectedItem == null)
                return;

            string selectedLevel = cmblevel1.SelectedItem.ToString();
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
                PopulateComboBox("strand");

            }
            else if (selectedLevel == "COLLEGE")
            {
                cmbgradeyear.Items.AddRange(new string[] { "1ST YEAR", "2ND YEAR", "3RD YEAR", "4TH YEAR" });
                cmbgradeyear.Enabled = true;


                PopulateComboBox("course");
            }
        }
        private void txtstudentid_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != '-')
            {
                e.Handled = true;
                errorProvider1.SetError(txtstudentid, "Input must be numeric");
                txtstudentid.Focus();
            }

            else if (e.KeyChar == '-' && txtstudentid.SelectionStart == 0)
            {
                e.Handled = true;
                errorProvider1.SetError(txtstudentid, "Input must be positive number");
                txtstudentid.Focus();

            }
            else
            {
                e.Handled = false;
                errorProvider1.SetError(txtstudentid, "");
            }
        }
        public class ComboBoxItem
        {
            public string Description { get; set; }
            public string Code { get; set; }

            public override string ToString()
            {
                return Description;
            }

        }
        private void btnclear_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            txtstudentid.Clear();
            txtlastname.Clear();
            txtfirstname.Clear();
            txtmiddlename.Clear();
            cmblevel1.SelectedIndex = -1;
            cmbstrandcourse.SelectedIndex = -1;
            cmbstrandcourse.Enabled = false;
            cmbgradeyear.SelectedIndex = -1;
        }

    }

}
