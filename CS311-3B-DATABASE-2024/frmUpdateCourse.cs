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
    public partial class frmUpdateCourse : Form
    {
        private string username, coursecode, editcoursecode, editdescription;
        public frmUpdateCourse(string username, string coursecode, string editcoursecode, string editdescription)
        {
            InitializeComponent();
            this.username = username;
            this.coursecode = coursecode;
            this.editcoursecode = editcoursecode;
            this.editdescription = editdescription;
        }
        private int errorCount;
        Class1 updatecourse = new Class1("127.0.0.1", "cs3113b2024", "Steve", "123456");

        private void btnclear_Click(object sender, EventArgs e)
        {
            txtdescription.Clear();
            errorProvider1.Clear();
            txtcoursecode.Clear();
        }

        private void validateForm()
        {
            errorCount = 0;
            errorProvider1.Clear();

            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtdescription.Text))
            {
                errorProvider1.SetError(txtdescription, "Input is Empty");
                errorCount++;
            }

        }

        private void frmUpdateCourse_Load(object sender, EventArgs e)
        {
            txtcoursecode.Text = editcoursecode;
            txtdescription.Text = editdescription;
        }
        public event Action CaseAdded;
        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorCount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to update this COURSE?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        updatecourse.executeSQL("UPDATE tblcourses SET description = '" + txtdescription.Text + "' WHERE coursecode = '" + txtcoursecode.Text + "'");
                        if (updatecourse.rowAffected > 0)
                        {
                            updatecourse.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() +
                                "', '" + DateTime.Now.ToShortTimeString() + "', 'UPDATE', 'Course Management', '" + txtcoursecode.Text + "', '" + username + "')");
                            MessageBox.Show("Account Updated", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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