using DVLD.Properties;
using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD
{
    public partial class frmIssueLicenseFirstTime : Form
    {
        int _LocalLicenseID;
        cntrlApplicationDetails appInfo;

        public frmIssueLicenseFirstTime(int locallicenseID)
        {
            InitializeComponent();
            _LocalLicenseID = locallicenseID;
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            appInfo = new cntrlApplicationDetails(_LocalLicenseID);
            appInfo.Dock = DockStyle.Fill;
            pnlApplicationInfo.Controls.Add(appInfo);
        }

        private void FillLicenseData(clsLocalLicense localLicense, clsDriver driver, clsLicense license)
        {
            license.ApplicationID = localLicense.ApplicationID;
            license.DriverID = driver.DriverID;
            license.LicenseClassID = localLicense.LicenseClassID;
            license.IssueDate = DateTime.Now;
            license.ExpirationDate = DateTime.Now.AddYears(localLicense.LicenseClassInfo.DefaultValidityLength);
            license.Notes = txtNotes.Text.Trim();
            license.PaidFees = localLicense.LicenseClassInfo.ClassFees;
            license.IsActive = true;
            license.IssueReason = 1;
            license.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;
        }

        private void lblIssue_Click(object sender, EventArgs e)
        {
            clsLocalLicense localLicense = clsLocalLicense.FindLocalLicenseByID(_LocalLicenseID);
            clsDriver driver = clsDriver._GetDriverInfoByPersonID(localLicense.ApplicantPersonID);

            if (driver == null)
            {
                driver = new clsDriver();
                driver.PersonID = localLicense.ApplicantPersonID;
                driver.CreationDate = DateTime.Now;
                driver.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

                if (!driver.Save())
                {
                    MessageBox.Show("Error: Could not create driver record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            clsLicense license = new clsLicense();

            FillLicenseData(localLicense, driver, license);

            if (license.Save())
            {
                localLicense._SetComplete();

                MessageBox.Show($"License Issued Successfully with ID: {license.LicenseID}", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblIssue.Enabled = false;
                txtNotes.Enabled = false;
                appInfo.ShowLicenseInfo.Enabled = true;
            }
            else
            {
                MessageBox.Show("Error: Could not issue the license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}