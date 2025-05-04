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
    public partial class FrmCases : Form
    {
        private string username, caseid;
        private Class1 fetchStudent = new Class1("127.0.0.1", "cs3113b2024", "Steve", "123456");
        private int errorCount;

        public FrmCases(string username, string caseid)
        {
            InitializeComponent();
            this.username = username;
            this.caseid = caseid;
            txtstudentid.KeyDown += new KeyEventHandler(txtstudentid_KeyDown);
        }
        private void txtstudentid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                validateForm();

                if (errorCount == 0)
                {
                    FetchStudentInfo();
                    FetchStudentCases(txtstudentid.Text.Trim());
                }
            }
        }
        private void validateForm()
        {
            errorCount = 0;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(txtstudentid.Text.Trim()))
            {
                errorProvider1.SetError(txtstudentid, "Please enter a valid Student ID.");
                errorCount++;
            }
        }
        private void FetchStudentInfo()
        {
            string studentID = txtstudentid.Text.Trim();

            if (!string.IsNullOrEmpty(studentID))
            {
                string query = $"SELECT lastname, firstname, middlename, level, course FROM tblstudents WHERE studentID = '{studentID}'";
                DataTable dataTable = fetchStudent.GetData(query);

                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    txtlastname.Text = row["lastname"].ToString();
                    txtfirstname.Text = row["firstname"].ToString();
                    txtmiddlename.Text = row["middlename"].ToString();
                    txtlevel.Text = row["level"].ToString();
                    txtstrandcourse.Text = row["course"].ToString();
                    errorProvider1.Clear();
                }
                else
                {
                    ClearStudentFields();
                    errorProvider1.SetError(txtstudentid, "Student ID not found.");
                }
            }
        }
        private void ClearStudentFields()
        {
            txtlastname.Clear();
            txtfirstname.Clear();
            txtmiddlename.Clear();
            txtlevel.Clear();
            txtstrandcourse.Clear();
            txtstudentid.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
        }
        private void FetchStudentCases(string studentID)
        {
            if (string.IsNullOrEmpty(studentID))
            {
                errorProvider1.SetError(txtstudentid, "Please enter a valid Student ID.");
                return;
            }
            try
            {
                string query = $"SELECT caseid, code, description, type, count, schoolyear, concernlevel, discplinaryaction, status, action, datecreated " +
                               $"FROM tblcases WHERE studentid = '{studentID}' ORDER BY caseid DESC";
                DataTable dt = fetchStudent.GetData(query);

                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridView1.Refresh();
                }
                else
                {
                    DataTable noCasesTable = new DataTable();
                    noCasesTable.Columns.Add("Message");
                    noCasesTable.Rows.Add("No cases found for this student.");
                    dataGridView1.DataSource = noCasesTable;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on Data Refresh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            validateForm();

            if (errorCount == 0)
            {
                FetchStudentInfo();
                if (!string.IsNullOrEmpty(txtstudentid.Text.Trim()))
                {
                    FetchStudentCases(txtstudentid.Text.Trim());
                }
            }
        }
        private void btnadd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtstudentid.Text.Trim()))
            {
                errorProvider1.SetError(txtstudentid, "Please enter a valid Student ID.");
                return;
            }
            FetchStudentInfo();

            if (string.IsNullOrEmpty(txtstudentid.Text))
            {
                MessageBox.Show("Please provide a valid student ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string studentID = txtstudentid.Text;
            string lastname = txtlastname.Text;
            string firstname = txtfirstname.Text;
            string middlename = txtmiddlename.Text;
            string level = txtlevel.Text;
            string course = txtstrandcourse.Text;

            FrmAddCase newaddcase = new FrmAddCase(username, caseid, studentID, lastname, firstname, middlename, level, course);
            newaddcase.CaseAdded += () => FetchStudentCases(studentID);
            this.StartPosition = FormStartPosition.CenterScreen;
            newaddcase.Show();
        }
        private void btnrefresh_Click(object sender, EventArgs e)
        {
            string studentID = txtstudentid.Text.Trim();
            if (!string.IsNullOrEmpty(studentID))
            {
                FetchStudentCases(studentID);
            }
            else
            {
                errorProvider1.SetError(txtstudentid, "Please enter a valid Student ID.");
            }
        }
        private int row;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                row = dataGridView1.SelectedRows[0].Index;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ClearStudentFields();
            errorProvider1.Clear();
        }

        private void btnrefresh_Click_1(object sender, EventArgs e)
        {
            string studentID = txtstudentid.Text.Trim();
            if (!string.IsNullOrEmpty(studentID))
            {
                FetchStudentCases(studentID);
            }
            else
            {
                errorProvider1.SetError(txtstudentid, "Please enter a valid Student ID.");
            }
        }
        private void btnsearch_Click_1(object sender, EventArgs e)
        {
            validateForm();

            if (errorCount == 0)
            {
                FetchStudentInfo();
                if (!string.IsNullOrEmpty(txtstudentid.Text.Trim()))
                {
                    FetchStudentCases(txtstudentid.Text.Trim());
                }
            }
        }
        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0 || (dataGridView1.Rows.Count == 1 && dataGridView1.Rows[0].Cells[0].Value?.ToString() == "No cases found for this student."))
            {
                MessageBox.Show("No Student Case found to Update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string studentid = txtstudentid.Text;
            string lastname = txtlastname.Text;
            string firstname = txtfirstname.Text;
            string middlename = txtmiddlename.Text;
            string level = txtlevel.Text;
            string course = txtstrandcourse.Text;

            if (dataGridView1.SelectedRows.Count > 0)
            {

                int rowIndex = dataGridView1.SelectedRows[0].Index;
                string editcaseid = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                string editviolationid = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();
                string editdescription = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
                string edittype = dataGridView1.Rows[rowIndex].Cells[3].Value.ToString();
                string editviolationcount = dataGridView1.Rows[rowIndex].Cells[4].Value.ToString();
                string edityear = dataGridView1.Rows[rowIndex].Cells[5].Value.ToString();
                string editconcernlevel = dataGridView1.Rows[rowIndex].Cells[6].Value.ToString();
                string editdiscplinaryaction = dataGridView1.Rows[rowIndex].Cells[7].Value.ToString();
                string editstatus = dataGridView1.Rows[rowIndex].Cells[8].Value.ToString();
                string editaction = dataGridView1.Rows[rowIndex].Cells[9].Value.ToString();
                FrmUpdateCase updateStudentAccountfrm = new FrmUpdateCase(username, caseid, editcaseid, studentid, lastname, firstname, middlename, level, course,
                    editviolationid, editviolationcount, editdescription, edityear, editconcernlevel, editdiscplinaryaction, editstatus, editaction);
                updateStudentAccountfrm.CaseAdded += () => FetchStudentCases(studentid);
                updateStudentAccountfrm.Show();
                this.StartPosition = FormStartPosition.CenterParent;
            }
            else
            {
                MessageBox.Show("Please select a student account to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void FrmCases_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtstudentid.Text.Trim()))
            {
                FetchStudentCases(txtstudentid.Text.Trim());
            }
        }
    }
}