using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DVLD
{
    public partial class cntrlUsers : UserControl
    {
        public cntrlUsers()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public class PurpleMenuRenderer : ToolStripProfessionalRenderer
        {
            public PurpleMenuRenderer() : base(new PurpleColorTable()) { }
        }

        public class PurpleColorTable : ProfessionalColorTable
        {
            // The "Hover" color - a lighter, elegant purple
            public override Color MenuItemSelected => Color.FromArgb(50, 30, 100);

            // The border around the hovered item
            public override Color MenuItemBorder => Color.FromArgb(120, 90, 180);

            // Selection colors for the "Press" state
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(65, 45, 130);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(65, 45, 130);
        }

        public void _RefreshUsersList()
        {
            dgvUsers.DataSource = clsUser._GetAllUsers();
            lblRecords.Text = "Records : " + dgvUsers.RowCount.ToString();
        }

        private void CustomizeUI()
        {
            if (dgvUsers.Rows.Count > 0)
            {
                //Context Menu Strip
                cmsUsers.ForeColor = Color.White;
                cmsUsers.BackColor = Color.FromArgb(28, 15, 69);
                cmsUsers.Renderer = new PurpleMenuRenderer();

                // Data Grid View Style
                dgvUsers.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11);
                dgvUsers.BackgroundColor = Color.Black;
                dgvUsers.BorderStyle = BorderStyle.None;
                dgvUsers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvUsers.GridColor = Color.FromArgb(45, 45, 48);
                dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvUsers.RowHeadersVisible = false;
                dgvUsers.EnableHeadersVisualStyles = false;

                dgvUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 15, 69);
                dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 12, FontStyle.Italic);
                dgvUsers.ColumnHeadersHeight = 45;

                dgvUsers.DefaultCellStyle.BackColor = Color.FromArgb(20, 20, 20);
                dgvUsers.DefaultCellStyle.ForeColor = Color.FromArgb(224, 224, 224);
                dgvUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(38, 2, 184);
                dgvUsers.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvUsers.RowTemplate.Height = 35;

                dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(15, 15, 15);

                //Data Grid View Rows
                dgvUsers.Columns["UserID"].Width = 50;
                dgvUsers.Columns["PersonID"].Width = 50;
                dgvUsers.Columns["FullName"].Width = 150;
                dgvUsers.Columns["Username"].Width = 90;
                dgvUsers.Columns["IsActive"].Width = 250;

                dgvUsers.Columns["UserID"].HeaderText = "User ID";
                dgvUsers.Columns["PersonID"].HeaderText = "Person ID";
                dgvUsers.Columns["FullName"].HeaderText = "Full Name";
                dgvUsers.Columns["IsActive"].HeaderText = "Active Status";
            }
        }

        private void cntrlUsers_Load(object sender, EventArgs e)
        {
            cbFilters.SelectedIndex = 0;
            txtFilter.Text = "";
            _RefreshUsersList();
            CustomizeUI();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            DataTable dtUsers = (DataTable)dgvUsers.DataSource;

            if (dtUsers == null)
                return;

            if (string.IsNullOrWhiteSpace(txtFilter.Text) || cbFilters.SelectedIndex == 0)
            {
                dtUsers.DefaultView.RowFilter = "";
                lblRecords.Text = "Records : " + dgvUsers.RowCount.ToString();
                return;
            }

            string columnName = "";

            switch (cbFilters.SelectedIndex)
            {
                case 1: columnName = "UserID"; break;
                case 2: columnName = "PersonID"; break;
                case 3: columnName = "FullName"; break;
                case 4: columnName = "UserName"; break;
                case 5: columnName = "IsActive"; break;
                default: columnName = "None"; break;
            }

            string filterValue = txtFilter.Text.Trim().Replace("'", "''");

            if (columnName == "UserID" || columnName == "PersonID")
            {
                dtUsers.DefaultView.RowFilter = string.Format("Convert([{0}], 'System.String') LIKE '{1}%'", columnName, filterValue);
            }
            else
            {
                dtUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", columnName, filterValue);
            }

            lblRecords.Text = "Records : " + dgvUsers.RowCount.ToString();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilters.SelectedIndex == 1 || cbFilters.SelectedIndex == 2)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void cbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Text = "";
            _RefreshUsersList();
        }

        private void AddUser_Click(object sender, EventArgs e)
        {
            Form frm = new frmAdd_EditUser(-1);
            frm.ShowDialog();
            _RefreshUsersList();
        }

        private void cmsShowDetails_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowUserDetails((int)dgvUsers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
            _RefreshUsersList();
        }

        private void cmsEdit_Click(object sender, EventArgs e)
        {
            Form frm = new frmAdd_EditUser((int)dgvUsers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
            _RefreshUsersList();
        }

        private void cmsDelete_Click(object sender, EventArgs e)
        {
            int userID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            int personID = (int)dgvUsers.CurrentRow.Cells[1].Value;

            clsDriver driver = clsDriver._GetDriverInfoByPersonID(personID);
            clsApplication app = clsApplication._FindApplicationByPersonID(personID);
            clsLicense license = clsLicense._FindByUserID(userID);
            clsTest test = clsTest._FindByUserID(userID);
            clsTestAppointment testAppointment = clsTestAppointment._FindByUserID(userID);
            // international still not implemented

            if (userID == clsGlobalSettings.CurrentUser.UserID)
            {
                MessageBox.Show("Cannot delete current user!",
                    "Deletion Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (driver != null || app != null || license != null || test != null || testAppointment != null)
            {
                MessageBox.Show("Cannot delete user with ID [" + dgvUsers.CurrentRow.Cells[0].Value.ToString() + "] as they have data linked to it!",
                    "Deletion Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else if (MessageBox.Show("Are you sure you want to delete [" + dgvUsers.CurrentRow.Cells[0].Value.ToString()
                + "] permanently?", "Confirm Deletion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (clsUser._DeleteUser((int)dgvUsers.CurrentRow.Cells[0].Value))
                {
                    _RefreshUsersList();
                }
            }
        }

        private void cmsChangePassword_Click(object sender, EventArgs e)
        {
            Form frm = new frmChangePassword((int)dgvUsers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
        }

        private void cmsSendEmail_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet, will be coming soon!");
        }

        private void cmsPhoneCall_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet, will be coming soon!");
        }
    }
}
