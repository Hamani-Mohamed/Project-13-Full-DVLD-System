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
    public partial class cntrlShowInternationalLicenseInfo : UserControl
    {
        private int _InterLicenseID;

        public cntrlShowInternationalLicenseInfo(int interLicenseID)
        {
            InitializeComponent();
            _InterLicenseID = interLicenseID;
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

        private void _UpdateControlData()
        {
            clsInternationalLicense InternationalLicense = clsInternationalLicense._FindByID(_InterLicenseID);

            if (InternationalLicense == null)
            {
                MessageBox.Show("No License found with ID = " + _InterLicenseID);
                return;
            }

            lblName.Text = "Name : " + InternationalLicense.DriverInfo.PersonInfo.FullName;
            lblNationalNo.Text = "National No : " + InternationalLicense.DriverInfo.PersonInfo.NationalNo;
            lblDateOfBirth.Text = "Date Of Birth : " + InternationalLicense.DriverInfo.PersonInfo.DateOfBirth.ToShortDateString();
            lblGender.Text = (InternationalLicense.DriverInfo.PersonInfo.Gender == 0) ? "Gender : Male" : "Gender : Female";
            lblIssueDate.Text = "Issue Date : " + InternationalLicense.IssueDate.ToShortDateString();
            lblExpirationDate.Text = "Expiration Date : " + InternationalLicense.ExpirationDate.ToShortDateString();
            lblInternationalLicenseID.Text = "International License ID : " + InternationalLicense.InternationalLicenseID;
            lblLicenseID.Text = "License ID : " + InternationalLicense.IssuedUsingLocalLicenseID;
            lblDriverID.Text = "Driver ID : " + InternationalLicense.DriverID;
            lblIsActive.Text = (InternationalLicense.IsActive == false) ? "Is Active ? No" : "Is Active ? Yes";

            LoadPersonImage(InternationalLicense.DriverInfo.PersonInfo);
        }

        private void cntrlShowInternationalLicenseInfo_Load(object sender, EventArgs e)
        {
            _UpdateControlData();
        }
    }
}