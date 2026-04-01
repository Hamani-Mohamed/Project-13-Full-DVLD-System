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
    public partial class frmAdd_EditUser : Form
    {
        int _PersonID;
        public enum enMode { AddNew, Update };
        enMode _Mode;

        public frmAdd_EditUser(int personID)
        {
            InitializeComponent();
            _PersonID = personID;
            if (_PersonID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
        }

        private void frmAdd_EditUser_Load(object sender, EventArgs e)
        {
            Control AddEditUser = new cntrlAdd_EditUser(_PersonID);
            AddEditUser.Dock = DockStyle.Fill;
            pnlPersonSearch.Controls.Add(AddEditUser);

            if (_Mode == enMode.Update)
            {
                lblAddNewUser.Text = "Edit User";
                lblAddNewUser.Location = new Point(450, lblAddNewUser.Location.Y);
            }
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}