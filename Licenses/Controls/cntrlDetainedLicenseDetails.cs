using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class cntrlDetainedLicenseDetails : UserControl
    {
        cntrlDriverLicenseInfoWithFilters LicenseFilterCard;
        clsLicense License;

        public cntrlDetainedLicenseDetails(cntrlDriverLicenseInfoWithFilters licenseFilterCard)
        {
            InitializeComponent();
            LicenseFilterCard = licenseFilterCard;

            if (LicenseFilterCard != null)
            {
                LicenseFilterCard.LicenseSelected += (SelectedLicenseID) =>
                {
                    LoadApplicationData(SelectedLicenseID);
                };
            }
        }

        void ResetDefaultApplicationData(int LicenseID)
        {
            lblDetentionID.Text = "Detention ID : ???";
            lblDetentionDate.Text = "Detention Date : " + DateTime.Now.ToShortDateString();
            lblFineFees.Text = "Fine Fees : ";
            lblLicenseID.Text = "License ID : " + LicenseID.ToString();
            lblCreatedBy.Text = "Created By : " + clsGlobalSettings.CurrentUser.Username;
            txtFineFees.Text = "";
            _SetInterfaceState(true);
        }

        void FillApplicationData(int LicenseID, clsDetainedLicense DetainedLicense)
        {
            lblDetentionID.Text = "Detention ID : " + DetainedLicense.DetainID;
            lblDetentionDate.Text = "Detention Date : " + DetainedLicense.DetainDate.ToShortDateString();
            lblLicenseID.Text = "License ID : " + LicenseID;
            lblFineFees.Text = "Fine Fees : " + DetainedLicense.FineFees;
            lblCreatedBy.Text = "Created By : " + DetainedLicense.CreatedByUserInfo.Username;
            lblDetain.Enabled = false;
            txtFineFees.Visible = false;
            MessageBox.Show("License with ID [" + License.LicenseID + "] is already detained!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void _SetInterfaceState(bool isLicenseValid)
        {
            lblDetain.Enabled = isLicenseValid;
            txtFineFees.Visible = isLicenseValid;
        }

        bool IsLicenseValid()
        {
            if (License.IsExpired())
            {
                MessageBox.Show("License with ID [" + License.LicenseID + "] has Expired!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _SetInterfaceState(false);
                return false;
            }

            return true;
        }

        public void LoadApplicationData(int LicenseID)
        {
            License = clsLicense._FindByLicenseID(LicenseID);

            if (License == null)
            {
                MessageBox.Show("License not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblShowLicensesHistory.Enabled = true;
            lblShowLicenseInfo.Enabled = true;

            clsDetainedLicense detainedLicense = clsDetainedLicense._FindByLicenseID(License.LicenseID);

            if (!IsLicenseValid())
                return;

            if (detainedLicense == null)
            {
                ResetDefaultApplicationData(LicenseID);
                return;
            }

            else
            {
                FillApplicationData(LicenseID, detainedLicense);
            }
        }

        private void lblShowLicenseInfo_Click(object sender, EventArgs e)
        {
            if (License == null)
            {
                MessageBox.Show("License was not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form frm = new frmShowLocalLicenseInfo(License.LicenseID, License.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void lblShowLicensesHistory_Click(object sender, EventArgs e)
        {
            if (License == null)
            {
                MessageBox.Show("License not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form frm = new frmShowLicenseHistory(License.DriverInfo.PersonInfo.PersonID, License.DriverID, -1);
            frm.ShowDialog();
        }

        void DetainLicense()
        {
            clsDetainedLicense DetainedLicense = new clsDetainedLicense();
            DetainedLicense.LicenseID = License.LicenseID;
            DetainedLicense.DetainDate = DateTime.Now;
            DetainedLicense.FineFees = Convert.ToDecimal(txtFineFees.Text);
            DetainedLicense.IsReleased = false;
            DetainedLicense.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            if (DetainedLicense.Save())
            {
                lblDetentionID.Text = "Detention ID : " + DetainedLicense.DetainID;
                lblFineFees.Text = "Fine Fees : " + DetainedLicense.FineFees;
                _SetInterfaceState(false);
                LicenseFilterCard.Enabled = false;

                MessageBox.Show("License with ID [" + License.LicenseID + "] has been detained successfully!",
                    "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            else
            {
                MessageBox.Show("Failed to detain License!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblDetain_Click(object sender, EventArgs e)
        {
            if (License == null)
            {
                MessageBox.Show("No license selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtFineFees.Text))
            {
                MessageBox.Show("Please enter the Fine Fees!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DetainLicense();
        }
    }
}