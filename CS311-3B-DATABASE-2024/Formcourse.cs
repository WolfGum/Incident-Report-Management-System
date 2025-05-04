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
    public partial class Frmcourse : Form
    {
        private string username, coursecode;
        public Frmcourse(string username, string coursecode)
        {
            InitializeComponent();
            this.username = username;
            this.coursecode = coursecode;
        }
        Class1 accounts = new Class1("127.0.0.1", "cs3113b2024", "Steve", "123456");

        private int row;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                row = dataGridView1.SelectedRows[0].Index;
            }

        }

        private void Frmcourse_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = accounts.GetData("SELECT coursecode, description, createdby, datecreated FROM tblcourses WHERE coursecode<> '" + coursecode + "' ORDER by  coursecode");
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            frmAddNewCourse newcourseform = new frmAddNewCourse(username, coursecode);
            newcourseform.CaseAdded += () => Frmcourse_Load(sender, e);
            newcourseform.Show();

        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int rowIndex = dataGridView1.SelectedRows[0].Index;
                string editcoursecode = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                string editdescription = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();


                frmUpdateCourse updateCoursefrm = new frmUpdateCourse(username, coursecode, editcoursecode, editdescription);
                updateCoursefrm.CaseAdded += () => Frmcourse_Load(sender, e);
                updateCoursefrm.Show();

            }
            else
            {
                MessageBox.Show("Please select a Course to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete this COURSE?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int rowIndex = dataGridView1.SelectedRows[0].Index;
                    string selectedUser = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                    try
                    {
                        accounts.executeSQL("DELETE FROM tblcourses WHERE coursecode = '" + selectedUser + "'");
                        if (accounts.rowAffected > 0)
                        {
                            accounts.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('"
                                                + DateTime.Now.ToShortDateString() + "', '"
                                                + DateTime.Now.ToShortTimeString() + "', 'DELETE', 'Course Management', '"
                                                + selectedUser + "', '" + username + "')");
                            MessageBox.Show("Course Deleted", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Frmcourse_Load(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("No Course found to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Course to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            Frmcourse_Load(sender, e);
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = accounts.GetData("SELECT coursecode, description, createdby, datecreated FROM tblcourses WHERE coursecode<> '" + coursecode + "' AND" +
                    "(coursecode LIKE '%" + txtsearch.Text + "%' or description LIKE '%" + txtsearch.Text + "%' )  ORDER by  coursecode");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}