using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLDBusinessLayer;

namespace DVLD
{
    public partial class cntrlUserCard : UserControl
    {
        int _PersonID;

        public cntrlUserCard(int personID)
        {
            InitializeComponent();
            _PersonID = personID;
            this.DoubleBuffered = true;
        }

        public void _LoadData()
        {
            cntrlPersonCard personCard = new cntrlPersonCard(_PersonID);
            personCard.Dock = DockStyle.Fill;
            pnlPersonInfo.Controls.Add(personCard);

            clsUser _User = clsUser.FindUserByPersonID(_PersonID);

            if (_User != null)
            {
                lblUserID.Text = "User ID = " + _User.UserID;
                lblUsername.Text = "Username : " + _User.Username;
                lblIsActive.Text = (_User.IsActive) ? "Is Active : Yes" : "Is Active : No";
            }
        }

        private void cntrlUserCard_Load(object sender, EventArgs e)
        {
            _LoadData();
        }
    }
}