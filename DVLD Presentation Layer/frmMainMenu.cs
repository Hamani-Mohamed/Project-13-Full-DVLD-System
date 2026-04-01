using DVLD.Applications.Test_Types.Controls;
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
    public partial class frmMainMenu : Form
    {
        Form _LoginForm;

        public frmMainMenu(Form frmLogIn)
        {
            InitializeComponent();
            _LoginForm = frmLogIn;
        }

        private void frmMainMenu_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.DoubleBuffered = true;
        }

        bool IsSideBarExpanded = false;
        bool AreHeadlinesExpanded = false;
        Control CurrentHeadline;
        UserControl CurrentControl;

        void ExpandSideBar()
        {
            IsSideBarExpanded = true;
            tmrSideBar.Start();
        }

        void CollapseAllHeadlines(Control cntrl)
        {
            foreach (Control c in cntrl.Controls)
            {
                if (c.HasChildren)
                {
                    c.Height = c.MinimumSize.Height;
                    CollapseAllHeadlines(c);
                }
            }
        }

        void CollapseSideBar()
        {
            tmrHeadlines.Stop();
            CurrentHeadline = null;
            AreHeadlinesExpanded = false;

            CollapseAllHeadlines(flpHeaders);

            IsSideBarExpanded = false;
            tmrSideBar.Start();
        }

        private void tmrSideBar_Tick(object sender, EventArgs e)
        {
            if (IsSideBarExpanded)
            {
                if (pnlSideBar.Width < pnlSideBar.MaximumSize.Width)
                {
                    pnlSideBar.Width += 15;
                }
                else
                {
                    tmrSideBar.Stop();
                }
            }
            else
            {
                if (pnlSideBar.Width > pnlSideBar.MinimumSize.Width)
                {
                    pnlSideBar.Width -= 20;
                }
                else
                {
                    tmrSideBar.Stop();
                }
            }
        }

        void ExpandHeadlines(Control headline)
        {
            CurrentHeadline = headline;
            AreHeadlinesExpanded = true;
            tmrHeadlines.Start();
        }

        void CollapseHeadlines(Control headline)
        {
            CurrentHeadline = headline;
            AreHeadlinesExpanded = false;
            tmrHeadlines.Start();
        }

        private int GetFullHeight(Control container)
        {
            int height = 0;
            foreach (Control c in container.Controls)
            {
                height += c.Height + c.Margin.Top + c.Margin.Bottom;
            }
            return height;
        }

        private void UpdateParentLayout(Control child)
        {
            Control parent = child.Parent;

            if (parent != null && parent is FlowLayoutPanel && parent != flpHeaders)
            {
                parent.Height = GetFullHeight(parent);
                UpdateParentLayout(parent);
            }
        }

        private void HeadlinesTimer_Tick(object sender, EventArgs e)
        {
            if (CurrentHeadline == null) return;

            int TargetHeight = GetFullHeight(CurrentHeadline);

            if (AreHeadlinesExpanded)
            {
                if (CurrentHeadline.Height < TargetHeight)
                {
                    CurrentHeadline.Height += 15;
                }
                else
                {
                    CurrentHeadline.Height = TargetHeight;
                    tmrHeadlines.Stop();
                }
            }
            else
            {
                if (CurrentHeadline.Height > CurrentHeadline.MinimumSize.Height)
                {
                    CurrentHeadline.Height -= 15;
                }
                else
                {
                    CurrentHeadline.Height = CurrentHeadline.MinimumSize.Height;
                    tmrHeadlines.Stop();
                }
            }

            UpdateParentLayout(CurrentHeadline);
        }

        void ToggleHeadlines(Control headline)
        {
            CurrentHeadline = headline;

            if (headline.Height == headline.MinimumSize.Height)
            {
                ExpandHeadlines(headline);
            }
            else
            {
                CollapseHeadlines(headline);
            }
        }

        private void Hamburger_Click(object sender, EventArgs e)
        {
            if (!IsSideBarExpanded)
            {
                ExpandSideBar();
            }
            else
            {
                CollapseSideBar();
            }
        }

        private void Headlines_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.ForeColor = Color.FromArgb(187, 171, 241);
        }

        private void Headlines_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.ForeColor = Color.FromArgb(223, 216, 245);
        }

        void CollapseOtherHeadlines(Control cntrl, Control headline)
        {
            foreach (Control c in cntrl.Controls)
            {
                if (c != headline && c is FlowLayoutPanel)
                {
                    c.Height = c.MinimumSize.Height;
                }
            }
        }

        private void Applications_Click(object sender, EventArgs e)
        {
            if (!IsSideBarExpanded)
                ExpandSideBar();

            CollapseOtherHeadlines(flpHeaders, flpApplications);
            ToggleHeadlines(flpApplications);
        }

        void LoadControl(UserControl cntrl)
        {
            if (CurrentControl != null && CurrentControl.GetType() == cntrl.GetType())
            {
                return;
            }

            if (pnlSideBar.Height > pnlSideBar.MinimumSize.Height)
                CollapseSideBar();

            pnlContent.Controls.Clear();
            cntrl.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(cntrl);

            CurrentControl = cntrl;
        }

        void LoadForm(Form form)
        {
            Form frm = form;
            CollapseSideBar();

            using (frm)
            {
                frm.ShowDialog();
            }

            CurrentControl = null;
        }

        private void People_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlPeople());
        }

        private void Drivers_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlDrivers());
        }

        private void Users_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlUsers());
        }

        private void AccountSettings_Click(object sender, EventArgs e)
        {
            if (!IsSideBarExpanded)
                ExpandSideBar();

            CollapseOtherHeadlines(flpHeaders, flpAccountSettings);
            ToggleHeadlines(flpAccountSettings);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
            _LoginForm.Close();
        }

        private void DrivingLicenseServices_Click(object sender, EventArgs e)
        {
            CollapseOtherHeadlines(flpApplications, flpDrivingLicenseServices);
            ToggleHeadlines(flpDrivingLicenseServices);
        }

        private void NewDrivingLicense_Click(object sender, EventArgs e)
        {
            ToggleHeadlines(flpNewDrivingLicense);
        }

        private void lblManageApplications_Click(object sender, EventArgs e)
        {
            CollapseOtherHeadlines(flpApplications, flpManageApplications);
            ToggleHeadlines(flpManageApplications);
        }

        private void lblDetainLicenses_Click(object sender, EventArgs e)
        {
            CollapseOtherHeadlines(flpApplications, flpDetainLicenses);
            ToggleHeadlines(flpDetainLicenses);
        }

        private void lblManageApplicationTypes_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlApplicationTypes());
        }

        private void lblManageTestTypes_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlTestTypes());
        }

        private void lblLocalLicense_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlLocalLicenses());
            LoadForm(new frmNewLocalDrivingLicenseApplication(-1, -1));
            LoadControl(new cntrlLocalLicenses());
        }

        private void lblInternationalLicense_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlInternationalLicenses());
            LoadForm(new frmNewInternationalDrivingLicense(-1));
            LoadControl(new cntrlInternationalLicenses());
        }

        private void lblRenewDrivingLicense_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlLocalLicenses());
            LoadForm(new frmRenewLicense(-1));
            LoadControl(new cntrlLocalLicenses());
        }

        private void lblReplacementForLostOrDamagedLicense_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlLocalLicenses());
            LoadForm(new frmReplaceLicense(-1));
            LoadControl(new cntrlLocalLicenses());
        }

        private void lblReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlDetainedLicenses());
            LoadForm(new frmReleaseDetainedLicense(-1));
            LoadControl(new cntrlDetainedLicenses());
        }

        private void lblRetakeTest_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlLocalLicenses());
        }

        private void lblLocalDrivingLicenseApplications_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlLocalLicenses());
        }

        private void lblInternationalDrivingLicenseApplications_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlInternationalLicenses());
        }

        private void lblManageDetainedLicenses_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlDetainedLicenses());
        }

        private void lblDetainLicense_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlDetainedLicenses());
            LoadForm(new frmNewDetainedLicense(-1));
            LoadControl(new cntrlDetainedLicenses());
        }

        private void lblReleaseDetainedLicense2_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlDetainedLicenses());
            LoadForm(new frmReleaseDetainedLicense(-1));
            LoadControl(new cntrlDetainedLicenses());
        }

        private void lblUserInfo_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlUsers());
            LoadForm(new frmShowPersonDetails(clsGlobalSettings.CurrentUser.PersonID));
        }

        private void lblChangePassword_Click(object sender, EventArgs e)
        {
            LoadControl(new cntrlUsers());
            LoadForm(new frmChangePassword(clsGlobalSettings.CurrentUser.PersonID));
        }

        private void lblSignOut_Click(object sender, EventArgs e)
        {
            clsGlobalSettings.CurrentUser = null;
            this.Close();
            _LoginForm.Show();
        }
    }
}