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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace DVLD
{
    public partial class cntrlPersonCardWithFilters : UserControl
    {
        int _PersonID;
        public enum enMode { AddNew, Update };
        enMode _Mode;
        public event Action<int> PersonSelected;

        protected virtual void OnPersonSelected(int PersonID)
        {
            Action<int> handler = PersonSelected;
            if (handler != null)
            {
                handler(PersonID);
            }
        }

        public cntrlPersonCardWithFilters(int personID)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            _PersonID = personID;

            if (_PersonID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
        }

        void _LoadData()
        {
            pnlPersonInfo.Controls.Clear();
            cntrlPersonCard personCard = new cntrlPersonCard(_PersonID);
            personCard.Dock = DockStyle.Fill;
            pnlPersonInfo.Controls.Add(personCard);
        }

        private void cntrlPersonCardWithFilters_Load(object sender, EventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                txtFilter.Focus();
            });

            if (_Mode == enMode.Update)
                groupBox1.Enabled = false;
            _LoadData();
            cbFilters.SelectedIndex = 0;
        }

        private void cbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Text = "";
            txtFilter.Focus();
        }

        void _FindPerson()
        {
            if (cbFilters.SelectedIndex == 0 || string.IsNullOrEmpty(txtFilter.Text))
                return;

            clsPerson PersonFound;

            if (cbFilters.SelectedIndex == 1)
                PersonFound = clsPerson._FindPersonByID(int.Parse(txtFilter.Text));

            else
                PersonFound = clsPerson._FindPersonByNationalNo(txtFilter.Text);

            if (PersonFound != null)
            {
                _PersonID = PersonFound.PersonID;
                _LoadData();
                PersonSelected?.Invoke(_PersonID);
            }
            else
            {
                MessageBox.Show("Person not found!", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilters.SelectedIndex == 0)
            {
                e.Handled = true;
                return;
            }

            if (cbFilters.SelectedIndex == 1)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }

            if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                _FindPerson();
            }
        }

        private void pbSearchPerson_Click(object sender, EventArgs e)
        {
            _FindPerson();
        }

        private void pbAddPerson_Click(object sender, EventArgs e)
        {
            frmAdd_EditPerson frm = new frmAdd_EditPerson(-1);

            frm.DataBack += (senderForm, PersonID) =>
            {
                cbFilters.SelectedIndex = 1;
                txtFilter.Text = PersonID.ToString();

                _PersonID = PersonID;
                _LoadData();

                PersonSelected?.Invoke(_PersonID);
            };

            frm.ShowDialog();
        }
    }
}