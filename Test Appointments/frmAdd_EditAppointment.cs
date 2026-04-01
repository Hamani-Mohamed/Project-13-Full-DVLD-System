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
    public partial class frmAdd_EditAppointment : Form
    {
        int _AppointmentID;
        int _LocalLicenseID;
        int _TestTypeID;

        public enum enMode { AddNew, Update };
        enMode _Mode;

        public frmAdd_EditAppointment(int AppointmentID, int localLicenseID, int testtypeID)
        {
            InitializeComponent();
            _AppointmentID = AppointmentID;
            _LocalLicenseID = localLicenseID;
            _TestTypeID = testtypeID;

            if (_AppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
        }

        void LoadAsUpdate()
        {
            lblAddNewAppointment.Text = "Edit Appointment";
            lblAddNewAppointment.Location = new Point(400, lblAddNewAppointment.Location.Y);

            clsTestAppointment appointment = clsTestAppointment._FindByTestAppointmentID(_AppointmentID);
            if (appointment != null)
            {
                if (appointment.RetakeTestApplicationID != -1)
                    lblRTestAppID.Text = "R. Test App. ID : " + appointment.RetakeTestApplicationID.ToString();

                dtpAppointmentDate.MinDate = DateTime.MinValue;
                dtpAppointmentDate.Value = appointment.AppointmentDate;

                if (appointment.IsLocked)
                {
                    lblAppointmentLocked.Visible = true;
                    dtpAppointmentDate.Enabled = false;
                    lblSave.Enabled = false;
                    return;
                }

                dtpAppointmentDate.MinDate = DateTime.Now;
            }
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

            if (_Mode == enMode.Update)
            {
                LoadAsUpdate();
            }

            int trialCount = clsTestAppointment._GetTotalTrialsPerTestType(_LocalLicenseID, _TestTypeID);

            if (_Mode == enMode.AddNew)
            {
                dtpAppointmentDate.MinDate = DateTime.Now;
                gbRetakeTestInfo.Enabled = (trialCount > 0);
            }
            else
            {
                trialCount--;
                gbRetakeTestInfo.Enabled = (trialCount > 0);
            }

            decimal testFees = clsTestType._GetTestInfoByID(_TestTypeID).TestTypeFees;

            lblDLAppID.Text = "DL Application ID : " + _LocalLicenseID;
            lblName.Text = "Name : " + license.PersonInfo.FullName;
            lblClassName.Text = "Class Name : " + license.LicenseClassInfo.ClassName;
            lblTrial.Text = "Trial : " + trialCount;
            lblFees.Text = "Fees : " + testFees.ToString();

            if (trialCount > 0)
            {
                decimal retakeAppFee = clsApplicationType._GetApplicationInfoByID(7).ApplicationFees;
                lblRAppFees.Text = "R. App. Fees : " + retakeAppFee.ToString();
                lblTotalFees.Text = "Total Fees : " + (testFees + retakeAppFee).ToString();
            }
            else
            {
                lblTotalFees.Text = "Total Fees : " + testFees.ToString();
            }
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
            lblAppointmentLocked.Visible = false;

            _LoadTestData();
            _LoadTestPicture();
        }

        private void lblSave_Click(object sender, EventArgs e)
        {
            clsTestAppointment Appointment;

            if (_Mode == enMode.AddNew)
                Appointment = new clsTestAppointment();
            else
                Appointment = clsTestAppointment._FindByTestAppointmentID(_AppointmentID);

            int trialCount = clsTestAppointment._GetTotalTrialsPerTestType(_LocalLicenseID, _TestTypeID);

            if (_Mode == enMode.AddNew && trialCount > 0)
            {
                clsApplication RetakeApp = new clsApplication();

                clsLocalLicense localLicense = clsLocalLicense.FindLocalLicenseByID(_LocalLicenseID);

                RetakeApp.ApplicantPersonID = localLicense.ApplicantPersonID;
                RetakeApp.ApplicationDate = DateTime.Now;
                RetakeApp.ApplicationTypeID = 7;
                RetakeApp.ApplicationStatus = 1;
                RetakeApp.LastStatusDate = DateTime.Now;
                RetakeApp.PaidFees = clsApplicationType._GetApplicationInfoByID(7).ApplicationFees;
                RetakeApp.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;

                if (RetakeApp.Save())
                {
                    Appointment.RetakeTestApplicationID = RetakeApp.ApplicationID;
                }
                else
                {
                    MessageBox.Show("Failed to create retake application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            Appointment.TestTypeID = _TestTypeID;
            Appointment.LocalDrivingLicenseApplicationID = _LocalLicenseID;
            Appointment.AppointmentDate = dtpAppointmentDate.Value;
            Appointment.PaidFees = clsTestType._GetTestInfoByID(_TestTypeID).TestTypeFees;
            Appointment.CreatedByUserID = clsGlobalSettings.CurrentUser.UserID;
            Appointment.IsLocked = false;

            if (Appointment.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (Appointment.RetakeTestApplicationID != -1)
                    lblRTestAppID.Text = "R. Test App. ID : " + Appointment.RetakeTestApplicationID.ToString();
                _Mode = enMode.Update;
                _AppointmentID = Appointment.TestAppointmentID;
                lblSave.Enabled = false;
                dtpAppointmentDate.Enabled = false;
            }
            else
            {
                MessageBox.Show("Error: Data was not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}