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
    public partial class cntrlShowLocalLicenseInfo : UserControl
    {
        private int _LicenseID;
        private int _PersonID;
        public enum enMode { AddNew, Update };
        enMode _Mode;

        public cntrlShowLocalLicenseInfo(int licenseID, int personID)
        {
            InitializeComponent();
            _LicenseID = licenseID;
            _PersonID = personID;

            if (licenseID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
        }

        private void _MakeImageRound(PictureBox pb)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, pb.Width - 1, pb.Height - 1);
            pb.Region = new Region(gp);
            pbPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        public void LoadPersonImage(clsPerson person)
        {
            if (person != null)
            {
                if (!string.IsNullOrEmpty(person.ImagePath) && File.Exists(person.ImagePath))
                {
                    using (FileStream fs = new FileStream(person.ImagePath, FileMode.Open, FileAccess.Read))
                    {
                        pbPhoto.Image = System.Drawing.Image.FromStream(fs);
                        _MakeImageRound(pbPhoto);
                    }
                }
                else
                {
                    pbPhoto.Image = (person.Gender == 0) ? Resources.man : Resources.woman;
                }
            }
        }

        void ResetDefaultValues()
        {
            lblName.Text = "Name : ";
            lblNationalNo.Text = "National No : ";
            lblDateOfBirth.Text = "Date Of Birth : ";
            lblIssueDate.Text = "Issue Date : ";
            lblIssueReason.Text = "Issue Reason : ";
            lblExpirationDate.Text = "Expiration Date : ";
            txtNotes.Text = "";
            lblClass.Text = "Class : ";
            lblLicenseID.Text = "License ID : ";
            lblGender.Text = "Gender : ";
            lblDriverID.Text = "Driver ID : ";
            lblIsActive.Text = "Is Active ? ";
            lblIsDetained.Text = "Is Detained ? ";
        }

        private void _UpdateControlData()
        {
            if (_Mode == enMode.AddNew)
            {
                ResetDefaultValues();
                return;
            }

            clsLicense license = clsLicense._FindByLicenseID(_LicenseID);
            bool IsDetained = clsDetainedLicense._IsLicenseDetained(_LicenseID);

            if (license == null)
            {
                MessageBox.Show("No License found with ID = " + _LicenseID);
                return;
            }

            lblName.Text = "Name : " + license.DriverInfo.PersonInfo.FullName;
            lblNationalNo.Text = "National No : " + license.DriverInfo.PersonInfo.NationalNo;
            lblDateOfBirth.Text = "Date Of Birth : " + license.DriverInfo.PersonInfo.DateOfBirth.ToShortDateString();
            lblIssueDate.Text = "Issue Date : " + license.IssueDate.ToShortDateString();

            if (license.IssueReason == 1)
                lblIssueReason.Text = "Issue Reason : First Time";

            else if (license.IssueReason == 2)
                lblIssueReason.Text = "Issue Reason : Renew ";

            else if (license.IssueReason == 3)
                lblIssueReason.Text = "Issue Reason : Replacement for Damaged";

            else
                lblIssueReason.Text = "Issue Reason : Replacement for Lost";

            lblExpirationDate.Text = "Expiration Date : " + license.ExpirationDate.ToShortDateString();

            if (license.Notes == null || license.Notes == "")
                txtNotes.Text = "No Notes.";
            else
                txtNotes.Text = license.Notes;

            lblClass.Text = "Class : " + license.LicenseClassInfo.ClassName;
            lblLicenseID.Text = "License ID : " + _LicenseID;
            lblGender.Text = (license.DriverInfo.PersonInfo.Gender == 0) ? "Gender : Male" : "Gender : Female";
            lblDriverID.Text = "Driver ID : " + license.DriverID;
            lblIsActive.Text = (license.IsActive == false) ? "Is Active ? No" : "Is Active ? Yes";
            lblIsDetained.Text = (IsDetained) ? "Is Detained ? Yes" : "Is Detained ? No";

            LoadPersonImage(license.DriverInfo.PersonInfo);
        }

        private void cntrlPersonCard_Load(object sender, EventArgs e)
        {
            _UpdateControlData();
        }

        private void lblEditPerson_Click(object sender, EventArgs e)
        {
            Form frm = new frmAdd_EditPerson(_PersonID);
            frm.ShowDialog();
            _UpdateControlData();
        }
    }
}