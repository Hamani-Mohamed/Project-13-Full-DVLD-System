using DVLD.Properties;
using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace DVLD
{
    public partial class cntrlAdd_EditPerson : UserControl
    {
        public enum enMode { AddNew, Update };
        enMode _Mode;
        int _PersonID;
        clsPerson _Person;
        bool PhotoSelected = false;
        public delegate void DataBackHandler(object sender, int PersonID);
        public event DataBackHandler DataBack;

        public cntrlAdd_EditPerson(int personID)
        {
            InitializeComponent();
            _PersonID = personID;

            if (_PersonID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
        }

        private void cntrlAdd_EditPerson_Load(object sender, EventArgs e)
        {
            _SetupDatePicker();
            rbMale.Checked = true;
            _LoadData();
        }

        private void _FillCountriesInComboBox()
        {
            cbCountries.Items.Clear();
            DataTable dt = clsCountry._GetAllCountries();

            foreach (DataRow row in dt.Rows)
            {
                cbCountries.Items.Add(row["CountryName"]);
            }
        }

        private void _SetupDatePicker()
        {
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;
        }

        private void _MakeImageRound(PictureBox pb)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddEllipse(0, 0, pb.Width - 1, pb.Height - 1);
            pb.Region = new Region(gp);
            pbPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMale.Checked && !PhotoSelected)
                pbPhoto.Image = Resources.man;
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFemale.Checked && !PhotoSelected)
                pbPhoto.Image = Resources.woman;
        }

        private void lblSetImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog1.FileName;
                pbPhoto.ImageLocation = selectedFilePath;
                _MakeImageRound(pbPhoto);
                PhotoSelected = true;
                lblRemoveImage.Enabled = true;
            }
        }

        private void lblRemoveImage_Click(object sender, EventArgs e)
        {
            PhotoSelected = false;
            pbPhoto.ImageLocation = null;

            if (rbMale.Checked)
                pbPhoto.Image = Resources.man;
            else
                pbPhoto.Image = Resources.woman;

            lblRemoveImage.Enabled = false;
        }

        private void txtRequiredFields_Validating(object sender, CancelEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (string.IsNullOrEmpty(txt.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txt, txt.Tag.ToString() + " is required!");
            }

            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txt, "");
            }
        }

        private void cbCountries_Validating(object sender, CancelEventArgs e)
        {
            if (cbCountries.SelectedIndex == -1 || string.IsNullOrWhiteSpace(cbCountries.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(cbCountries, "Country is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(cbCountries, "");
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNationalNo.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is required!");
                return;
            }

            if (_Mode == enMode.AddNew && clsPerson._DoesPersonExist((txtNationalNo.Text)))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number already exists!");
            }

            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNationalNo, "");
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text)) return;

            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Please enter a valid email address!");
            }

            if (txtEmail.Text.Length < 7)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Email too short!");
            }

            else
            {
                errorProvider1.SetError(txtEmail, "");
            }
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPhone.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPhone, "Phone Number is required!");
                return;
            }

            if (!txtPhone.Text.All(char.IsDigit))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPhone, "Phone Number must contain only numbers (0-9)!");
            }

            if (txtPhone.Text.Length < 7)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPhone, "Phone Number too short!");
            }

            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPhone, "");
            }
        }

        private void _LoadData()
        {
            _FillCountriesInComboBox();
            cbCountries.SelectedIndex = cbCountries.FindString("United Kingdom");

            if (_Mode == enMode.AddNew)
            {
                _Person = new clsPerson();
                return;
            }

            _Person = clsPerson._FindPersonByID(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("No Person with ID [" + _PersonID + "] exists!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblPersonID.Text = "Person ID : " + _PersonID.ToString();
            txtNationalNo.Text = _Person.NationalNo;
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            dtpDateOfBirth.Value = _Person.DateOfBirth;

            if (_Person.Gender == 0)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            cbCountries.SelectedIndex = cbCountries.FindString(clsCountry.Find(_Person.NationalityCountryID).CountryName);
            txtEmail.Text = _Person.Email;
            txtPhone.Text = _Person.Phone;
            txtAddress.Text = _Person.Address;

            if (!string.IsNullOrEmpty(_Person.ImagePath) && File.Exists(_Person.ImagePath))
            {
                using (FileStream fs = new FileStream(_Person.ImagePath, FileMode.Open, FileAccess.Read))
                {
                    pbPhoto.Image = System.Drawing.Image.FromStream(fs);
                }
                pbPhoto.ImageLocation = _Person.ImagePath;

                PhotoSelected = true;
                lblRemoveImage.Enabled = true;
                _MakeImageRound(pbPhoto);
            }

            else
            {
                pbPhoto.Image = (rbMale.Checked) ? Resources.man : Resources.woman;
                PhotoSelected = false;
                lblRemoveImage.Enabled = false;
            }
        }

        private bool SaveImageToFolder()
        {
            if (_Person.ImagePath == pbPhoto.ImageLocation)
            {
                return true;
            }

            if (!string.IsNullOrEmpty(_Person.ImagePath))
            {
                try
                {
                    if (File.Exists(_Person.ImagePath))
                        File.Delete(_Person.ImagePath);
                }
                catch (IOException)
                {
                    // might log later
                }
            }

            if (!string.IsNullOrEmpty(pbPhoto.ImageLocation))
            {
                string sourceImageFile = pbPhoto.ImageLocation;

                if (clsUtil.CopyImageToProjectFolder(ref sourceImageFile))
                {
                    pbPhoto.ImageLocation = sourceImageFile;
                    return true;
                }
                else
                {
                    MessageBox.Show("Error copying Image to Folder!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private void lblSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Please fill all the required Information!", "Information Missing...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!SaveImageToFolder())
                return;

            int CountryID = clsCountry.Find(cbCountries.Text).ID;

            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.Gender = (byte)(rbMale.Checked ? 0 : 1);
            _Person.Email = txtEmail.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            _Person.NationalityCountryID = CountryID;
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.ImagePath = (PhotoSelected && !string.IsNullOrEmpty(pbPhoto.ImageLocation))
                    ? pbPhoto.ImageLocation
                    : "";

            if (_Person.Save())
            {
                _PersonID = _Person.PersonID;
                MessageBox.Show("Person Data saved successfully!", "Saved!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _Mode = enMode.Update;
                lblPersonID.Text = "Person ID : " + _PersonID.ToString();
                DataBack?.Invoke(this, _PersonID);
            }
            else
                MessageBox.Show("Person Data not saved!", "Cancelled!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            groupBox1.Enabled = false;
        }
    }
}