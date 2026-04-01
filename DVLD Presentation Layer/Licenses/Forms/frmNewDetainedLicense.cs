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
    public partial class frmNewDetainedLicense : Form
    {
        int _LicenseID;

        public frmNewDetainedLicense(int licenseID)
        {
            InitializeComponent();
            _LicenseID = licenseID;
        }

        private void frmNewInternationalDrivingLicense_Load(object sender, EventArgs e)
        {
            cntrlDriverLicenseInfoWithFilters LicenseFilterCard = new cntrlDriverLicenseInfoWithFilters(_LicenseID);
            LicenseFilterCard.Dock = DockStyle.Fill;
            pnlLicenseInfo.Controls.Add(LicenseFilterCard);

            cntrlDetainedLicenseDetails DetainDetails = new cntrlDetainedLicenseDetails(LicenseFilterCard);
            DetainDetails.Dock = DockStyle.Fill;
            pnlDetainInfo.Controls.Add(DetainDetails);
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}