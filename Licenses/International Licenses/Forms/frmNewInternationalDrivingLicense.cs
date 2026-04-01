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
    public partial class frmNewInternationalDrivingLicense : Form
    {
        int _LicenseID;

        public frmNewInternationalDrivingLicense(int licenseID)
        {
            InitializeComponent();
            _LicenseID = licenseID;
        }

        private void frmNewInternationalDrivingLicense_Load(object sender, EventArgs e)
        {
            cntrlDriverLicenseInfoWithFilters LicenseFilterCard = new cntrlDriverLicenseInfoWithFilters(_LicenseID);
            LicenseFilterCard.Dock = DockStyle.Fill;
            pnlDriverInfo.Controls.Add(LicenseFilterCard);

            cntrlInternationalApplicationDetails AppDetails = new cntrlInternationalApplicationDetails(LicenseFilterCard);
            AppDetails.Dock = DockStyle.Fill;
            pnlAppInfo.Controls.Add(AppDetails);
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}