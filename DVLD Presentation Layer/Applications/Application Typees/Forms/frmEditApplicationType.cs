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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD
{
    public partial class frmEditApplicationType : Form
    {
        int _ApplicationTypeID;
        clsApplicationType NewType;

        public frmEditApplicationType(int applicationtypeID)
        {
            InitializeComponent();
            _ApplicationTypeID = applicationtypeID;
        }

        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            NewType = clsApplicationType._GetApplicationInfoByID(_ApplicationTypeID);
            lblID.Text = "ID :             " + _ApplicationTypeID;
            txtTitle.Text = NewType.ApplicationTypeTitle;
            txtFees.Text = NewType.ApplicationFees.ToString();
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                errorProvider1.SetError(txtTitle, "A new title is required!");
            }
        }

        private void txtFee_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFees.Text))
            {
                errorProvider1.SetError(txtFees, "A new fee is required!");
            }
        }

        private void txtFee_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private bool IsDataValid()
        {
            return !(string.IsNullOrWhiteSpace(txtTitle.Text)) && !(string.IsNullOrWhiteSpace(txtFees.Text));
        }

        private void lblSave_Click(object sender, EventArgs e)
        {
            if (!IsDataValid())
            {
                MessageBox.Show("Please fill the required fields with correct info!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            NewType.ApplicationTypeTitle = txtTitle.Text.Trim();
            NewType.ApplicationFees = Convert.ToDecimal(txtFees.Text.Trim());

            if (NewType.UpdateApplicationType())
            {
                errorProvider1.Clear();
                MessageBox.Show("New Data Saved Successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTitle.Enabled = false;
                txtFees.Enabled = false;
                lblSave.Enabled = false;
            }
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}