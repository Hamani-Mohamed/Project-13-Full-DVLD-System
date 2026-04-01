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
    public partial class cntrlInternationalApplicationDetails : UserControl
    {
        cntrlDriverLicenseInfoWithFilters LicenseFilterCard;
        clsLicense License;

        public cntrlInternationalApplicationDetails(cntrlDriverLicenseInfoWithFilters licenseFilterCard)
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

        void ResetDefaultApplicationData(int LicenseID, clsApplicationType appType)
        {
            lblInternationalApplicationID.Text = "International Application ID : ???";
            lblInternationalLocalLicenseID.Text = "International Local License ID : ???";
            lblAppDate.Text = "Application Date : " + DateTime.Now.ToShortDateString();
            lblIssueDate.Text = "Issue Date : " + DateTime.Now.ToShortDateString();
            lblExpirationDate.Text = "Expiration Date : " + DateTime.Now.AddYears(1).ToShortDateString();
            lblLocalLicenseID.Text = "Local License ID : " + LicenseID.ToString();
            lblFees.Text = "Fees : " + appType.ApplicationFees;
            lblCreatedBy.Text = "Created By : " + clsGlobalSettings.CurrentUser.Username;
            lblIssue.Enabled = true;
            lblShowLicensesHistory.Enabled = true;
            lblShowLicenseInfo.Enabled = false;
        }

        void FillApplicationData(int LicenseID, clsApplicationType appType, clsInternationalLicense InternationalLicense)
        {
            lblInternationalApplicationID.Text = "International Application ID : " + InternationalLicense.ApplicationID;
            lblInternationalLocalLicenseID.Text = "International Local License ID : " + InternationalLicense.InternationalLicenseID;
            lblAppDate.Text = "Application Date : " + InternationalLicense.ApplicationInfo.ApplicationDate.ToShortDateString();
            lblIssueDate.Text = "Issue Date : " + InternationalLicense.IssueDate.ToShortDateString();
            lblExpirationDate.Text = "Expiration Date : " + InternationalLicense.ExpirationDate.ToShortDateString();
            lblLocalLicenseID.Text = "Local License ID : " + LicenseID.ToString();
            lblFees.Text = "Fees : " + appType.ApplicationFees.ToString();
            lblCreatedBy.Text = "Created By : " + InternationalLicense.UserInfo.Username;

            lblShowLicenseInfo.Enabled = true;
            lblIssue.Enabled = false;
            MessageBox.Show("Person already has an active international license with ID = " + InternationalLicense.InternationalLicenseID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        bool IsLicenseValid()
        {
            if (License.LicenseClassID != 3)
            {
            MessageBox.Show("Cannot make an international license of this class! Only Class 3 is allowed (Ordinary License).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
            }
            
            if (License.IsActive == false)
            {
                MessageBox.Show("License with ID [" + License.LicenseID + "] is Inactive!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (License.IsExpired())
            {
                MessageBox.Show("License with ID [" + License.LicenseID + "] has Expired!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (clsDetainedLicense._IsLicenseDetained(License.LicenseID))
            {
                MessageBox.Show("License with ID [" + License.LicenseID + "] is Detained!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public void LoadApplicationData(int LicenseID)
        {
            License = clsLicense._FindByLicenseID(LicenseID);
            clsApplicationType appType = clsApplicationType._GetApplicationInfoByID(6);

            if (License == null)
            {
                MessageBox.Show("License not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsInternationalLicense InternationalLicense = clsInternationalLicense._FindByDriverID(License.DriverID);

            lblShowLicensesHistory.Enabled = true;

            if (InternationalLicense == null || (InternationalLicense != null && !InternationalLicense.IsActive))
            {
                if (!IsLicenseValid())
                    return;
                ResetDefaultApplicationData(LicenseID, appType);
                return;
            }

            else
            {
                FillApplicationData(LicenseID, appType, InternationalLicense);
            }
        }

        private void lblShowLicenseInfo_Click(object sender, EventArgs e)
        {
            clsInternationalLicense InternationalLicense = clsInternationalLicense._FindByDriverID(License.DriverID);

            if (InternationalLicense == null)
            {
                MessageBox.Show("International License was not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form frm = new frmShowInternationalLicenseInfo(InternationalLicense.InternationalLicenseID);
            frm.ShowDialog();
        }

        private void lblShowLicensesHistory_Click(object sender, EventArgs e)
        {
            if (License == null)
            {
                MessageBox.Show("License not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form frm = new frmShowLicenseHistory(License.DriverInfo.PersonInfo.PersonID, License.DriverID, 1);
            frm.ShowDialog();
        }

        void IssueInternationalLicense(clsApplication app)
        {
            clsInternationalLicense internationalLicense = new clsInternationalLicense();
            internationalLicense.ApplicationID = app.ApplicationID;
            internationalLicense.DriverID = License.DriverID;
            internationalLicense.IssuedUsingLocalLicenseID = License.LicenseID;
            internationalLicense.IssueDate = DateTime.Now;
            internationalLicense.ExpirationDate = DateTime.Now.AddYears(1);
            internationalLicense.IsActive = true;
            internationalLicense.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            if (internationalLicense.Save())
            {
                lblInternationalApplicationID.Text = "International Application ID : " + app.ApplicationID;
                lblInternationalLocalLicenseID.Text = "International Local License ID : " + internationalLicense.InternationalLicenseID;
                lblIssue.Enabled = false;
                lblShowLicenseInfo.Enabled = true;
                LicenseFilterCard.Enabled = false;

                MessageBox.Show("International License with ID [" + internationalLicense.InternationalLicenseID + "] has been saved successfully!",
                    "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            else
            {
                clsApplication._DeleteApplication(app.ApplicationID);
                MessageBox.Show("Failed to create the International License!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblIssue_Click(object sender, EventArgs e)
        {
            if (License == null)
            {
                MessageBox.Show("No license selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsInternationalLicense InternationalLicense = clsInternationalLicense._FindByDriverID(License.DriverID);
            clsApplicationType appType = clsApplicationType._GetApplicationInfoByName("New International License");

            if (appType == null)
            {
                MessageBox.Show("Application Type not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (InternationalLicense != null)
            {
                MessageBox.Show("This person already has an International License!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsApplication app = new clsApplication();
            app.ApplicantPersonID = License.DriverInfo.PersonID;
            app.ApplicationDate = DateTime.Now;
            app.ApplicationTypeID = 6; // international application
            app.ApplicationStatus = 3; // completed
            app.PaidFees = appType.ApplicationFees;
            app.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            if (app.Save())
            {
                IssueInternationalLicense(app);
            }

            else
                MessageBox.Show("Failed to create the system application!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
