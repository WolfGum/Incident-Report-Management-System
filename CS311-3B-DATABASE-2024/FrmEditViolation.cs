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
    public partial class FrmEditViolation : Form
    {
        private string username, code, type, editcode, edittype, editdescription, editstatus;
        public FrmEditViolation(string username, string code, string edittype, string editcode, string editdescription, string editstatus)
        {
            InitializeComponent();
            this.username = username;
            this.code = code;
            this.editcode = editcode;
            this.edittype = edittype;
            this.editdescription = editdescription;
            this.editstatus = editstatus;
        }

        private void FrmEditViolation_Load(object sender, EventArgs e)
        {
            txtcode.Text = editcode;
            if (edittype == "MINOR OFFENSE")
            {
                cmbtype.SelectedIndex = 0;
            }
            else
            {
                cmbtype.SelectedIndex = 1;
            }
            txtdescription.Text = editdescription;

            if (editstatus == "ACTIVE")
            {
                cmbstatus.SelectedIndex = 0;
            }
            else
            {
                cmbstatus.SelectedIndex = 1;
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            txtcode.Clear();
            cmbtype.SelectedIndex = -1;
            txtdescription.Clear();
            cmbstatus.SelectedIndex = -1;
            errorProvider1.Clear();
        }

        private int errorCount;
        Class1 editviolation = new Class1("127.0.0.1", "cs3113b2024", "Steve", "123456");
        private void validateForm()
        {
            errorCount = 0;
            errorProvider1.Clear();


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

        }
        public event Action CaseAdded;
        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorCount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to edit this VIOLATION?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        editviolation.executeSQL("UPDATE tblviolations SET type = '" + cmbtype.Text.ToUpper() + "', description = '" + txtdescription.Text + "', status = '" +
                            cmbstatus.Text.ToUpper() + "' WHERE code = '" + txtcode.Text + "'");
                        if (editviolation.rowAffected > 0)
                        {
                            editviolation.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() +
                                "', '" + DateTime.Now.ToShortTimeString() + "', 'EDIT', 'Violation Management', '" + txtcode.Text + "', '" + username + "')");
                            MessageBox.Show("Violation Edited", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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