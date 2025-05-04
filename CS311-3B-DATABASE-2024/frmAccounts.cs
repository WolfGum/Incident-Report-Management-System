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
    public partial class frmAccounts : Form
    {
        private String username;
        public frmAccounts(string username)
        {
            InitializeComponent();
            this.username = username;
        }
        Class1 accounts = new Class1("127.0.0.1", "cs3113b2024", "Steve", "123456");

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = accounts.GetData("SELECT username, password, usertype, status, createdby, datecreated FROM tblaccounts WHERE username<> '" + username + "' AND" +
                    "(username LIKE '%" + txtsearch.Text + "%' or usertype LIKE '%" + txtsearch.Text + "%')  ORDER by  username");
                dataGridView1.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private int row;
        private void btnadd_Click(object sender, EventArgs e)
        {
            frmAddNewAccount newaccountform = new frmAddNewAccount(username);
            newaccountform.CaseAdded += () => frmAccounts_Load(sender, e);
            newaccountform.Show();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string selectedUser = dataGridView1.Rows[row].Cells[0].Value.ToString();
                try
                {
                    accounts.executeSQL("DELETE FROM tblaccounts WHERE username = '" + selectedUser + "'");
                    if (accounts.rowAffected > 0)
                    {
                        accounts.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() +
                                "', '" + DateTime.Now.ToShortTimeString() + "', 'DELETE', 'Accounts Management', '" + selectedUser + "', '" + username + "')");
                        MessageBox.Show("Account Deleted", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        frmAccounts_Load(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                row = dataGridView1.SelectedRows[0].Index;
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            string editusername = dataGridView1.Rows[row].Cells[0].Value.ToString();
            string editpassword = dataGridView1.Rows[row].Cells[1].Value.ToString();
            string edittype = dataGridView1.Rows[row].Cells[2].Value.ToString();
            string editstatus = dataGridView1.Rows[row].Cells[3].Value.ToString();
            frmUpdateAccount updateAccountfrm = new frmUpdateAccount(editusername, editpassword, edittype, editstatus, username);
            updateAccountfrm.CaseAdded += () => frmAccounts_Load(sender, e);
            updateAccountfrm.Show();
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            frmAccounts_Load(sender, e);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmAccounts_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = accounts.GetData("SELECT username, password, usertype, status, createdby, datecreated FROM tblaccounts WHERE username<> '" + username + "' ORDER by  username");
                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}