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
    public partial class Frmstrand : Form
    {
        private string username, strandcode;
        public Frmstrand(string username, string strandcode)
        {
            InitializeComponent();
            this.username = username;
            this.strandcode = strandcode;
        }
        Class1 accounts = new Class1("127.0.0.1", "cs3113b2024", "Steve", "123456");

        private int row;
        private void Frmstrand_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = accounts.GetData("SELECT strandcode, description, createdby, datecreated FROM tblstrands WHERE strandcode<> '" + strandcode + "' ORDER by  strandcode");
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
                DataTable dt = accounts.GetData("SELECT strandcode, description, createdby, datecreated FROM tblstrands WHERE strandcode<> '" + strandcode + "' AND" +
                    "(strandcode LIKE '%" + txtsearch.Text + "%' or description LIKE '%" + txtsearch.Text + "%' )  ORDER by  strandcode");
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            FrmAddStrand AddStrandfrm = new FrmAddStrand(username, strandcode);
            AddStrandfrm.CaseAdded += () => Frmstrand_Load(sender, e);
            AddStrandfrm.Show();

        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int rowIndex = dataGridView1.SelectedRows[0].Index;
                string editstrandcode = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                string editdescription = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();


                FrmUpdateStrand updateStrandfrm = new FrmUpdateStrand(username, strandcode, editstrandcode, editdescription);
                updateStrandfrm.CaseAdded += () => Frmstrand_Load(sender, e);
                updateStrandfrm.Show();

            }
            else
            {
                MessageBox.Show("Please select a Course to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete this STRAND?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int rowIndex = dataGridView1.SelectedRows[0].Index;
                    string selectedUser = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                    try
                    {
                        accounts.executeSQL("DELETE FROM tblstrands WHERE strandcode = '" + selectedUser + "'");
                        if (accounts.rowAffected > 0)
                        {
                            accounts.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('"
                                                + DateTime.Now.ToShortDateString() + "', '"
                                                + DateTime.Now.ToShortTimeString() + "', 'DELETE', 'Strand Management', '"
                                                + selectedUser + "', '" + username + "')");
                            MessageBox.Show("Strand Deleted", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Frmstrand_Load(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("No Strand found to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Strand to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            Frmstrand_Load(sender, e);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                row = dataGridView1.SelectedRows[0].Index;
            }
        }
    }
}