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
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmTakeTest : Form
    {
        int _AppointmentID;
        int _LocalLicenseID;
        int _TestTypeID;

        public frmTakeTest(int AppointmentID, int localLicenseID, int testtypeID)
        {
            InitializeComponent();
            _AppointmentID = AppointmentID;
            _LocalLicenseID = localLicenseID;
            _TestTypeID = testtypeID;
        }

        void _LoadTestData()
        {
            clsLocalLicense license = clsLocalLicense.FindLocalLicenseByID(_LocalLicenseID);

            if (license == null)
            {
                MessageBox.Show("Error: Could not find application with ID " + _LocalLicenseID);
                this.Close();
                return;
            }

            clsTestAppointment appointment = clsTestAppointment._FindByTestAppointmentID(_AppointmentID);

            if (appointment == null)
            {
                MessageBox.Show("Error: Could not find appointment with ID " + _AppointmentID);
                this.Close();
                return;
            }

            int trialCount = clsTestAppointment._GetTotalTrialsPerTestType(_LocalLicenseID, _TestTypeID);
            trialCount--;

            lblDLAppID.Text = "DL Application ID : " + _LocalLicenseID;
            lblName.Text = "Name : " + license.PersonInfo.FullName;
            lblClassName.Text = "Class Name : " + license.LicenseClassInfo.ClassName;
            lblTrial.Text = "Trial : " + trialCount;
            lblFees.Text = "Fees : " + clsTestType._GetTestInfoByID(_TestTypeID).TestTypeFees.ToString();
            lblDate.Text = "Date : " + appointment.AppointmentDate.ToShortDateString().ToString();
        }

        void _LoadTestPicture()
        {
            if (_TestTypeID == 1)
                pbTestPhoto.Image = Resources.vision_test;
            else if (_TestTypeID == 2)
                pbTestPhoto.Image = Resources.written_test;
            else
                pbTestPhoto.Image = Resources.street_test;
        }

        private void frmAdd_EditAppointment_Load(object sender, EventArgs e)
        {
            _LoadTestData();
            _LoadTestPicture();
        }

        private void lblSave_Click(object sender, EventArgs e)
        {
            clsTest test = new clsTest();

            test.TestAppointmentID = _AppointmentID;
            test.Notes = txtNotes.Text;
            test.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

            if (rbPass.Checked)
                test.TestResult = true;
            else
                test.TestResult = false;

            if (test.Save())
            {
                clsTestAppointment appointment = clsTestAppointment._FindByTestAppointmentID(_AppointmentID);
                appointment.IsLocked = true;
                appointment.Save();

                MessageBox.Show("Data saved successfully!", "Saved!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblSave.Enabled = false;
                rbPass.Enabled = false;
                rbFail.Enabled = false;
                txtNotes.Enabled = false;
            }

            else
            {
                MessageBox.Show("Failed to save data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}