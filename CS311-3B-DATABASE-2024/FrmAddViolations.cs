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
    public partial class FrmAddViolations : Form
    {
        private string username, code;
        public FrmAddViolations(string username, string code)
        {
            InitializeComponent();
            this.username = username;
            this.code = code;
        }
        private int errorCount;
        Class1 newstrandform = new Class1("127.0.0.1", "cs3113b2024", "Steve", "123456");
        private void validateForm()
        {
            errorCount = 0;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtcode.Text))
            {
                errorProvider1.SetError(txtcode, "Input is Empty");
                errorCount++;
            }

            if (string.IsNullOrEmpty(txtdescription.Text))
            {
                errorProvider1.SetError(txtdescription, "Input is Empty");
                errorCount++;
            }
            if (cmbtype.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbtype, "Input is Empty");
                errorCount++;
            }

            try
            {
                DataTable dt = newstrandform.GetData("SELECT * FROM tblviolations WHERE code = '" + txtcode.Text + "'");
                if (dt.Rows.Count > 0)
                {
                    errorProvider1.SetError(txtcode, "code is already in use");
                    errorCount++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on validating existing username", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            txtcode.Clear();
            cmbtype.SelectedIndex = -1;
            txtdescription.Clear();
            errorProvider1.Clear();
        }

        private void FrmAddViolations_Load(object sender, EventArgs e)
        {

        }
        public event Action CaseAdded;
        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorCount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to add this VIOLATION?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {

                        newstrandform.executeSQL("INSERT INTO tblviolations (code, type, description, status, createdby, datecreated) VALUES ('" + txtcode.Text + "' , '" + cmbtype.Text.ToUpper() + "' , '" +
                             txtdescription.Text + "' , 'ACTIVE' , '" + username + "' , '" + DateTime.Now.ToShortDateString() + "')");
                        if (newstrandform.rowAffected > 0)
                        {

                            newstrandform.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) " +
                                "VALUES ('" + DateTime.Now.ToShortDateString() + "', '" + DateTime.Now.ToShortTimeString() + "', 'ADD', 'Violation Management', '" + txtcode.Text + "', '" + username + "')");

                            MessageBox.Show("New Violation Added!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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