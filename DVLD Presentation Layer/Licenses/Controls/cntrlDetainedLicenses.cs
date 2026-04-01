using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace DVLD
{
    public partial class cntrlDetainedLicenses : UserControl
    {
        public cntrlDetainedLicenses()
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

        public void _RefreshDetainedLicensesList()
        {
            dgvDetainedLicenses.DataSource = clsDetainedLicense._GetAllDetainedLicenses();
            lblRecords.Text = "Records : " + dgvDetainedLicenses.RowCount.ToString();
        }

        private void CustomizeUI()
        {
            if (dgvDetainedLicenses.Rows.Count > 0)
            {
                //Context Menu Strip
                cmsDetainedLicenses.ForeColor = Color.White;
                cmsDetainedLicenses.BackColor = Color.FromArgb(28, 15, 69);
                cmsDetainedLicenses.Renderer = new PurpleMenuRenderer();

                // Data Grid View Style
                dgvDetainedLicenses.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11);
                dgvDetainedLicenses.BackgroundColor = Color.Black;
                dgvDetainedLicenses.BorderStyle = BorderStyle.None;
                dgvDetainedLicenses.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvDetainedLicenses.GridColor = Color.FromArgb(45, 45, 48);
                dgvDetainedLicenses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvDetainedLicenses.RowHeadersVisible = false;
                dgvDetainedLicenses.EnableHeadersVisualStyles = false;

                dgvDetainedLicenses.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dgvDetainedLicenses.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 15, 69);
                dgvDetainedLicenses.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvDetainedLicenses.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 12, FontStyle.Italic);
                dgvDetainedLicenses.ColumnHeadersHeight = 45;

                dgvDetainedLicenses.DefaultCellStyle.BackColor = Color.FromArgb(20, 20, 20);
                dgvDetainedLicenses.DefaultCellStyle.ForeColor = Color.FromArgb(224, 224, 224);
                dgvDetainedLicenses.DefaultCellStyle.SelectionBackColor = Color.FromArgb(38, 2, 184);
                dgvDetainedLicenses.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvDetainedLicenses.RowTemplate.Height = 35;

                dgvDetainedLicenses.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(15, 15, 15);

                //Data Grid View Rows
                dgvDetainedLicenses.Columns["DetainID"].Width = 130;
                dgvDetainedLicenses.Columns["LicenseID"].Width = 120;
                dgvDetainedLicenses.Columns["IsReleased"].Width = 120;

                dgvDetainedLicenses.Columns["NationalNo"].Width = 130;


                dgvDetainedLicenses.Columns["DetainID"].HeaderText = "Detention ID";
                dgvDetainedLicenses.Columns["LicenseID"].HeaderText = "License ID";
                dgvDetainedLicenses.Columns["DetainDate"].HeaderText = "Detention Date";
                dgvDetainedLicenses.Columns["IsReleased"].HeaderText = "Is Released";
                dgvDetainedLicenses.Columns["FineFees"].HeaderText = "Fine Fees";
                dgvDetainedLicenses.Columns["ReleaseDate"].HeaderText = "Release Date";
                dgvDetainedLicenses.Columns["NationalNo"].HeaderText = "National No.";
                dgvDetainedLicenses.Columns["FullName"].HeaderText = "Full Name";
                dgvDetainedLicenses.Columns["ReleaseApplicationID"].HeaderText = "Release Application ID";
            }
        }

        private void cntrDetainedLicenses_Load(object sender, EventArgs e)
        {
            cbFilters.SelectedIndex = 0;
            txtFilter.Text = "";
            _RefreshDetainedLicensesList();
            CustomizeUI();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (cbFilters.SelectedIndex == 0)
                return;

            DataTable dtInternationalLicenses = (DataTable)dgvDetainedLicenses.DataSource;

            if (dtInternationalLicenses == null)
                return;

            if (string.IsNullOrEmpty(txtFilter.Text) || string.IsNullOrWhiteSpace(txtFilter.Text))
            {
                dtInternationalLicenses.DefaultView.RowFilter = "";
                _RefreshDetainedLicensesList();
                return;
            }

            string columnName = "";

            switch (cbFilters.SelectedIndex)
            {
                case 1:
                    columnName = "DetainID";
                    break;

                case 2:
                    columnName = "LicenseID";
                    break;

                case 3:
                    columnName = "DetainDate";
                    break;

                case 4:
                    columnName = "IsReleased";
                    break;

                case 5:
                    columnName = "FineFees";
                    break;

                case 6:
                    columnName = "ReleaseDate";
                    break;

                case 7:
                    columnName = "NationalNo";
                    break;

                case 8:
                    columnName = "FullName";
                    break;

                case 9:
                    columnName = "ReleaseApplicationID";
                    break;
            }

            string filterValue = txtFilter.Text.Trim();

            if (columnName.Contains("ID") || columnName.Contains("Date"))
            {
                dtInternationalLicenses.DefaultView.RowFilter = string.Format("Convert([{0}], 'System.String') LIKE '{1}%'", columnName, filterValue);
            }
            else
            {
                dtInternationalLicenses.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", columnName, filterValue);
            }

            lblRecords.Text = "Records : " + dgvDetainedLicenses.RowCount.ToString();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilters.SelectedIndex == 0)
            {
                e.Handled = true;
                return;
            }

            if (cbFilters.Text.Contains("ID"))
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            if (cbFilters.Text.Contains("Date"))
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
            _RefreshDetainedLicensesList();
        }

        private void pbDetainLicense_Click(object sender, EventArgs e)
        {
            Form frm = new frmNewDetainedLicense(-1);
            frm.ShowDialog();
            _RefreshDetainedLicensesList();
        }

        private void pbReleaseLicense_Click(object sender, EventArgs e)
        {
            Form frm = new frmReleaseDetainedLicense(-1);
            frm.ShowDialog();
        }

        private void cmsShowPersonDetails_Click(object sender, EventArgs e)
        {
            clsPerson person = clsPerson._FindPersonByNationalNo((string)dgvDetainedLicenses.CurrentRow.Cells[6].Value);

            if (person == null)
            {
                MessageBox.Show("Person with ID [" + person.PersonID + "] was not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form frm = new frmShowPersonDetails(person.PersonID);
            frm.ShowDialog();
        }

        private void cmsShowLicense_Click(object sender, EventArgs e)
        {
            clsLicense License = clsLicense._FindByLicenseID((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value);

            if (License == null)
            {
                MessageBox.Show("License with ID [" + License.LicenseID + "] was not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form frm = new frmShowLocalLicenseInfo(License.LicenseID, License.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void cmsShowPersonLicenseHistory_Click(object sender, EventArgs e)
        {
            clsLicense license = clsLicense._FindByLicenseID((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value);

            if (license == null)
            {
                MessageBox.Show("License with ID [" + license.LicenseID + "] was not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form frm = new frmShowLicenseHistory(license.DriverInfo.PersonID, license.DriverID, -1);
            frm.ShowDialog();
        }
    }
}