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
    public partial class frmShowPersonDetails : Form
    {
        private int _PersonID;

        public frmShowPersonDetails(int personID)
        {
            InitializeComponent();
            _PersonID = personID;
        }

        private void frmShowPersonDetails_Load(object sender, EventArgs e)
        {
            Control personCard = new cntrlPersonCard(_PersonID);
            personCard.Dock = DockStyle.Fill;
            pnlPerson.Controls.Add(personCard);
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}