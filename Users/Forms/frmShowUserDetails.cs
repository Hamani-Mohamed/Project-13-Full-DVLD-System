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
    public partial class frmShowUserDetails : Form
    {
        private int _PersonID;

        public frmShowUserDetails(int personID)
        {
            InitializeComponent();
            _PersonID = personID;
        }

        private void frmShowUserDetails_Load(object sender, EventArgs e)
        {
            Control userCard = new cntrlUserCard(_PersonID);
            userCard.Dock = DockStyle.Fill;
            pnlPerson.Controls.Add(userCard);
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}