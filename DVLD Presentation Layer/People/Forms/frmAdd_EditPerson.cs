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
    public partial class frmAdd_EditPerson : Form
    {
        int _PersonID;
        public enum enMode { AddNew, Update };
        enMode _Mode;
        public delegate void DataBackHandler(object sender, int PersonID);
        public event DataBackHandler DataBack;

        public frmAdd_EditPerson(int personID)
        {
            InitializeComponent();
            _PersonID = personID;
            if (_PersonID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
        }

        private void frmAdd_EditPerson_Load(object sender, EventArgs e)
        {
            cntrlAdd_EditPerson AddEditPerson = new cntrlAdd_EditPerson(_PersonID);
            AddEditPerson.Dock = DockStyle.Fill;
            pnlPerson.Controls.Add(AddEditPerson);

            AddEditPerson.DataBack += (senderCtrl, PersonID) =>
            {
                DataBack?.Invoke(this, PersonID);
            };

            pnlPerson.Controls.Add(AddEditPerson);

            if (_Mode == enMode.Update)
            {
                lblAddNewPerson.Text = "Edit Person";
                lblAddNewPerson.Location = new Point(450, lblAddNewPerson.Location.Y);
            }
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}