using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS311_3B_DATABASE_2024
{

    public partial class frmAddNewCourse : Form
    {
        private string username, coursecode;
        public frmAddNewCourse(string username, string coursecode)
        {
            InitializeComponent();
            this.username = username;
            this.coursecode = coursecode;
        }
        private int errorCount;
        Class1 newcourseform = new Class1("127.0.0.1", "cs3113b2024", "Steve", "123456");
        private void validateForm()
        {
            errorCount = 0;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtcoursecode.Text))
            {
                errorProvider1.SetError(txtcoursecode, "Input is Empty");
                errorCount++;
            }

            if (string.IsNullOrEmpty(txtdescription.Text))
            {
                errorProvider1.SetError(txtdescription, "Input is Empty");
                errorCount++;
            }

            try
            {
                DataTable dt = newcourseform.GetData("SELECT * FROM tblcourses WHERE coursecode = '" + txtcoursecode.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtcoursecode, "Course code is already in use");
                    errorCount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on validating existing username", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cmbdescription_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            txtcoursecode.Clear();
            txtdescription.Clear();
            errorProvider1.Clear();
        }

        private void frmAddNewCourse_Load(object sender, EventArgs e)
        {

        }
        public event Action CaseAdded;
        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();


            if (errorCount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to add this COURSE?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {

                        newcourseform.executeSQL("INSERT INTO tblcourses (coursecode, description, createdby, datecreated) VALUES ('" + txtcoursecode.Text + "' , '" +
                             txtdescription.Text + "' , '" + username + "' , '" + DateTime.Now.ToShortDateString() + "')");
                        if (newcourseform.rowAffected > 0)
                        {

                            newcourseform.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) " +
                                "VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() + "', 'ADD', 'Course Management', '" + txtcoursecode.Text + "', '" + username + "')");

                            MessageBox.Show("New Course account added!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}