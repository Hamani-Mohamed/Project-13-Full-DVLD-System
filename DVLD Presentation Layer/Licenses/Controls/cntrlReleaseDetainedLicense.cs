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
    public partial class cntrlReleaseDetainedLicense : UserControl
    {
        cntrlDriverLicenseInfoWithFilters LicenseFilterCard;
        clsLicense License;

        public cntrlReleaseDetainedLicense(cntrlDriverLicenseInfoWithFilters licenseFilterCard)
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

        void FillApplicationData(clsApplicationType appType, clsDetainedLicense detainedLicense)
        {
            lblDetentionID.Text = "Detention ID : " + detainedLicense.DetainID;
            lblDetentionDate.Text = "Detention Date : " + detainedLicense.DetainDate.ToShortDateString();
            lblApplicationFees.Text = "Fees : " + appType.ApplicationFees;
            lblTotalFees.Text = "Total Fees : " + (Convert.ToDecimal(appType.ApplicationFees) + detainedLicense.FineFees).ToString();
            lblLicenseID.Text = "License ID : " + detainedLicense.LicenseID;
            lblReleaseAppID.Text = "Release Application ID : ???";
            lblFineFees.Text = "Fine Fees : " + detainedLicense.FineFees;
            lblCreatedBy.Text = "Created By : " + clsGlobalSettings.CurrentUser.Username;
        }

        bool IsLicenseValid()
        {
            if (!clsDetainedLicense._IsLicenseDetained(License.LicenseID))
            {
                MessageBox.Show("License with ID [" + License.LicenseID + "] is not Detained!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblRelease.Enabled = false;
                return false;
            }

            if (License.IsExpired())
            {
                MessageBox.Show("License with ID [" + License.LicenseID + "] has Expired!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblRelease.Enabled = false;
                return false;
            }

            return true;
        }

        public void LoadApplicationData(int LicenseID)
        {
            License = clsLicense._FindByLicenseID(LicenseID);
            clsApplicationType appType = clsApplicationType._GetApplicationInfoByID(5); // release

            if (License == null)
            {
                MessageBox.Show("License not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblRelease.Enabled = false;
                return;
            }

            lblShowLicenseInfo.Enabled = true;
            lblShowLicensesHistory.Enabled = true;

            if (!IsLicenseValid())
                return;

            clsDetainedLicense DetainedLicense = clsDetainedLicense._FindByLicenseID(License.LicenseID);

            if (DetainedLicense == null)
            {
                MessageBox.Show("Detained License not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FillApplicationData(appType, DetainedLicense);
            lblRelease.Enabled = true;
        }

        private void lblShowLicenseInfo_Click(object sender, EventArgs e)
        {
            if (License == null)
            {
                MessageBox.Show("License not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        void ReleaseLicense(clsApplication app)
        {
            clsDetainedLicense DetainedLicense = clsDetainedLicense._FindByLicenseID(License.LicenseID);

            if (DetainedLicense == null)
            {
                MessageBox.Show("Detained License not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DetainedLicense.IsReleased = true;
            DetainedLicense.ReleaseDate = DateTime.Now;
            DetainedLicense.ReleasedByUserID = clsGlobalSettings.CurrentUser.UserID;
            DetainedLicense.ReleaseApplicationID = app.ApplicationID;

            if (DetainedLicense.Save())
            {
                lblDetentionID.Text = "Release Application ID : " + app.ApplicationID;
                lblRelease.Enabled = false;
                lblReleaseAppID.Text = "Release Application ID : " + DetainedLicense.ReleaseApplicationID;

                MessageBox.Show("License with ID [" + DetainedLicense.LicenseID + "] has been released successfully!",
                    "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            else
            {
                clsApplication._DeleteApplication(app.ApplicationID);
                MessageBox.Show("Failed to release License!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblIssue_Click(object sender, EventArgs e)
        {
            if (License == null)
            {
                MessageBox.Show("No license selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsApplicationType appType = clsApplicationType._GetApplicationInfoByID(5);

            if (appType == null)
            {
                MessageBox.Show("Application Type not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsApplication app = new clsApplication();
            app.ApplicantPersonID = License.DriverInfo.PersonID;
            app.ApplicationDate = DateTime.Now;
            app.ApplicationTypeID = 5; // release
            app.ApplicationStatus = 3; // completed
            app.PaidFees = appType.ApplicationFees;
            app.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            if (app.Save())
            {
                ReleaseLicense(app);
            }

            else
                MessageBox.Show("Failed to create the system application!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}