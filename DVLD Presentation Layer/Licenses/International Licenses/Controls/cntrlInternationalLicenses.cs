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
    public partial class cntrlInternationalLicenses : UserControl
    {
        public cntrlInternationalLicenses()
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

        public void _RefreshInternationalLicensesList()
        {
            dgvInternationalLicenses.DataSource = clsInternationalLicense._GetAllInternationalLicenses();
            lblRecords.Text = "Records : " + dgvInternationalLicenses.RowCount.ToString();
        }

        private void CustomizeUI()
        {
            if (dgvInternationalLicenses.Rows.Count > 0)
            {
                //Context Menu Strip
                cmsInternationalLicenses.ForeColor = Color.White;
                cmsInternationalLicenses.BackColor = Color.FromArgb(28, 15, 69);
                cmsInternationalLicenses.Renderer = new PurpleMenuRenderer();

                // Data Grid View Style
                dgvInternationalLicenses.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11);
                dgvInternationalLicenses.BackgroundColor = Color.Black;
                dgvInternationalLicenses.BorderStyle = BorderStyle.None;
                dgvInternationalLicenses.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvInternationalLicenses.GridColor = Color.FromArgb(45, 45, 48);
                dgvInternationalLicenses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvInternationalLicenses.RowHeadersVisible = false;
                dgvInternationalLicenses.EnableHeadersVisualStyles = false;

                dgvInternationalLicenses.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dgvInternationalLicenses.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 15, 69);
                dgvInternationalLicenses.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvInternationalLicenses.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 12, FontStyle.Italic);
                dgvInternationalLicenses.ColumnHeadersHeight = 45;

                dgvInternationalLicenses.DefaultCellStyle.BackColor = Color.FromArgb(20, 20, 20);
                dgvInternationalLicenses.DefaultCellStyle.ForeColor = Color.FromArgb(224, 224, 224);
                dgvInternationalLicenses.DefaultCellStyle.SelectionBackColor = Color.FromArgb(38, 2, 184);
                dgvInternationalLicenses.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvInternationalLicenses.RowTemplate.Height = 35;

                dgvInternationalLicenses.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(15, 15, 15);

                //Data Grid View Rows
                dgvInternationalLicenses.Columns["InternationalLicenseID"].HeaderText = "International License ID";
                dgvInternationalLicenses.Columns["ApplicationID"].HeaderText = "Application ID";
                dgvInternationalLicenses.Columns["DriverID"].HeaderText = "Driver ID";
                dgvInternationalLicenses.Columns["IssuedUsingLocalLicenseID"].HeaderText = "Local License ID";
                dgvInternationalLicenses.Columns["IssueDate"].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns["ExpirationDate"].HeaderText = "Expiration Date";
                dgvInternationalLicenses.Columns["IsActive"].HeaderText = "Is Active";
            }
        }

        private void cntrlInternationalLicenses_Load(object sender, EventArgs e)
        {
            cbFilters.SelectedIndex = 0;
            txtFilter.Text = "";
            _RefreshInternationalLicensesList();
            CustomizeUI();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (cbFilters.SelectedIndex == 0)
                return;

            DataTable dtInternationalLicenses = (DataTable)dgvInternationalLicenses.DataSource;

            if (dtInternationalLicenses == null)
                return;

            if (string.IsNullOrEmpty(txtFilter.Text) || string.IsNullOrWhiteSpace(txtFilter.Text))
            {
                dtInternationalLicenses.DefaultView.RowFilter = "";
                _RefreshInternationalLicensesList();
                return;
            }

            string columnName = "";

            switch (cbFilters.SelectedIndex)
            {
                case 1:
                    columnName = "InternationalLicenseID";
                    break;

                case 2:
                    columnName = "ApplicationID";
                    break;

                case 3:
                    columnName = "DriverID";
                    break;

                case 4:
                    columnName = "IssuedUsingLocalLicenseID";
                    break;

                case 5:
                    columnName = "IssueDate";
                    break;

                case 6:
                    columnName = "ExpirationDate";
                    break;

                case 7:
                    columnName = "IsActive";
                    break;
            }

            string filterValue = txtFilter.Text.Trim();

            if (columnName.Contains("ID"))
            {
                dtInternationalLicenses.DefaultView.RowFilter = string.Format("Convert([{0}], 'System.String') LIKE '{1}%'", columnName, filterValue);
            }
            else
            {
                dtInternationalLicenses.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", columnName, filterValue);
            }

            lblRecords.Text = "Records : " + dgvInternationalLicenses.RowCount.ToString();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilters.SelectedIndex == 0)
            {
                e.Handled = true;
                return;
            }

            if (cbFilters.SelectedIndex == 1 || cbFilters.SelectedIndex == 2 || cbFilters.SelectedIndex == 3 || cbFilters.SelectedIndex == 4)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void cbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Text = "";
            _RefreshInternationalLicensesList();
        }

        private void pbAddApplication_Click(object sender, EventArgs e)
        {
            Form frm = new frmNewInternationalDrivingLicense(-1);
            frm.ShowDialog();
            _RefreshInternationalLicensesList();
        }

        private void cmsShowPersonDetails_Click(object sender, EventArgs e)
        {
            clsDriver driver = clsDriver._GetDriverInfoByDriverID((int)dgvInternationalLicenses.CurrentRow.Cells[2].Value);

            if (driver == null)
            {
                MessageBox.Show("Person with ID [" + driver.PersonID + "] was not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form frm = new frmShowPersonDetails(driver.PersonID);
            frm.ShowDialog();
        }

        private void cmsShowLicense_Click(object sender, EventArgs e)
        {
            clsInternationalLicense InternationalLicense = clsInternationalLicense._FindByID((int)dgvInternationalLicenses.CurrentRow.Cells[0].Value);

            if (InternationalLicense == null)
            {
                MessageBox.Show("License with ID [" + InternationalLicense.InternationalLicenseID + "] was not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form frm = new frmShowInternationalLicenseInfo(InternationalLicense.InternationalLicenseID);
            frm.ShowDialog();
        }

        private void cmsShowPersonLicenseHistory_Click(object sender, EventArgs e)
        {
            clsDriver driver = clsDriver._GetDriverInfoByDriverID((int)dgvInternationalLicenses.CurrentRow.Cells[2].Value);
            clsLicense license = clsLicense._FindByLicenseID((int)dgvInternationalLicenses.CurrentRow.Cells[3].Value);

            if (driver == null)
            {
                MessageBox.Show("Person with ID [" + driver.PersonID + "] was not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (license == null)
            {
                MessageBox.Show("License with ID [" + license.LicenseID + "] was not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form frm = new frmShowLicenseHistory(driver.PersonInfo.PersonID, driver.DriverID, 1);
            frm.ShowDialog();
        }
    }
}