using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmLogIn : Form
    {
        public frmLogIn()
        {
            InitializeComponent();
        }

        private void frmLogIn_Load(object sender, EventArgs e)
        {
            chkRememberMe.Checked = true;

            if (clsGlobalSettings.LoadCredentialsFromRegistry())
            {
                txtUsername.Text = clsGlobalSettings.Username;
                txtPassword.Text = clsGlobalSettings.Password;
            }

            if (!string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtPassword.Text))
                btnLogIn.Select();
            else
                txtUsername.Select();
        }

        private void frmLogIn_Paint(object sender, PaintEventArgs e)
        {
            Color colorTop = Color.FromArgb(28, 15, 69);
            Color colorBottom = Color.FromArgb(48, 25, 89);

            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, colorTop, colorBottom, 90F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void Username_Password_Validating(object sender, CancelEventArgs e)
        {
            TextBox txt = sender as TextBox;

            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                errorProvider1.SetError(txt, txt.Tag.ToString() + " is required!");
            }

            else
            {
                errorProvider1.SetError(txt, "");
            }
        }

        private bool IsDataValid()
        {
            return string.IsNullOrEmpty(errorProvider1.GetError(txtUsername)) &&
                   string.IsNullOrEmpty(errorProvider1.GetError(txtPassword));
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            if (!IsDataValid())
                return;

            string username = txtUsername.Text;
            string password = txtPassword.Text;

            clsUser user = clsUser.FindUserByUsernameAndPassword(username, password);

            if (user != null && user.Username == username && user.Password == password)
            {
                if (!user.IsActive)
                {
                    MessageBox.Show("Your account is deactivated, contact your admin!", "Account Deactivated", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsGlobalSettings.CurrentUser = user;

                if (chkRememberMe.Checked)
                    clsGlobalSettings.SaveCredentialsInRegistry(username, password);
                else
                {
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                    clsGlobalSettings.SaveCredentialsInRegistry("", "");
                }

                Form frm = new frmMainMenu(this);
                this.Hide();
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Username or Password incorrect, Try again!", "Wrong Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
