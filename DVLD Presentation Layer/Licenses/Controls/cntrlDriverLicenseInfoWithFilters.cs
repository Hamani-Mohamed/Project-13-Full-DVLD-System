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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace DVLD
{
    public partial class cntrlDriverLicenseInfoWithFilters : UserControl
    {
        int _LicenseID;
        public enum enMode { AddNew, Update };
        enMode _Mode;
        clsLicense LicenseInfo;
        public event Action<int> LicenseSelected;
        protected virtual void OnLicenseSelected(int LicenseID)
        {
            Action<int> handler = LicenseSelected;
            if (handler != null)
            {
                handler(LicenseID);
            }
        }

        public cntrlDriverLicenseInfoWithFilters(int licenseID)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            _LicenseID = licenseID;

            if (_LicenseID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
        }

        void _LoadData(clsLicense LicenseInfo)
        {
            pnlLicenseInfo.Controls.Clear();

            int personID = (LicenseInfo != null) ? LicenseInfo.DriverInfo.PersonID : -1;

            cntrlShowLocalLicenseInfo licenseCard = new cntrlShowLocalLicenseInfo(_LicenseID, personID);
            licenseCard.Dock = DockStyle.Fill;
            pnlLicenseInfo.Controls.Add(licenseCard);
        }

        private void cntrlLicenseCardWithFilters_Load(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                txtFilter.Focus();
            });

            if (_Mode == enMode.Update)
            {
                txtFilter.Text = _LicenseID.ToString();
                groupBox1.Enabled = false;
                _FindLicense();
                _LoadData(LicenseInfo);
            }
            else
            {
                _LoadData(null);
            }
        }

        void _FindLicense()
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
                return;

            LicenseInfo = clsLicense._FindByLicenseID(int.Parse(txtFilter.Text));

            if (LicenseInfo != null)
            {
                _LicenseID = LicenseInfo.LicenseID;
                _LoadData(LicenseInfo);
                LicenseSelected?.Invoke(_LicenseID);
            }
            else
            {
                MessageBox.Show("License not found!", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                _FindLicense();
            }
        }

        private void pbSearchLicense_Click(object sender, EventArgs e)
        {
            _FindLicense();
        }
    }
}