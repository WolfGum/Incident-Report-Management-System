using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CS311_3B_DATABASE_2024
{
    public partial class Frmstudentsaccounts : Form
    {
        private string studentid;
        public Frmstudentsaccounts(string studentid)
        {
            InitializeComponent();

            this.studentid = studentid;
        }
        Class1 accounts = new Class1("127.0.0.1", "cs3113b2024", "Steve", "123456");
        private void Frmstudentsaccounts_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = accounts.GetData("SELECT studentid, lastname, firstname, middlename, level, course, createdby, datecreated FROM tblstudents WHERE studentid<> '" + studentid + "' ORDER by  studentid");
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private int row;

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                row = dataGridView1.SelectedRows[0].Index;
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            frmAddNewStudentAccount newaccountform = new frmAddNewStudentAccount(studentid);
            newaccountform.CaseAdded += () => Frmstudentsaccounts_Load(sender, e);
            newaccountform.Show();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int rowIndex = dataGridView1.SelectedRows[0].Index;
                string editstudentid = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                string editlastname = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();
                string editfirstname = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
                string editmiddlename = dataGridView1.Rows[rowIndex].Cells[3].Value.ToString();
                string editlevel = dataGridView1.Rows[rowIndex].Cells[4].Value.ToString();
                string editgradeyear = dataGridView1.Rows[rowIndex].Cells[4].Value.ToString();
                string editstrandandcourse = dataGridView1.Rows[rowIndex].Cells[5].Value.ToString();

                frmUpdateStudentAccount1 updateStudentAccountfrm = new frmUpdateStudentAccount1(editstudentid, editlastname, editfirstname, editmiddlename, editlevel, editstrandandcourse, editgradeyear, studentid);
                updateStudentAccountfrm.CaseAdded += () => Frmstudentsaccounts_Load(sender, e);
                updateStudentAccountfrm.Show();
                this.StartPosition = FormStartPosition.CenterParent;
            }
            else
            {
                MessageBox.Show("Please select a student account to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int rowIndex = dataGridView1.SelectedRows[0].Index;
                    string selectedUser = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                    try
                    {
                        accounts.executeSQL("DELETE FROM tblstudents WHERE studentid = '" + selectedUser + "'");
                        if (accounts.rowAffected > 0)
                        {
                            accounts.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('"
                                                + DateTime.Now.ToShortDateString() + "', '"
                                                + DateTime.Now.ToShortTimeString() + "', 'DELETE', 'Student Management', '"
                                                + selectedUser + "', '" + studentid + "')");
                            MessageBox.Show("Student Account Deleted", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Frmstudentsaccounts_Load(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("No account found to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select an account to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            Frmstudentsaccounts_Load(sender, e);
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = accounts.GetData("SELECT studentid, lastname, firstname, middlename, level, course, createdby, datecreated FROM tblstudents WHERE studentid<> '" + studentid + "' AND" +
                    "(studentid LIKE '%" + txtsearch.Text + "%' or lastname LIKE '%" + txtsearch.Text + "%' or course LIKE '%" + txtsearch.Text + "%' )  ORDER by  studentid");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}