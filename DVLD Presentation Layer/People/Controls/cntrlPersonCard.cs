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
    public partial class cntrlPersonCard : UserControl
    {
        private int _PersonID;
        clsPerson _Person;
        clsCountry _Country;

        public cntrlPersonCard(int personID)
        {
            InitializeComponent();
            _PersonID = personID;
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
            if (_PersonID == -1)
            {
                lblEditPersonInfo.Enabled = false;
                return;
            }

            _Person = clsPerson._FindPersonByID(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("No Person found with ID = " + _PersonID);
                return;
            }

            _Country = clsCountry.Find(_Person.NationalityCountryID);

            lblEditPersonInfo.Enabled = true;

            lblPersonID.Text = "Person ID : " + _Person.PersonID;
            lblNationalNo.Text = "National No : " + _Person.NationalNo;
            lblName.Text = "Name : " + _Person.FullName;
            lblGender.Text = (_Person.Gender == 0) ? "Gender : Male" : "Gender : Female";
            lblDateOfBirth.Text = "Date Of Birth : " + _Person.DateOfBirth.ToShortDateString();
            lblEmail.Text = "Email : " + _Person.Email;
            lblPhone.Text = "Phone : " + _Person.Phone;
            lblAddress.Text = "Address : " + _Person.Address;
            lblCountry.Text = "Country : " + _Country.CountryName;

            LoadPersonImage(_Person);
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