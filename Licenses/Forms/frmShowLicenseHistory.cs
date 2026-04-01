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
    public partial class frmShowLicenseHistory : Form
    {
        int _PersonID;
        int _DriverID;
        int _LicenseType;

        public frmShowLicenseHistory(int personID, int driverID, int licenseType = -1)
        {
            InitializeComponent();
            _PersonID = personID;
            _DriverID = driverID;
            _LicenseType = licenseType;
        }

        private void frmShowLicenseHistory_Load(object sender, EventArgs e)
        {
            Control personCard = new cntrlPersonCard(_PersonID);
            personCard.Dock = DockStyle.Fill;
            pnlPerson.Controls.Add(personCard);

            Control licenses = new cntrlDriverLicensesHistory(_DriverID, _LicenseType);
            licenses.Dock = DockStyle.Fill;
            pnlLicenses.Controls.Add(licenses);
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}