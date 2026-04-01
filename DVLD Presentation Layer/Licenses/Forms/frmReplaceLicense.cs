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
    public partial class frmReplaceLicense : Form
    {
        int _LicenseID;

        public frmReplaceLicense(int licenseID)
        {
            InitializeComponent();
            _LicenseID = licenseID;
        }

        private void frmReplaceLicense_Load(object sender, EventArgs e)
        {
            cntrlDriverLicenseInfoWithFilters LicenseFilterCard = new cntrlDriverLicenseInfoWithFilters(_LicenseID);
            LicenseFilterCard.Dock = DockStyle.Fill;
            pnlDriverInfo.Controls.Add(LicenseFilterCard);

            cntrlReplaceLicense RenewAppDetails = new cntrlReplaceLicense(LicenseFilterCard);
            RenewAppDetails.Dock = DockStyle.Fill;
            pnlAppInfo.Controls.Add(RenewAppDetails);
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}