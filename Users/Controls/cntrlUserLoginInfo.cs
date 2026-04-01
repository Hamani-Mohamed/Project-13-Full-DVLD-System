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
    public partial class cntrlUserLoginInfo : UserControl
    {
        int _PersonID;
        clsUser user;

        public cntrlUserLoginInfo(int personID)
        {
            InitializeComponent();
            _PersonID = personID;
        }

        private void cntrlUserLoginInfo_Load(object sender, EventArgs e)
        {
            user = clsUser.FindUserByPersonID(_PersonID);

            if (user != null)
            {
                lblUserID.Text = "User ID :        " + user.UserID;
                txtUsername.Text = user.Username;
                txtPassword.Text = user.Password;
                txtConfirm.Text = user.Password;
                chkActive.Checked = user.IsActive;
            }
            else
                user = new clsUser();
        }

        private void txtUsernameAndPassword_Validating(object sender, CancelEventArgs e)
        {
            TextBox txt = sender as TextBox;

            if (string.IsNullOrEmpty(txt.Text))
            {
                errorProvider1.SetError(txt, txt.Tag.ToString() + " is required!");
                return;
            }

            if (txtPassword.Text.Length < 8)
            {
                errorProvider1.SetError(txtPassword, "Password must be 8 characters or higher!");
                return;
            }

            if (txtUsername.Text.Trim() != user.Username && clsUser._DoesUserExistByUsername(txtUsername.Text.Trim()))
            {
                errorProvider1.SetError(txtUsername, "Username already in use by another account!");
                return;
            }

            errorProvider1.SetError(txt, "");
        }

        private void txtConfirm_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtConfirm.Text))
            {
                errorProvider1.SetError(txtConfirm, "Password Confirmation is required!");
            }

            else if (txtConfirm.Text != txtPassword.Text)
            {
                errorProvider1.SetError(txtConfirm, "Passwords don't match!");
            }

            else
            {
                errorProvider1.SetError(txtConfirm, "");
            }
        }

        private bool IsDataValid()
        {
            this.ValidateChildren();

            return string.IsNullOrEmpty(errorProvider1.GetError(txtUsername)) &&
                   string.IsNullOrEmpty(errorProvider1.GetError(txtPassword)) &&
                   string.IsNullOrEmpty(errorProvider1.GetError(txtConfirm)) &&
                   txtPassword.Text == txtConfirm.Text;
        }

        private void lblSave_Click(object sender, EventArgs e)
        {
            if (!IsDataValid())
            {
                MessageBox.Show("Please fix the errors marked with the red icon.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // clsPerson person = clsPerson.Find(_PersonID);

            user.PersonID = _PersonID;
            user.FullName = user.PersonInfo.FullName;
            user.Username = txtUsername.Text.Trim();
            user.Password = txtPassword.Text.Trim();
            user.IsActive = chkActive.Checked;

            if (user.Save())
            {
                lblUserID.Text = "User ID :        " + user.UserID;
                txtUsername.Text = user.Username;
                txtPassword.Text = user.Password;
                txtConfirm.Text = user.Password;
                chkActive.Checked = user.IsActive;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPassword.Enabled = false;
                txtConfirm.Enabled = false;
                txtUsername.Enabled = false;
                chkActive.Enabled = false;
                lblSave.Enabled = false;
            }

            else
            {
                MessageBox.Show("Error: Data could not be saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}