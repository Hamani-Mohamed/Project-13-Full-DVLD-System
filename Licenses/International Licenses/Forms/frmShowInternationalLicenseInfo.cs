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
    public partial class frmShowInternationalLicenseInfo : Form
    {
        private int _LicenseID;

        public frmShowInternationalLicenseInfo(int licenseID)
        {
            InitializeComponent();
            _LicenseID = licenseID;
        }

        private void frmShowPersonDetails_Load(object sender, EventArgs e)
        {
            Control licenseCard = new cntrlShowInternationalLicenseInfo(_LicenseID);
            licenseCard.Dock = DockStyle.Fill;
            pnlLicense.Controls.Add(licenseCard);
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}