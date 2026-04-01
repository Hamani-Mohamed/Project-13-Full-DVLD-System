using DVLDBusinessLayer;
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
    public partial class cntrlAdd_EditUser : UserControl
    {
        int _PersonID;
        public enum enMode { AddNew, Update };
        enMode _Mode;

        public cntrlAdd_EditUser(int personID)
        {
            InitializeComponent();
            _PersonID = personID;
            if (_PersonID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
        }

        private void cntrlAdd_EditUser_Load(object sender, EventArgs e)
        {
            cntrlPersonCardWithFilters UserFilterCard = new cntrlPersonCardWithFilters(_PersonID);

            UserFilterCard.PersonSelected += (SelectedPersonID) =>
            {
                _PersonID = SelectedPersonID;
            };

            UserFilterCard.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(UserFilterCard);
        }

        private void lblNext_Click(object sender, EventArgs e)
        {
            if (lblNext.Text == "Next")
            {
                if (_PersonID != -1)
                {
                    if (_Mode == enMode.AddNew && clsUser._DoesUserExistByPersonID(_PersonID))
                    {
                        MessageBox.Show("Selected person is already a user!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    pnlContent.Controls[0].Visible = false;

                    if (pnlContent.Controls.Count < 2)
                    {
                        Control UserLoginInfo = new cntrlUserLoginInfo(_PersonID);
                        UserLoginInfo.Dock = DockStyle.Fill;
                        pnlContent.Controls.Add(UserLoginInfo);
                    }
                    else
                    {
                        pnlContent.Controls[1].Visible = true;
                    }
                    lblNext.Text = "Back";
                }

                else
                    MessageBox.Show("Select a person first!", "Person Not Selected!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            else
            {
                pnlContent.Controls[0].Visible = true;
                pnlContent.Controls.RemoveAt(1);
                lblNext.Text = "Next";
            }
        }
    }
}