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
    public partial class FrmUpdateStrand : Form
    {
        private string username, strandcode, editstrandcode, editdescription;
        public FrmUpdateStrand(string username, string strandcode, string editstrandcode, string editdescription)
        {
            InitializeComponent();
            this.username = username;
            this.strandcode = strandcode;
            this.editstrandcode = editstrandcode;
            this.editdescription = editdescription;
        }
        private int errorCount;
        Class1 updatestrandform = new Class1("127.0.0.1", "cs3113b2024", "Steve", "123456");

        private void btnclear_Click(object sender, EventArgs e)
        {

            txtdescription.Clear();
            txtstrandcode.Clear();
            errorProvider1.Clear();
        }

        private void validateForm()
        {
            errorCount = 0;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtstrandcode.Text))
            {
                errorProvider1.SetError(txtstrandcode, "Input is Empty");
                errorCount++;
            }
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtdescription.Text))
            {
                errorProvider1.SetError(txtdescription, "Input is Empty");
                errorCount++;
            }

        }

        private void FrmUpdateStrand_Load(object sender, EventArgs e)
        {
            txtstrandcode.Text = editstrandcode;
            txtdescription.Text = editdescription;
        }
        public event Action CaseAdded;
        private void btnsave_Click(object sender, EventArgs e)
        {
            validateForm();
            if (errorCount == 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to update this STRAND?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        updatestrandform.executeSQL("UPDATE tblstrands SET description = '" + txtdescription.Text + "' WHERE strandcode = '" + txtstrandcode.Text + "'");
                        if (updatestrandform.rowAffected > 0)
                        {
                            updatestrandform.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() +
                                "', '" + DateTime.Now.ToShortTimeString() + "', 'UPDATE', 'Strand Management', '" + txtstrandcode.Text + "', '" + username + "')");
                            MessageBox.Show("Strand Updated", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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