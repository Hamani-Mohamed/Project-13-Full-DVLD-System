using DVLD.Properties;
using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD
{
    public partial class frmShowLocalLicenseInfo : Form
    {
        int _LicenseID;
        int _PersonID;

        public frmShowLocalLicenseInfo(int licenseID, int personID)
        {
            InitializeComponent();
            _LicenseID = licenseID;
            _PersonID = personID;
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            Control showLicense = new cntrlShowLocalLicenseInfo(_LicenseID, _PersonID);
            showLicense.Dock = DockStyle.Fill;
            pnlApplicationInfo.Controls.Add(showLicense);
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}