using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CS311_3B_DATABASE_2024.frmUpdateStudentAccount1;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace CS311_3B_DATABASE_2024
{
    public partial class FrmUpdateCase : Form
    {
        private string username, caseid, editcaseid, studentid, editlastname, editfirstname, editmiddlename, editlevel, editstrandcourse, editviolationid,
        editviolationcount, editdescription, edityear, editconcernlevel, editdiscplinaryaction, editstatus, editaction;

        private void btnclear_Click(object sender, EventArgs e)
        {
            txtdescription.Clear();
            txtcount.Clear();
            cmbcode.SelectedIndex = -1;
            txtyear.Clear();
            cmbconcern.SelectedIndex = -1;
            txtdiscplinaryaction.Clear();
        }

        private void cmbstatus_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmbstatus.SelectedIndex == 1)
            {
                txtaction.Enabled = true;
            }
            else
            {
                txtaction.Enabled = false;
                txtaction.Clear();
            }
        }
        private Class1 updatestrandform = new Class1("127.0.0.1", "cs3113b2024", "Steve", "123456");
        private int errorCount;
        public FrmUpdateCase(string username, string caseid, string editcaseid, string studentid, string lastname, string firstname, string middlename,
            string level, string course, string editviolationid, string editviolationcount, string editdescription, string edityear, string editconcernlevel, string editdiscplinaryaction,  string editstatus, string editaction)
        {
            InitializeComponent();
            this.username = username;
            this.caseid = caseid;
            this.editcaseid = editcaseid;
            txtstudentid.Text = studentid;
            txtlastname.Text = lastname;
            txtfirstname.Text = firstname;
            txtmiddlename.Text = middlename;
            txtlevel.Text = level;
            txtstrandcourse.Text = course;
            this.editviolationid = editviolationid;
            this.editviolationcount = editviolationcount;
            this.editdescription = editdescription;
            this.edityear = edityear;
            this.editconcernlevel = editconcernlevel;
            this.editdiscplinaryaction = editdiscplinaryaction;
            this.editstatus = editstatus;
            this.editaction = editaction;
        }
        private void FrmUpdateCase_Load(object sender, EventArgs e)
        {
            txtcase.Text = editcaseid;
            PopulateViolationId();
            txtcount.Text = editviolationcount;
            txtdescription.Text = editdescription;
            txtyear.Text = edityear;


            if (editconcernlevel == "Prefect of Discpline")
            {
                cmbconcern.SelectedIndex = 0;
            }
            else if (editconcernlevel == "Branch OSA")
            {
                cmbconcern.SelectedIndex = 1;
            }
            else if (editconcernlevel == "Dean of Student Affairs")
            {
                cmbconcern.SelectedIndex = 2;
            }
            else
            {
                cmbconcern.SelectedIndex = 3;
            }
            txtdiscplinaryaction.Text = editdiscplinaryaction;
            if (editstatus == "On going")
            {
                cmbstatus.SelectedIndex = 0;
                txtaction.Enabled = false;
                txtaction.Clear();
            }
            else
            {
                cmbstatus.SelectedIndex = 1;
                txtaction.Enabled = true;

            }
            txtaction.Text = editaction;

           

        }
        
        private void PopulateViolationId()
        {
            cmbcode.Items.Clear();
            string query = "SELECT code, type, description FROM tblviolations";
            DataTable dataTable = updatestrandform.GetData(query);

            foreach (DataRow row in dataTable.Rows)
            {
                ComboBoxItem comboItem = new ComboBoxItem
                {
                    Code = row["code"].ToString(),
                    Type = row["type"].ToString(),
                    Description = row["description"].ToString()
                };
                cmbcode.Items.Add(comboItem);
                if (editviolationid == comboItem.Code)
                {
                    cmbcode.SelectedItem = comboItem;
                    txtdescription.Text = comboItem.Description;
                    txtcount.Text = editviolationcount;
                }
            }
        }
        private void cmbcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtcount.Clear();
            errorProvider1.SetError(txtcount, string.Empty);

            if (cmbcode.SelectedItem is ComboBoxItem selectedItem)
            {
                txtdescription.Text = selectedItem.Description;
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
                        if (cmbcode.SelectedItem is ComboBoxItem selectedItem)
                        {
                            updatestrandform.executeSQL("UPDATE tblcases SET studentid = '" + txtstudentid.Text + "', code = '" + selectedItem.Code.ToUpper() +
                                "', description = '" + txtdescription.Text + "', type = '" + selectedItem.Type + "', count = '" +
                                txtcount.Text + "', schoolyear = '" + txtyear.Text + "', concernlevel = '" + cmbconcern.Text + "', discplinaryaction = '" + txtdiscplinaryaction.Text + "', status = '" + cmbstatus.Text + "', action = '" + txtaction.Text +
                                "' WHERE caseid = '" + txtcase.Text + "'");

                            if (updatestrandform.rowAffected > 0)
                            {
                                updatestrandform.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) VALUES ('" + DateTime.Now.ToShortDateString() +
                                    "', '" + DateTime.Now.ToShortTimeString() + "', 'UPDATE', 'Case Management', '" + txtcase.Text + "', '" + username + "')");
                                MessageBox.Show("Case Updated", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                CaseAdded?.Invoke();
                                this.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void validateForm()
        {
            errorCount = 0;
            errorProvider1.Clear();
            if (cmbstatus.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbstatus, "Please Select a Status");
                errorCount++;
            }
            if (string.IsNullOrEmpty(txtyear.Text))
            {
                errorProvider1.SetError(txtyear, "Please Input School Year");
                errorCount++;
            }
            if (string.IsNullOrEmpty(txtdiscplinaryaction.Text))
            {
                errorProvider1.SetError(txtdiscplinaryaction, "Please Input Discplinary Action");
                errorCount++;
            }
            if (cmbconcern.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbconcern, "Please Select Concern Level");
                errorCount++;
            }
            if (cmbstatus.Text == "Resolved")
            {
                if (string.IsNullOrEmpty(txtaction.Text))
                {
                    errorProvider1.SetError(txtaction, "Please Input Action");
                    errorCount++;
                }
            }

        }

        public class ComboBoxItem
        {
            public string Code { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }

            public override string ToString() => Code;
        }
    }
}
