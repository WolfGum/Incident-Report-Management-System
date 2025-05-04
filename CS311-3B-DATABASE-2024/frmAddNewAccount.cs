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
    public partial class frmAddNewAccount : Form
    {
        private string username;
        public frmAddNewAccount(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmAddNewAccount_Load(object sender, EventArgs e)
        {

        }

        private int errorCount;
        Class1 newaccount = new Class1("127.0.0.1", "cs3113b2024", "Steve", "123456");

        private void validateForm()
        {
            errorCount = 0;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtusername.Text))
            {
                errorProvider1.SetError(txtusername, "Input is Empty");
                errorCount++;
            }
            if (string.IsNullOrEmpty(txtpassword.Text))
            {

                errorProvider1.SetError(txtpassword, "Input is Empty");
                errorCount++;
            }
            if (txtpassword.Text.Length < 6)
            {
                errorProvider1.SetError(txtpassword, "Password should be atleast 6 characters");
                errorCount++;
            }
            if (cmbusertype.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbusertype, "Select Usertype");
                errorCount++;
            }
            try
            {
                DataTable dt = newaccount.GetData("SELECT * FROM tblaccounts WHERE username = '" + txtusername.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtusername, "Username is already in use");
                    errorCount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on validating existing username", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }
        public event Action CaseAdded;
        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorCount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to add this account?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        newaccount.executeSQL("INSERT INTO tblaccounts (username, password, usertype, status, createdby, datecreated) VALUES ('" + txtusername.Text +
                            "', '" + txtpassword.Text + "', '" + cmbusertype.Text.ToUpper() + "' , 'ACTIVE' , '" + username + "' , '" + DateTime.Now.ToShortDateString() + "')");
                        if (newaccount.rowAffected > 0)
                        {
                            newaccount.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() +
                                "', '" + DateTime.Now.ToShortTimeString() + "', 'ADD', 'Accounts Management', '" + txtusername.Text + "', '" + username + "')");
                            MessageBox.Show("New account Added", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CaseAdded?.Invoke();
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on Validating existing username", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
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
    }
}