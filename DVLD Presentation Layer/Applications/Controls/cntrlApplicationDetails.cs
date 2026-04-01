using DVLD.Properties;
using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class cntrlApplicationDetails : UserControl
    {
        private int _LocalLicenseID;
        clsLocalLicense _LocalLicense;
        public Label ShowLicenseInfo
        {
            get
            {
                return lblShowLicenseInfo;
            }
        }

        public cntrlApplicationDetails(int locallicenseID)
        {
            InitializeComponent();
            _LocalLicenseID = locallicenseID;
        }

        public void _UpdateControlData()
        {
            if (_LocalLicenseID == -1)
            {
                lblShowLicenseInfo.Enabled = false;
                return;
            }

            _LocalLicense = clsLocalLicense.FindLocalLicenseByID(_LocalLicenseID);

            if (_LocalLicense == null)
            {
                MessageBox.Show("No License found with ID = " + _LocalLicenseID);
                return;
            }

            string LicenseClass = _LocalLicense.LicenseClassInfo.ClassName;
            string ApplicationType = _LocalLicense.ApplicationTypeInfo.ApplicationTypeTitle;
            string Username = _LocalLicense.UserInfo.Username;
            string FullName = _LocalLicense.PersonInfo.FullName;

            lblDLApplicationID.Text = "DL Application ID : " + _LocalLicenseID;
            lblPassedTests.Text = "Passed Tests : " + clsTestAppointment._GetPassedTestCount(_LocalLicenseID).ToString() + "/3";
            lblAppliedForLicense.Text = "Applied For License : " + LicenseClass;
            lblID.Text = "ID : " + _LocalLicense.ApplicationID;
            lblFees.Text = "Fees : " + _LocalLicense.PaidFees.ToString();
            lblType.Text = "Type : " + ApplicationType;
            lblApplicant.Text = "Applicant : " + FullName;
            lblStatus.Text = "Status : " + (_LocalLicense.ApplicationStatus == 1 ? "New" : _LocalLicense.ApplicationStatus == 2 ? "Cancelled"
            : _LocalLicense.ApplicationStatus == 3 ? "Completed" : "");
            lblDate.Text = "Date : " + _LocalLicense.ApplicationDate.ToShortDateString().ToString();
            lblStatusDate.Text = "Status Date : " + _LocalLicense.LastStatusDate.ToShortDateString().ToString();
            lblCreatedBy.Text = "Created By : " + Username;

            clsLicense license = clsLicense._FindByApplicationID(_LocalLicense.ApplicationID);

            if (license == null)
            {
                lblShowLicenseInfo.Enabled = false;
            }
        }

        private void cntrlApplicationDetails_Load(object sender, EventArgs e)
        {
            _UpdateControlData();
        }

        private void lblShowLicenseInfo_Click(object sender, EventArgs e)
        {
            clsLicense license = clsLicense._FindByApplicationID(_LocalLicense.ApplicationID);

            Form frm = new frmShowLocalLicenseInfo(license.LicenseID, _LocalLicense.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void lblShowPersonInfo_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonDetails(_LocalLicense.ApplicantPersonID);
            frm.ShowDialog();
        }
    }
}