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
    public partial class cntrlReplaceLicense : UserControl
    {
        cntrlDriverLicenseInfoWithFilters LicenseFilterCard;
        clsLicense License;

        public cntrlReplaceLicense(cntrlDriverLicenseInfoWithFilters licenseFilterCard)
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

        void DefaultApplicationData(int LicenseID)
        {
            clsApplicationType appType;

            appType = clsApplicationType._GetApplicationInfoByID(rbReplaceLostLicense.Checked ? 3 : 4); // = Lost, 4 = Damaged

            lblLicenseReplacementApplicationID.Text = "License Replacement Application ID : ???";
            lblReplacedLicenseID.Text = "Replaced License ID : ???";
            lblApplicationDate.Text = "Application Date : " + DateTime.Now.ToShortDateString();
            lblOldLicenseID.Text = "Old License ID :  " + LicenseID.ToString();

            if (rbReplaceDamagedLicense.Checked || rbReplaceLostLicense.Checked)
                lblApplicationFees.Text = "Application Fees : " + appType.ApplicationFees;

            lblCreatedBy.Text = "Created By : " + clsGlobalSettings.CurrentUser.Username;
        }

        private void _SetInterfaceState(bool isLicenseValid)
        {
            lblReplace.Enabled = isLicenseValid;
            rbReplaceDamagedLicense.Enabled = isLicenseValid;
            rbReplaceLostLicense.Enabled = isLicenseValid;

            lblShowLicenseInfo.Enabled = !isLicenseValid;
            lblShowLicensesHistory.Enabled = true;
        }

        bool IsLicenseValid()
        {
            if (clsDetainedLicense._IsLicenseDetained(License.LicenseID))
            {
                _SetInterfaceState(false);
                DefaultApplicationData(License.LicenseID);
                MessageBox.Show("License with ID [" + License.LicenseID + "] is Detained!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (License.IsExpired())
            {
                _SetInterfaceState(false);
                DefaultApplicationData(License.LicenseID);
                MessageBox.Show("License with ID [" + License.LicenseID + "] is expired!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                DefaultApplicationData(LicenseID);
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

        private void rbLostOrDamaged_CheckedChanged(object sender, EventArgs e)
        {
            if (License != null)
                DefaultApplicationData(License.LicenseID);
        }

        void ReplaceLicense(clsApplication app)
        {
            clsLicense ReplacedLicense = new clsLicense();

            ReplacedLicense.ApplicationID = app.ApplicationID;
            ReplacedLicense.DriverID = License.DriverID;
            ReplacedLicense.LicenseClassID = License.LicenseClassInfo.LicenseClassID;
            ReplacedLicense.IssueDate = DateTime.Now;
            ReplacedLicense.ExpirationDate = License.ExpirationDate;
            ReplacedLicense.PaidFees = License.PaidFees;
            ReplacedLicense.IsActive = true;
            ReplacedLicense.IssueReason = (rbReplaceLostLicense.Checked) ? (byte)3 /*Lost*/ : (byte)4 /*Damaged*/;
            ReplacedLicense.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            if (ReplacedLicense.Save())
            {
                lblLicenseReplacementApplicationID.Text = "License Replacement Application ID : " + app.ApplicationID;
                lblReplacedLicenseID.Text = "Replaced License ID : " + ReplacedLicense.LicenseID;
                lblReplace.Enabled = false;
                lblShowLicenseInfo.Enabled = true;
                LicenseFilterCard.Enabled = false;
                lblShowLicensesHistory.Enabled = true;

                License._DeactivateLicense();

                MessageBox.Show("New License with ID [" + ReplacedLicense.LicenseID + "] has been saved successfully!",
                    "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            else
            {
                clsApplication._DeleteApplication(app.ApplicationID);
                MessageBox.Show("Failed to replace License!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblReplace_Click(object sender, EventArgs e)
        {
            if (License == null)
            {
                MessageBox.Show("No License selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsLicenseValid())
                return;

            clsApplicationType appType = clsApplicationType._GetApplicationInfoByID(rbReplaceLostLicense.Checked ? 3 : 4); // = Lost, 4 = Damaged

            if (appType == null)
            {
                MessageBox.Show("Application Type not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!(rbReplaceLostLicense.Checked && rbReplaceDamagedLicense.Checked))
            {
                MessageBox.Show("Replacement Type not selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsApplication app = new clsApplication();
            app.ApplicantPersonID = License.DriverInfo.PersonID;
            app.ApplicationDate = DateTime.Now;
            app.ApplicationTypeID = appType.ApplicationTypeID;
            app.ApplicationStatus = 3; // completed
            app.PaidFees = appType.ApplicationFees;
            app.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            if (app.Save())
            {
                ReplaceLicense(app);
            }

            else
                MessageBox.Show("Failed to create the system application!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}