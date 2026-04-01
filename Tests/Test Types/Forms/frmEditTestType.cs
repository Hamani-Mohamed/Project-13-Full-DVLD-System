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
    public partial class frmEditTestType : Form
    {
        int _TestTypeID;
        clsTestType NewType;

        public frmEditTestType(int testtypeID)
        {
            InitializeComponent();
            _TestTypeID = testtypeID;
        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            NewType = clsTestType._GetTestInfoByID(_TestTypeID);
            lblID.Text = "ID :             " + _TestTypeID;
            txtTitle.Text = NewType.TestTypeTitle;
            txtFees.Text = NewType.TestTypeFees.ToString();
            txtDescription.Text = NewType.TestTypeDescription;
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

        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                errorProvider1.SetError(txtDescription, "A description is required!");
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
            return !(string.IsNullOrWhiteSpace(txtTitle.Text)) && !(string.IsNullOrWhiteSpace(txtFees.Text))
                && !(string.IsNullOrWhiteSpace(txtDescription.Text));
        }

        private void lblSave_Click(object sender, EventArgs e)
        {
            if (!IsDataValid())
            {
                MessageBox.Show("Please fill the required fields with correct info!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            NewType.TestTypeTitle = txtTitle.Text.Trim();
            NewType.TestTypeFees = Convert.ToDecimal(txtFees.Text.Trim());
            NewType.TestTypeDescription = txtDescription.Text.Trim();

            if (NewType.UpdateTestType())
            {
                errorProvider1.Clear();
                MessageBox.Show("New Data Saved Successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTitle.Enabled = false;
                txtFees.Enabled = false;
                txtDescription.Enabled = false;
                lblSave.Enabled = false;
            }
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}