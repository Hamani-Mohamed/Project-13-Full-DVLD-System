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
    public partial class cntrlRenewLicense : UserControl
    {
        cntrlDriverLicenseInfoWithFilters LicenseFilterCard;
        clsLicense License;

        public cntrlRenewLicense(cntrlDriverLicenseInfoWithFilters licenseFilterCard)
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

        void DefaultApplicationData(int LicenseID, clsLicense License)
        {
            clsApplicationType appType = clsApplicationType._GetApplicationInfoByID(2); // Renew

            lblRenewLicenseApplicationID.Text = "Renew License Application ID : ???";
            lblRenewedLicenseID.Text = "Renewed License ID : ???";
            lblApplicationDate.Text = "Application Date : " + DateTime.Now.ToShortDateString();
            lblOldLicenseID.Text = "Old License ID :  " + LicenseID.ToString();
            lblIssueDate.Text = "Issue Date : " + DateTime.Now.ToShortDateString();
            lblExpirationDate.Text = "Expiration Date : " + DateTime.Now.AddYears(License.LicenseClassInfo.DefaultValidityLength).ToShortDateString();
            lblApplicationFees.Text = "Application Fees : " + appType.ApplicationFees;
            lblCreatedBy.Text = "Created By : " + clsGlobalSettings.CurrentUser.Username;
            lblLicenseFees.Text = "License Fees : " + License.LicenseClassInfo.ClassFees;
            lblTotalFees.Text = "Total Fees : " + (appType.ApplicationFees + License.LicenseClassInfo.ClassFees).ToString();
            txtNotes.Text = "";
        }

        int DoesPersonHaveActiveLicense()
        {
            int ActiveLicenseID = clsLicense._GetActiveLicenseIDByDriverID(License.DriverID, License.LicenseClassID);
            return ActiveLicenseID;
        }

        void ShowActiveLicense(int ActiveLicenseID)
        {
            clsLicense ActiveLicense = clsLicense._FindByLicenseID(ActiveLicenseID);
            DefaultApplicationData(ActiveLicenseID, ActiveLicense);
            lblRenewLicenseApplicationID.Text = "Renew License Application ID : " + ActiveLicense.ApplicationID;
            lblRenewedLicenseID.Text = "Renewed License ID : " + ActiveLicense.LicenseID;
            lblOldLicenseID.Text = "Old License ID :  " + License.LicenseID.ToString();
            lblRenew.Enabled = false;
            lblShowLicenseInfo.Enabled = true;
            lblShowLicensesHistory.Enabled = true;
        }

        private void _SetInterfaceState(bool isLicenseValid)
        {
            lblRenew.Enabled = isLicenseValid;
            txtNotes.Enabled = isLicenseValid;

            lblShowLicenseInfo.Enabled = !isLicenseValid;
            lblShowLicensesHistory.Enabled = true;
        }

        bool IsLicenseValid()
        {
            int ActiveLicenseID = DoesPersonHaveActiveLicense();
            clsLicense ActiveLicense = clsLicense._FindByLicenseID(ActiveLicenseID);

            if (ActiveLicenseID != -1 && ActiveLicenseID != License.LicenseID)
            {
                ShowActiveLicense(ActiveLicenseID);
                DefaultApplicationData(ActiveLicenseID, ActiveLicense);
                _SetInterfaceState(false);
                MessageBox.Show("This person already has a renewed active license with ID [" + ActiveLicenseID.ToString() + "]!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (clsDetainedLicense._IsLicenseDetained(License.LicenseID))
            {
                DefaultApplicationData(License.LicenseID, License);
                _SetInterfaceState(false);
                MessageBox.Show("License with ID [" + License.LicenseID + "] is Detained!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!License.IsExpired())
            {
                DefaultApplicationData(License.LicenseID, License);
                _SetInterfaceState(false);
                MessageBox.Show("License with ID [" + License.LicenseID + "] hasn't expired yet!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public void LoadApplicationData(int LicenseID)
        {
            License = clsLicense._FindByLicenseID(LicenseID);

            if (License == null)
            {
                MessageBox.Show("License was not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (!IsLicenseValid())
                    return;

                DefaultApplicationData(LicenseID, License);
                _SetInterfaceState(true);
            }
        }

        private void lblShowLicenseInfo_Click(object sender, EventArgs e)
        {
            if (License == null)
            {
                MessageBox.Show("License was not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsLicense ActiveLicense = clsLicense._FindByLicenseID(DoesPersonHaveActiveLicense());

            if (ActiveLicense != null)
            {
                Form frm1 = new frmShowLocalLicenseInfo(ActiveLicense.LicenseID, ActiveLicense.DriverInfo.PersonID);
                frm1.ShowDialog();
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

        void RenewLicense(clsApplication app)
        {
            clsLicense RenewedLicense = new clsLicense();
            RenewedLicense.ApplicationID = app.ApplicationID;
            RenewedLicense.DriverID = License.DriverID;
            RenewedLicense.LicenseClassID = License.LicenseClassInfo.LicenseClassID;
            RenewedLicense.IssueDate = DateTime.Now;
            RenewedLicense.ExpirationDate = DateTime.Now.AddYears(License.LicenseClassInfo.DefaultValidityLength);
            RenewedLicense.Notes = (string.IsNullOrWhiteSpace(txtNotes.Text) || txtNotes.Text == "No Notes.") ? null : txtNotes.Text;
            RenewedLicense.PaidFees = License.PaidFees;
            RenewedLicense.IsActive = true;
            RenewedLicense.IssueReason = 2;
            RenewedLicense.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            if (RenewedLicense.Save())
            {
                lblRenewLicenseApplicationID.Text = "Renew License Application ID : " + app.ApplicationID;
                lblRenewedLicenseID.Text = "Renewed License ID : " + RenewedLicense.LicenseID;
                lblRenew.Enabled = false;
                lblShowLicenseInfo.Enabled = true;
                LicenseFilterCard.Enabled = false;
                lblShowLicensesHistory.Enabled = true;

                License._DeactivateLicense();

                MessageBox.Show("New License with ID [" + RenewedLicense.LicenseID + "] has been saved successfully!",
                    "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            else
            {
                clsApplication._DeleteApplication(app.ApplicationID);
                MessageBox.Show("Failed to renew License!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblRenew_Click(object sender, EventArgs e)
        {
            if (License == null)
            {
                MessageBox.Show("No License selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsLicenseValid())
                return;

            clsApplicationType appType = clsApplicationType._GetApplicationInfoByID(2);

            if (appType == null)
            {
                MessageBox.Show("Application Type not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsApplication app = new clsApplication();
            app.ApplicantPersonID = License.DriverInfo.PersonID;
            app.ApplicationDate = DateTime.Now;
            app.ApplicationTypeID = 2; // renew application
            app.ApplicationStatus = 3; // completed
            app.PaidFees = appType.ApplicationFees;
            app.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            if (app.Save())
            {
                RenewLicense(app);
            }

            else
                MessageBox.Show("Failed to create the system application!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}