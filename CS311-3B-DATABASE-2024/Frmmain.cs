using Microsoft.SqlServer.Server;
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
    public partial class Frmmain : Form
    {
        private string username, usertype;
        public Frmmain(string username, string usertype)
        {
            InitializeComponent();
            this.username = username;
            this.usertype = usertype;

        }

        private void accountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccounts accounts = new frmAccounts(username);
            accounts.MdiParent = this;
            accounts.Show();

        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frmstudentsaccounts studentsaccounts = new Frmstudentsaccounts(username);
            studentsaccounts.MdiParent = this;
            studentsaccounts.Show();
        }


        private string coursecode;
        private void courseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frmcourse course = new Frmcourse(username, coursecode);
            course.MdiParent = this;
            course.Show();

        }
        private string strandcode;
        private void strandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frmstrand strand = new Frmstrand(username, strandcode);
            strand.MdiParent = this;
            strand.Show();

        }
        private string code;
        private void violationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmViolation violation = new FrmViolation(username, code);
            violation.MdiParent = this;
            violation.Show();
        }
        private string caseid;
        private void casesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCases cases = new FrmCases(username, caseid);
            cases.MdiParent = this;
            cases.Show();
        }

        private void nameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Frmmain_Load_1(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Username: " + username;
            toolStripStatusLabel2.Text = "User Type: " + usertype;
            nameToolStripMenuItem.Text = username;
            if (usertype == "ADMINISTRATOR")
            {
                accountsToolStripMenuItem.Visible = true;
                studentsToolStripMenuItem.Visible = true;
                courseandstrandToolStripMenuItem.Visible = true;
            }
            else if (usertype == "BRANCHADMIN")
            {
                accountsToolStripMenuItem.Visible = false;
                studentsToolStripMenuItem.Visible = true;
                courseandstrandToolStripMenuItem.Visible = true;
            }
            else
            {
                accountsToolStripMenuItem.Visible = false;
                studentsToolStripMenuItem.Visible = false;
                courseandstrandToolStripMenuItem.Visible = true;
            }
        }
    }
}