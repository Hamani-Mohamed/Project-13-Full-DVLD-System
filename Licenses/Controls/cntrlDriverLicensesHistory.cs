using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class cntrlDriverLicensesHistory : UserControl
    {
        int _DriverID;
        enum enLicenses { enLocal, enInternational };
        enLicenses enLicenseType;

        public cntrlDriverLicensesHistory(int driverID, int licenseType = -1)
        {
            InitializeComponent();
            _DriverID = driverID;

            if (licenseType == -1)
                enLicenseType = enLicenses.enLocal;
            else
                enLicenseType = enLicenses.enInternational;
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

        public void _RefreshLicensesList()
        {
            if (enLicenseType == enLicenses.enLocal)
                dgvDriverLicenses.DataSource = clsLicense._GetLocalLicensesHistory(_DriverID);
            else
            {
                dgvDriverLicenses.DataSource = clsInternationalLicense._GetInternationalLicensesHistory(_DriverID);
            }

            lblRecords.Text = "Records : " + dgvDriverLicenses.RowCount.ToString();
            lblNoLicensesYet.Visible = (dgvDriverLicenses.Rows.Count == 0);
        }

        private void CustomizeUI()
        {
            if (dgvDriverLicenses.Rows.Count > 0)
            {
                // Context Menu Strip
                cmsLicenses.ForeColor = Color.White;
                cmsLicenses.BackColor = Color.FromArgb(28, 15, 69);
                cmsLicenses.Renderer = new PurpleMenuRenderer();

                // Data Grid View Style
                dgvDriverLicenses.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11);
                dgvDriverLicenses.BackgroundColor = Color.Black;
                dgvDriverLicenses.BorderStyle = BorderStyle.None;
                dgvDriverLicenses.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvDriverLicenses.GridColor = Color.FromArgb(45, 45, 48);
                dgvDriverLicenses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvDriverLicenses.RowHeadersVisible = false;
                dgvDriverLicenses.EnableHeadersVisualStyles = false;

                dgvDriverLicenses.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dgvDriverLicenses.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 15, 69);
                dgvDriverLicenses.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvDriverLicenses.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 12, FontStyle.Italic);
                dgvDriverLicenses.ColumnHeadersHeight = 45;

                dgvDriverLicenses.DefaultCellStyle.BackColor = Color.FromArgb(20, 20, 20);
                dgvDriverLicenses.DefaultCellStyle.ForeColor = Color.FromArgb(224, 224, 224);
                dgvDriverLicenses.DefaultCellStyle.SelectionBackColor = Color.FromArgb(38, 2, 184);
                dgvDriverLicenses.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvDriverLicenses.RowTemplate.Height = 35;

                dgvDriverLicenses.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(15, 15, 15);

                //Data Grid View Rows

                if (enLicenseType == enLicenses.enLocal)
                {
                    dgvDriverLicenses.Columns["LicenseID"].HeaderText = "License ID";
                    dgvDriverLicenses.Columns["ApplicationID"].HeaderText = "Application ID";
                    dgvDriverLicenses.Columns["ClassName"].HeaderText = "Class Name";
                    dgvDriverLicenses.Columns["IssueDate"].HeaderText = "Issue Date";
                    dgvDriverLicenses.Columns["ExpirationDate"].HeaderText = "Expiration Date";
                    dgvDriverLicenses.Columns["IsActive"].HeaderText = "Is Active";
                }

                else
                {
                    dgvDriverLicenses.Columns["InternationalLicenseID"].HeaderText = "Int. License ID";
                    dgvDriverLicenses.Columns["ApplicationID"].HeaderText = "Application ID";
                    dgvDriverLicenses.Columns["IssuedUsingLocalLicenseID"].HeaderText = "Local License ID";
                    dgvDriverLicenses.Columns["IssueDate"].HeaderText = "Issue Date";
                    dgvDriverLicenses.Columns["ExpirationDate"].HeaderText = "Expiration Date";
                    dgvDriverLicenses.Columns["IsActive"].HeaderText = "Is Active";
                }
            }
        }

        private void cntrlDriverLicensesHistory_Load(object sender, EventArgs e)
        {
            if (enLicenseType == enLicenses.enLocal)
                rbLocalLicenses.Checked = true;
            else
                rbInternationalLicenses.Checked = true;

            _RefreshLicensesList();
            CustomizeUI();
        }

        private void rbLocalLicenses_CheckedChanged(object sender, EventArgs e)
        {
            enLicenseType = enLicenses.enLocal;
            _RefreshLicensesList();
            CustomizeUI();
        }

        private void rbInternationalLicenses_CheckedChanged(object sender, EventArgs e)
        {
            enLicenseType = enLicenses.enInternational;
            _RefreshLicensesList();
            CustomizeUI();
        }

        private void cmsLicenses_Opening(object sender, CancelEventArgs e)
        {
            if (dgvDriverLicenses.Rows.Count <= 0)
            {
                cmsLicenses.Enabled = false;
            }

            else
                cmsLicenses.Enabled = true;
        }

        private void cmsShowLicense_Click(object sender, EventArgs e)
        {
            if (dgvDriverLicenses.Rows.Count > 0)
            {
                if (rbLocalLicenses.Checked)
                {
                    clsLicense license = clsLicense._FindByLicenseID((int)dgvDriverLicenses.CurrentRow.Cells[0].Value);
                    Form frm = new frmShowLocalLicenseInfo(license.LicenseID, license.ApplicationInfo.ApplicantPersonID);
                    frm.ShowDialog();
                }

                else
                {
                    clsInternationalLicense InternationalLicense = clsInternationalLicense._FindByID((int)dgvDriverLicenses.CurrentRow.Cells[0].Value);
                    Form frm = new frmShowInternationalLicenseInfo(InternationalLicense.InternationalLicenseID);
                    frm.ShowDialog();
                }
            }

            else
            {
                MessageBox.Show("No License selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}