using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DVLD
{
    public partial class cntrlLocalLicenses : UserControl
    {
        public cntrlLocalLicenses()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public class PurpleMenuRenderer : ToolStripProfessionalRenderer
        {
            public PurpleMenuRenderer() : base(new PurpleColorTable()) { }
        }

        public class PurpleColorTable : ProfessionalColorTable
        {
            // The "Hover" color - a lighter, elegant purple
            public override Color MenuItemSelected => Color.FromArgb(50, 30, 100);

            // The border around the hovered item
            public override Color MenuItemBorder => Color.FromArgb(120, 90, 180);

            // Selection colors for the "Press" state
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(65, 45, 130);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(65, 45, 130);
        }

        public void _RefreshLocalLicensesList()
        {
            dgvLocalLicenses.DataSource = clsLocalLicense._GetAllLocalLicenses();
            lblRecords.Text = "Records : " + dgvLocalLicenses.RowCount.ToString();
        }

        private void CustomizeUI()
        {
            if (dgvLocalLicenses.Rows.Count > 0)
            {
                //Context Menu Strip
                cmsLocalLicenses.ForeColor = Color.White;
                cmsLocalLicenses.BackColor = Color.FromArgb(28, 15, 69);
                cmsLocalLicenses.Renderer = new PurpleMenuRenderer();

                // Schedule Tests Menu Styles
                cmsScheduleTests.DropDown.BackColor = Color.FromArgb(28, 15, 69);
                cmsScheduleTests.DropDown.ForeColor = Color.White;

                // Data Grid View Style
                dgvLocalLicenses.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11);
                dgvLocalLicenses.BackgroundColor = Color.Black;
                dgvLocalLicenses.BorderStyle = BorderStyle.None;
                dgvLocalLicenses.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvLocalLicenses.GridColor = Color.FromArgb(45, 45, 48);
                dgvLocalLicenses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvLocalLicenses.RowHeadersVisible = false;
                dgvLocalLicenses.EnableHeadersVisualStyles = false;

                dgvLocalLicenses.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dgvLocalLicenses.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 15, 69);
                dgvLocalLicenses.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvLocalLicenses.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 12, FontStyle.Italic);
                dgvLocalLicenses.ColumnHeadersHeight = 45;

                dgvLocalLicenses.DefaultCellStyle.BackColor = Color.FromArgb(20, 20, 20);
                dgvLocalLicenses.DefaultCellStyle.ForeColor = Color.FromArgb(224, 224, 224);
                dgvLocalLicenses.DefaultCellStyle.SelectionBackColor = Color.FromArgb(38, 2, 184);
                dgvLocalLicenses.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvLocalLicenses.RowTemplate.Height = 35;

                dgvLocalLicenses.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(15, 15, 15);

                //Data Grid View Rows
                dgvLocalLicenses.Columns["LocalDrivingLicenseApplicationID"].Width = 70;
                dgvLocalLicenses.Columns["ClassName"].Width = 120;
                dgvLocalLicenses.Columns["NationalNo"].Width = 50;
                dgvLocalLicenses.Columns["FullName"].Width = 160;
                dgvLocalLicenses.Columns["ApplicationDate"].Width = 100;
                dgvLocalLicenses.Columns["PassedTestCount"].Width = 60;
                dgvLocalLicenses.Columns["Status"].Width = 110;

                dgvLocalLicenses.Columns["LocalDrivingLicenseApplicationID"].HeaderText = "Local Application ID";
                dgvLocalLicenses.Columns["ClassName"].HeaderText = "Class Name";
                dgvLocalLicenses.Columns["NationalNo"].HeaderText = "National No";
                dgvLocalLicenses.Columns["FullName"].HeaderText = "Full Name";
                dgvLocalLicenses.Columns["ApplicationDate"].HeaderText = "Application Date";
                dgvLocalLicenses.Columns["PassedTestCount"].HeaderText = "Passed Tests";
            }
        }

        private void cntrlLocalLicenses_Load(object sender, EventArgs e)
        {
            cbFilters.SelectedIndex = 0;
            txtFilter.Text = "";
            _RefreshLocalLicensesList();
            CustomizeUI();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (cbFilters.SelectedIndex == 0)
                return;

            DataTable dtLocalLicenses = (DataTable)dgvLocalLicenses.DataSource;

            if (dtLocalLicenses == null)
                return;

            if (string.IsNullOrEmpty(txtFilter.Text) || string.IsNullOrWhiteSpace(txtFilter.Text))
            {
                dtLocalLicenses.DefaultView.RowFilter = "";
                _RefreshLocalLicensesList();
                return;
            }

            string columnName = "";

            switch (cbFilters.SelectedIndex)
            {
                case 1:
                    columnName = "LocalDrivingLicenseApplicationID";
                    break;

                case 2:
                    columnName = "ClassName";
                    break;

                case 3:
                    columnName = "NationalNo";
                    break;

                case 4:
                    columnName = "FullName";
                    break;

                case 5:
                    columnName = "ApplicationDate";
                    break;

                case 6:
                    columnName = "PassedTestCount";
                    break;

                case 7:
                    columnName = "Status";
                    break;
            }


            string filterValue = txtFilter.Text.Trim();

            if (columnName == "LocalDrivingLicenseApplicationID" || columnName == "ApplicationDate" || columnName == "PassedTestCount")
            {
                dtLocalLicenses.DefaultView.RowFilter = string.Format("Convert([{0}], 'System.String') LIKE '{1}%'", columnName, filterValue);
            }
            else
            {
                dtLocalLicenses.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", columnName, filterValue);
            }

            lblRecords.Text = "Records : " + dgvLocalLicenses.RowCount.ToString();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilters.SelectedIndex == 0)
            {
                e.Handled = true;
                return;
            }

            if (cbFilters.SelectedIndex == 1 || cbFilters.SelectedIndex == 6)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }

            if (cbFilters.SelectedIndex == 5)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '/')
                {
                    e.Handled = true;
                }
            }
        }

        private void cbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Text = "";
            _RefreshLocalLicensesList();
        }

        private void cmsLocalLicenses_Opening(object sender, CancelEventArgs e)
        {
            string Status = dgvLocalLicenses.CurrentRow.Cells["Status"].Value.ToString();
            int PassedTests = Convert.ToInt32(dgvLocalLicenses.CurrentRow.Cells["PassedTestCount"].Value);

            cmsShowApplicationDetails.Enabled = true;

            cmsShowPersonLicenseHistory.Enabled = false;
            cmsDeleteApplication.Enabled = false;
            cmsEditApplication.Enabled = false;
            cmsScheduleTests.Enabled = false;
            cmsIssueDriverLicense.Enabled = false;
            cmsShowLicense.Enabled = false;
            cmsCancelApplication.Enabled = false;
            cmsScheduleVisionTest.Enabled = false;
            cmsScheduleWrittenTest.Enabled = false;
            cmsScheduleStreetTest.Enabled = false;

            if (Status == "Cancelled")
                return;

            else if (Status == "Completed")
            {
                cmsShowLicense.Enabled = true;
                cmsShowPersonLicenseHistory.Enabled = true;
            }

            else if (Status == "New")
            {
                cmsCancelApplication.Enabled = true;

                if (PassedTests < 3)
                    cmsScheduleTests.Enabled = true;

                if (PassedTests == 0)
                {
                    cmsEditApplication.Enabled = true;
                    cmsDeleteApplication.Enabled = true;
                    cmsScheduleVisionTest.Enabled = true;
                }

                else if (PassedTests == 1)
                {
                    cmsScheduleWrittenTest.Enabled = true;
                }

                else if (PassedTests == 2)
                {
                    cmsScheduleStreetTest.Enabled = true;
                }

                else if (PassedTests == 3)
                {
                    cmsIssueDriverLicense.Enabled = true;
                }
            }

            if (clsTestAppointment._FindByLocalLicenseID((int)dgvLocalLicenses.CurrentRow.Cells[0].Value) != null)
            {
                cmsDeleteApplication.Enabled = false;
            }
        }

        private void pbAddApplication_Click(object sender, EventArgs e)
        {
            Form frm = new frmNewLocalDrivingLicenseApplication(-1, -1);
            frm.ShowDialog();
            _RefreshLocalLicensesList();
        }

        private void cmsShowApplicationDetails_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowApplicationDetails((int)dgvLocalLicenses.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void cmsEditApplication_Click(object sender, EventArgs e)
        {
            clsLocalLicense localLicense = clsLocalLicense.FindLocalLicenseByID((int)dgvLocalLicenses.CurrentRow.Cells[0].Value);
            // clsPerson person = clsPerson.Find((string)dgvLocalLicenses.CurrentRow.Cells[2].Value);

            Form frm = new frmNewLocalDrivingLicenseApplication(localLicense.PersonInfo.PersonID, localLicense.LocalDrivingLicenseApplicationID);
            frm.ShowDialog();
            _RefreshLocalLicensesList();
        }

        private void cmsDeleteApplication_Click(object sender, EventArgs e)
        {
            clsLocalLicense localLicense = clsLocalLicense.FindLocalLicenseByID((int)dgvLocalLicenses.CurrentRow.Cells[0].Value);
            clsTestAppointment testAppointment = clsTestAppointment._FindByLocalLicenseID(localLicense.LocalDrivingLicenseApplicationID);
            clsDriver driver = clsDriver._GetDriverInfoByPersonID(localLicense.ApplicantPersonID);

            if (driver != null || testAppointment != null)
            {
                MessageBox.Show("Cannot delete local application with ID [" + localLicense.LocalDrivingLicenseApplicationID + "] as they have data linked to it!",
                    "Deletion Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete application [" + (int)dgvLocalLicenses.CurrentRow.Cells[0].Value + "] permanently?",
                "Confirm Deletion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (clsLocalLicense._DeleteLocalLicense((int)dgvLocalLicenses.CurrentRow.Cells[0].Value))
                {
                    _RefreshLocalLicensesList();
                }
                else
                {
                    MessageBox.Show("Error: Could not delete application. It might be linked to other data.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void cmsCancelApplication_Click(object sender, EventArgs e)
        {
            clsLocalLicense license = clsLocalLicense.FindLocalLicenseByID((int)dgvLocalLicenses.CurrentRow.Cells[0].Value);

            if (MessageBox.Show("Are you sure you want to cancel [" + dgvLocalLicenses.CurrentRow.Cells[0].Value.ToString()
            + "] ?", "Confirm Cancellation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (clsApplication._CancelApplication(license.ApplicationID))
                {
                    _RefreshLocalLicensesList();
                }
            }
        }

        private void cmsScheduleVisionTest_Click(object sender, EventArgs e)
        {
            Form frm = new frmScheduleTest((int)dgvLocalLicenses.CurrentRow.Cells[0].Value, 1);
            frm.ShowDialog();
            _RefreshLocalLicensesList();
        }

        private void cmsScheduleWrittenTest_Click(object sender, EventArgs e)
        {
            Form frm = new frmScheduleTest((int)dgvLocalLicenses.CurrentRow.Cells[0].Value, 2);
            frm.ShowDialog();
            _RefreshLocalLicensesList();
        }

        private void cmsScheduleStreetTest_Click(object sender, EventArgs e)
        {
            Form frm = new frmScheduleTest((int)dgvLocalLicenses.CurrentRow.Cells[0].Value, 3);
            frm.ShowDialog();
            _RefreshLocalLicensesList();
        }

        private void cmsIssueDL_Click(object sender, EventArgs e)
        {
            Form frm = new frmIssueLicenseFirstTime((int)dgvLocalLicenses.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshLocalLicensesList();
        }

        private void cmsShowLicense_Click(object sender, EventArgs e)
        {
            clsLocalLicense localLicense = clsLocalLicense.FindLocalLicenseByID((int)dgvLocalLicenses.CurrentRow.Cells[0].Value);
            clsLicense license = clsLicense._FindByApplicationID(localLicense.ApplicationID);
            Form frm = new frmShowLocalLicenseInfo(license.LicenseID, localLicense.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void cmsShowPersonLicenseHistory_Click(object sender, EventArgs e)
        {
            clsLocalLicense localLicense = clsLocalLicense.FindLocalLicenseByID((int)dgvLocalLicenses.CurrentRow.Cells[0].Value);
            clsLicense license = clsLicense._FindByApplicationID(localLicense.ApplicationID);

            Form frm = new frmShowLicenseHistory(localLicense.PersonInfo.PersonID, license.DriverInfo.DriverID, -1);
            frm.ShowDialog();
        }
    }
}