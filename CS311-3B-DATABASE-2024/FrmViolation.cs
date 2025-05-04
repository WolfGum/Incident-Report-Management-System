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
    public partial class FrmViolation : Form
    {
        private string username, code;
        public FrmViolation(string username, string code)
        {
            InitializeComponent();
            this.username = username;
            this.code = code;
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

        private void btnadd_Click(object sender, EventArgs e)
        {
            FrmAddViolations violation = new FrmAddViolations(username, code);
            violation.CaseAdded += () => FrmViolation_Load(sender, e);
            violation.Show();
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int rowIndex = dataGridView1.SelectedRows[0].Index;

                string editcode = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                string edittype = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();
                string editdescription = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
                string editstatus = dataGridView1.Rows[rowIndex].Cells[3].Value.ToString(); 
                FrmEditViolation editviolationfrm = new FrmEditViolation(username, editcode, edittype, editcode, editdescription, editstatus);
                editviolationfrm.CaseAdded += () => FrmViolation_Load(sender, e);
                editviolationfrm.Show();
            }
            else
            {
                MessageBox.Show("Please select a Course to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete this VIOLATION?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int rowIndex = dataGridView1.SelectedRows[0].Index;
                    string selectedUser = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                    try
                    {
                        accounts.executeSQL("DELETE FROM tblviolations WHERE code = '" + selectedUser + "'");
                        if (accounts.rowAffected > 0)
                        {
                            accounts.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('"
                                                + DateTime.Now.ToShortDateString() + "', '"
                                                + DateTime.Now.ToShortTimeString() + "', 'DELETE', 'Violations Management', '"
                                                + selectedUser + "', '" + username + "')");
                            MessageBox.Show("Violation Deleted", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FrmViolation_Load(sender, e);
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
                    MessageBox.Show("Please select a Violation deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            FrmViolation_Load(sender, e);
        }

        private void FrmViolation_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = accounts.GetData("SELECT code, type, description, status, createdby, datecreated FROM tblviolations WHERE code<> '" + code + "' ORDER by  code");
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = accounts.GetData("SELECT code, type, description, status, createdby, datecreated FROM tblviolations WHERE code<> '" + code + "' AND" +
                    "(code LIKE '%" + txtsearch.Text + "%' or type LIKE '%" + txtsearch.Text + "%' )  ORDER by  code");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}