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
    public partial class frmShowApplicationDetails : Form
    {
        private int _LocalLicenseID;

        public frmShowApplicationDetails(int locallicenseID)
        {
            InitializeComponent();
            _LocalLicenseID = locallicenseID;
        }

        private void frmShowApplicationDetails_Load(object sender, EventArgs e)
        {
            Control applicationInfo = new cntrlApplicationDetails(_LocalLicenseID);
            applicationInfo.Dock = DockStyle.Fill;
            pnlApplication.Controls.Add(applicationInfo);
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}