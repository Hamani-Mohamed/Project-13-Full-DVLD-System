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
    public partial class frmChangePassword : Form
    {
        int _PersonID;
        clsUser _User;

        public frmChangePassword(int personID)
        {
            InitializeComponent();
            _PersonID = personID;
        }

        public void _LoadData()
        {
            cntrlUserCard userCard = new cntrlUserCard(_PersonID);
            userCard.Dock = DockStyle.Fill;
            pnlUserCard.Controls.Add(userCard);

            _User = clsUser.FindUserByPersonID(_PersonID);
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCurrentPassword.Text))
            {
                errorProvider1.SetError(txtCurrentPassword, "Current password required!");
                return;
            }

            if (txtCurrentPassword.Text != _User.Password)
            {
                errorProvider1.SetError(txtCurrentPassword, "Wrong Password! Try Again...");
                return;
            }

            errorProvider1.SetError(txtCurrentPassword, "");
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text))
            {
                errorProvider1.SetError(txtNewPassword, "New password required!");
                return;
            }

            if (txtNewPassword.Text.Length < 8)
            {
                errorProvider1.SetError(txtNewPassword, "Password must be 8 characters or higher!");
                return;
            }

            errorProvider1.SetError(txtNewPassword, "");
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtConfirmPassword.Text))
            {
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation required!");
                return;
            }

            if (txtConfirmPassword.Text != txtNewPassword.Text)
            {
                errorProvider1.SetError(txtConfirmPassword, "Passwords don't match!");
                return;
            }

            errorProvider1.SetError(txtConfirmPassword, "");
        }

        private bool IsDataValid()
        {
            this.ValidateChildren();

            return string.IsNullOrEmpty(errorProvider1.GetError(txtCurrentPassword)) &&
                   string.IsNullOrEmpty(errorProvider1.GetError(txtNewPassword)) &&
                   string.IsNullOrEmpty(errorProvider1.GetError(txtConfirmPassword));
        }

        private void lblSave_Click(object sender, EventArgs e)
        {
            if (!IsDataValid())
            {
                MessageBox.Show("Please fix the errors marked with the red icon.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _User.Password = txtNewPassword.Text.Trim();

            if (_User.Save())
            {
                MessageBox.Show("Password changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }
            else
            {
                MessageBox.Show("An error occurred while saving. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
