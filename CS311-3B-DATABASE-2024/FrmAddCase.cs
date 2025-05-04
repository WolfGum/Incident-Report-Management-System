using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS311_3B_DATABASE_2024
{
    public partial class FrmAddCase : Form
    {
        private string username, caseid;
        private Class1 updatestrandform = new Class1("127.0.0.1", "cs3113b2024", "Steve", "123456");
        public FrmAddCase(string username, string caseid, string studentID, string lastname, string firstname, string middlename, string level, string course)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.username = username;
            this.caseid = caseid;
            txtstudentid.Text = studentID;
            txtlastname.Text = lastname;
            txtfirstname.Text = firstname;
            txtmiddlename.Text = middlename;
            txtlevel.Text = level;
            txtstrandcourse.Text = course;
            cmbcode.SelectedIndexChanged += cmbcode_SelectedIndexChanged;
        }
        private string GenerateCaseID()
        {
            DateTime now = DateTime.Now;
            return $"case - {now.Year}{now.Month:D2}{now.Day:D2}{now.Hour:D2}{now.Minute:D2}{now.Second:D2}";
        }
        private void FrmAddCase_Load(object sender, EventArgs e)
        {
            caseid = GenerateCaseID();
            txtcase.Text = caseid;
            PopulateViolationId();
        }
        private void PopulateViolationId()
        {
            cmbcode.Items.Clear();
            string query = "SELECT code, type, description FROM tblviolations";
            DataTable dataTable = updatestrandform.GetData(query);

            foreach (DataRow row in dataTable.Rows)
            {
                cmbcode.Items.Add(new ComboBoxItem
                {
                    Code = row["code"].ToString(),
                    Type = row["type"].ToString(),
                    Description = row["description"].ToString()
                });
            }
        }
        public class ComboBoxItem
        {
            public string Code { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }

            public override string ToString() => Code;
        }
        private void cmbcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtcount.Clear();
            errorProvider1.SetError(txtcount, string.Empty);

            if (cmbcode.SelectedItem is ComboBoxItem selectedItem)
            {
                txtdescription.Text = selectedItem.Description;
                SetOffenseCount(txtstudentid.Text, selectedItem.Code);
            }
        }
        private void SetOffenseCount(string studentId, string violationCode)
        {
            string query = $"SELECT count FROM tblcases WHERE studentid = '{studentId}' AND code = '{violationCode}'";
            DataTable result = updatestrandform.GetData(query);

            bool hasFirstOffense = false;
            bool hasSecondOffense = false;
            foreach (DataRow row in result.Rows)
            {
                string existingCount = row["count"].ToString().ToLower().Trim();
                Console.WriteLine($"Retrieved count from DB: {existingCount}");

                if (existingCount == "1st offense")
                {
                    hasFirstOffense = true;
                }
                else if (existingCount == "2nd offense")
                {
                    hasSecondOffense = true;
                }
            }
            if (hasSecondOffense)
            {
                txtcount.Text = "Repeat Offense";
            }
            else if (hasFirstOffense)
            {
                txtcount.Text = "2nd Offense";
            }
            else
            {
                txtcount.Text = "1st Offense";
            }
            Console.WriteLine($"Set txtcount.Text to: {txtcount.Text}");
        }
        private int errorCount = 0;
        private bool validateForm()
        {
            errorCount = 0;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtcase.Text))
            {
                errorProvider1.SetError(txtcase, "Input is Empty");
                errorCount++;
            }
            if (string.IsNullOrEmpty(txtyear.Text))
            {
                errorProvider1.SetError(txtyear, "Input is Empty");
                errorCount++;
            }
            if (string.IsNullOrEmpty(txtdiscplinaryaction.Text))
            {
                errorProvider1.SetError(txtdiscplinaryaction, "Input is Empty");
                errorCount++;
            }
            if (cmbcode.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbcode, "Select a violation code");
                errorCount++;
            }
            if (cmbconcern.SelectedIndex < 0)
            {
                errorProvider1.SetError(cmbconcern, "Select a violation code");
                errorCount++;
            }

            string violationCode = cmbcode.Text.ToUpper();
            string existingOffenses = CheckExistingOffense(txtstudentid.Text, violationCode).Trim().ToLower();
            string currentCount = txtcount.Text.Trim().ToLower();
            Console.WriteLine($"Existing Offenses: {existingOffenses}, Current Count: {currentCount}");
            if (existingOffenses.Contains("1st offense") && currentCount == "1st offense")
            {
                errorProvider1.SetError(txtcount, "Student already has 1st offense for this violation.");
                errorCount++;
            }
            if (existingOffenses.Contains("2nd offense") && currentCount == "2nd offense")
            {
                errorProvider1.SetError(txtcount, "Student already has 2nd offense for this violation.");
                errorCount++;
            }
            return errorCount == 0;
        }
        private string CheckExistingOffense(string studentId, string violationCode)
        {
            string query = $"SELECT count FROM tblcases WHERE studentid = '{studentId}' AND code = '{violationCode}'";
            DataTable result = updatestrandform.GetData(query);
            List<string> offenses = new List<string>();
            foreach (DataRow row in result.Rows)
            {
                string offense = row["count"].ToString().Trim().ToLower();
                if (!offenses.Contains(offense))
                {
                    offenses.Add(offense);
                }
            }
            return offenses.Count > 0 ? string.Join(", ", offenses) : string.Empty;
        }
        public event Action CaseAdded;

        private void btnclear_Click(object sender, EventArgs e)
        {
           txtdescription.Clear();
           txtcount.Clear();
            cmbcode.SelectedIndex = -1;
            txtyear.Clear();
            cmbconcern.SelectedIndex = -1;
            txtdiscplinaryaction.Clear();

        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if (validateForm())
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to add this Case for this Student?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        ComboBoxItem selectedItem = (ComboBoxItem)cmbcode.SelectedItem;
                        string violationType = selectedItem.Type;

                        updatestrandform.executeSQL("INSERT INTO tblcases (caseid, studentid, code, description, type, count, schoolyear, concernlevel, discplinaryaction, status, action, createdby, datecreated) " +
                            $"VALUES ('{txtcase.Text}', '{txtstudentid.Text}', '{cmbcode.Text.ToUpper()}', '{txtdescription.Text}', " +
                            $"'{violationType}', '{txtcount.Text}', '{txtyear.Text}', '{cmbconcern.Text}', '{txtdiscplinaryaction.Text}', 'On going', '', '{username}', '{DateTime.Now.ToShortDateString()}')");


                        if (updatestrandform.rowAffected > 0)
                        {
                            updatestrandform.executeSQL("INSERT INTO tbllogs (datelog, timelog, action, module, ID, performedby) " +
                                                         $"VALUES ('{DateTime.Now.ToShortDateString()}', '{DateTime.Now.ToShortTimeString()}', 'ADD', 'Cases Management', '{txtcase.Text}', '{username}')");

                            MessageBox.Show("New case added successfully!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CaseAdded?.Invoke();
                            this.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error on saving the case", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}