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

namespace CS311_3B_DATABASE_2024
{
    public partial class frmUpdateAccount : Form
    {

        private string username, editusername, editpassword, edittype, editstatus;
        private int errorcount;
        public event Action CaseAdded;
        private void btnsave_Click(object sender, EventArgs e)
        {
            validateform();
            if (errorcount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to update this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        updateaccount.executeSQL("UPDATE tblaccounts SET password = '" + txtpassword.Text + "', usertype = '" + cmbtype.Text.ToUpper() + "', status = '" +
                            cmbstatus.Text.ToUpper() + "' WHERE username = '" + txtusername.Text + "'");
                        if (updateaccount.rowAffected > 0)
                        {
                            updateaccount.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() +
                                "', '" + DateTime.Now.ToShortTimeString() + "', 'UPDATE', 'Accounts Management', '" + txtusername.Text + "', '" + username + "')");
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

        private void btnclear_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            txtusername.Clear();
            txtpassword.Clear();
            cmbstatus.SelectedIndex = -1;
            cmbtype.SelectedIndex = -1;
        }

        private void chkshowpassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkshowpassword.Checked == true)
            {
                txtpassword.PasswordChar = '\0';
            }
            else
            {
                txtpassword.PasswordChar = '*';
            }
        }

        Class1 updateaccount = new Class1("127.0.0.1", "cs3113b2024", "Steve", "123456");
        private void validateform()
        {
            errorcount = 0;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtpassword.Text))
            {
                errorProvider1.SetError(txtpassword, "Input is Empty.");
                errorcount++;
            }
            else if (txtpassword.TextLength < 6)
            {
                errorProvider1.SetError(txtpassword, "Password should be at least 6 characters.");
                errorcount++;
            }
            if (cmbtype.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbtype, "Select User Type.");
                errorcount++;
            }
            if (cmbstatus.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbtype, "Select Status.");
                errorcount++;
            }
        }
        public frmUpdateAccount(string editusername, string editpassword, string edittype, string editstatus, string username)
        {
            InitializeComponent();
            this.username = username;
            this.editpassword = editpassword;
            this.edittype = edittype;
            this.editstatus = editstatus;
            this.editusername = editusername;
        }

        public frmUpdateAccount()
        {
            InitializeComponent();
        }

        private void frmUpdateAccount_Load(object sender, EventArgs e)
        {
            txtusername.Text = editusername;
            txtpassword.Text = editpassword;
            if (edittype == "ADMINISTRATOR")
            {
                cmbtype.SelectedIndex = 0;
            }
            else if (edittype == "BRANCHADMIN")
            {
                cmbtype.SelectedIndex = 1;
            }
            else
            {
                cmbtype.SelectedIndex = 2;
            }
            if (editstatus == "ACTIVE")
            {
                cmbstatus.SelectedIndex = 0;
            }
            else
            {
                cmbstatus.SelectedIndex = 1;
            }
        }
    }
}