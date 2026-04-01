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
    public partial class frmNewLocalDrivingLicenseApplication : Form
    {
        int _PersonID, _LocalApplicationID;
        public enum enMode { AddNew, Update };
        enMode _Mode;

        public frmNewLocalDrivingLicenseApplication(int personID, int localapplicationID)
        {
            InitializeComponent();
            _PersonID = personID;
            _LocalApplicationID = localapplicationID;

            if (_LocalApplicationID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
        }

        private void frmAdd_EditLocalLicense_Load(object sender, EventArgs e)
        {
            Control AddNewLicense = new cntrlNewLocalDrivingLicenseApplication(_PersonID, _LocalApplicationID);
            AddNewLicense.Dock = DockStyle.Fill;
            pnlPersonSearch.Controls.Add(AddNewLicense);

            if (_Mode == enMode.Update)
            {
                lblNewLocalDrivingLicenseApplication.Text = "Edit Local Driving License Application";
            }
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}