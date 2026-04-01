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
    public partial class cntrlLocalApplicationInfo : UserControl
    {
        int _PersonID, _LocalApplicationID;
        clsLocalLicense license;
        enum enMode { AddNew, UpdateMode };
        enMode Mode = enMode.UpdateMode;

        public cntrlLocalApplicationInfo(int personID, int applicationID)
        {
            InitializeComponent();
            _PersonID = personID;
            _LocalApplicationID = applicationID;

            if (_LocalApplicationID == -1)
                Mode = enMode.AddNew;
            else
                Mode = enMode.UpdateMode;
        }

        private void _FillLicenseClassesInComboBox()
        {
            cbLicenseClasses.Items.Clear();
            DataTable dt = clsLicenseClass._GetAllLicenseClasses();

            foreach (DataRow row in dt.Rows)
            {
                cbLicenseClasses.Items.Add(row["ClassName"]);
            }
        }

        private void cntrlApplicationInfo_Load(object sender, EventArgs e)
        {
            _FillLicenseClassesInComboBox();
            lblApplicationFees.Text = "Application Fees :      " + clsApplicationType._GetApplicationInfoByName("New Local Driving License Service").ApplicationFees;

            if (Mode == enMode.AddNew)
            {
                lblDLApplicationID.Text = "DL Application ID :      ???";
                lblApplicationDate.Text = "Application Date :      " + DateTime.Now.ToString();
                cbLicenseClasses.SelectedIndex = 2;
                lblCreatedBy.Text = "Created By :      " + clsGlobalSettings.CurrentUser.Username;
            }

            else
            {
                // --- OLD MANUAL CALLS (Commented Out) ---
                // clsApplication application = clsApplication._FindApplicationByPersonID(_PersonID);
                // clsUser user = clsUser.FindUserByUserID(application.CreatedByUserID);
                // clsLocalLicense license = clsLocalLicense.FindLocalLicenseByID(_LocalApplicationID);

                clsLocalLicense license = clsLocalLicense.FindLocalLicenseByID(_LocalApplicationID);

                lblDLApplicationID.Text = "DL Application ID :      " + license.LocalDrivingLicenseApplicationID;
                lblApplicationDate.Text = "Application Date :      " + license.ApplicationDate;
                cbLicenseClasses.SelectedIndex = license.ApplicationTypeID;
                lblCreatedBy.Text = "Created By :      " + license.UserInfo.Username;

                cbLicenseClasses.SelectedIndex = cbLicenseClasses.FindString(license.LicenseClassInfo.ClassName);

            }
        }

        private void _LoadApplicationData(int LocalApplicationID)
        {
            if (Mode == enMode.AddNew)
            {
                license = new clsLocalLicense();
                return;
            }
            else
            {
                license = clsLocalLicense.FindLocalLicenseByID(_LocalApplicationID);

                if (license == null)
                {
                    MessageBox.Show("Could not find application with ID " + LocalApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void lblSave_Click(object sender, EventArgs e)
        {
            _LoadApplicationData(_LocalApplicationID);

            license.LastStatusDate = DateTime.Now;
            license.LicenseClassID = clsLicenseClass._FindByName(cbLicenseClasses.Text).LicenseClassID;
            license.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            if (Mode == enMode.AddNew)
            {
                license.ApplicantPersonID = _PersonID;
                license.ApplicationDate = DateTime.Now;
                license.ApplicationTypeID = 1;
                license.ApplicationStatus = 1;
                license.PaidFees = clsApplicationType._GetApplicationInfoByID(1).ApplicationFees;

                if (clsApplication._DoesPersonHaveActiveApplication(_PersonID, license.LicenseClassID))
                {
                    MessageBox.Show("Person already has an active application for this class.",
                                    "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (license.Save())
            {
                Mode = enMode.UpdateMode;
                lblDLApplicationID.Text = "DL Application ID :      " + license.LocalDrivingLicenseApplicationID;
                _LocalApplicationID = license.LocalDrivingLicenseApplicationID;
                lblSave.Enabled = false;
                cbLicenseClasses.Enabled = false;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {
                MessageBox.Show("Error: Data could not be saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}