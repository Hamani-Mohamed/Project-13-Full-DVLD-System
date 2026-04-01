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
using System.IO;

namespace DVLD
{
    public partial class cntrlDrivers : UserControl
    {
        public cntrlDrivers()
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

        public void _RefreshDriversList()
        {
            dgvDrivers.DataSource = clsDriver._GetAllDrivers();
            lblRecords.Text = "Records : " + dgvDrivers.RowCount.ToString();
        }

        private void CustomizeUI()
        {
            if (dgvDrivers.Rows.Count > 0)
            {
                //Context Menu Strip
                cmsDrivers.ForeColor = Color.White;
                cmsDrivers.BackColor = Color.FromArgb(28, 15, 69);
                cmsDrivers.Renderer = new PurpleMenuRenderer();

                // Data Grid View Style
                dgvDrivers.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11);
                dgvDrivers.BackgroundColor = Color.Black;
                dgvDrivers.BorderStyle = BorderStyle.None;
                dgvDrivers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvDrivers.GridColor = Color.FromArgb(45, 45, 48);
                dgvDrivers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvDrivers.RowHeadersVisible = false;
                dgvDrivers.EnableHeadersVisualStyles = false;

                dgvDrivers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dgvDrivers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 15, 69);
                dgvDrivers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvDrivers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 12, FontStyle.Italic);
                dgvDrivers.ColumnHeadersHeight = 45;

                dgvDrivers.DefaultCellStyle.BackColor = Color.FromArgb(20, 20, 20);
                dgvDrivers.DefaultCellStyle.ForeColor = Color.FromArgb(224, 224, 224);
                dgvDrivers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(38, 2, 184);
                dgvDrivers.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvDrivers.RowTemplate.Height = 35;

                dgvDrivers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(15, 15, 15);

                //Data Grid View Rows
                dgvDrivers.Columns["DriverID"].Width = 70;
                dgvDrivers.Columns["PersonID"].Width = 70;
                dgvDrivers.Columns["NationalNo"].Width = 70;
                dgvDrivers.Columns["FullName"].Width = 150;
                dgvDrivers.Columns["CreatedDate"].Width = 70;
                dgvDrivers.Columns["NumberOfActiveLicenses"].Width = 250;

                dgvDrivers.Columns["DriverID"].HeaderText = "Driver ID";
                dgvDrivers.Columns["PersonID"].HeaderText = "Person ID";
                dgvDrivers.Columns["NationalNo"].HeaderText = "National No";
                dgvDrivers.Columns["FullName"].HeaderText = "Full Name";
                dgvDrivers.Columns["CreatedDate"].HeaderText = "Creation Date";
                dgvDrivers.Columns["NumberOfActiveLicenses"].HeaderText = "Number Of Active Licenses";
            }
        }

        private void cntrlDrivers_Load(object sender, EventArgs e)
        {
            cbFilters.SelectedIndex = 0;
            txtFilter.Text = "";
            _RefreshDriversList();
            CustomizeUI();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            DataTable dtDrivers = (DataTable)dgvDrivers.DataSource;

            if (dtDrivers == null)
                return;

            if (string.IsNullOrWhiteSpace(txtFilter.Text) || cbFilters.SelectedIndex == 0)
            {
                dtDrivers.DefaultView.RowFilter = "";
                lblRecords.Text = "Records : " + dgvDrivers.RowCount.ToString();
                return;
            }

            string columnName = "";

            switch (cbFilters.SelectedIndex)
            {
                case 1: columnName = "DriverID"; break;
                case 2: columnName = "PersonID"; break;
                case 3: columnName = "NationalNo"; break;
                case 4: columnName = "FullName"; break;
                case 5: columnName = "CreatedDate"; break;
                case 6: columnName = "NumberOfActiveLicenses"; break;
                default: columnName = "None"; break;
            }

            string filterValue = txtFilter.Text.Trim().Replace("'", "''");

            if (columnName == "NationalNo" || columnName == "FullName")
            {
                dtDrivers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", columnName, filterValue);
            }
            else
            {
                dtDrivers.DefaultView.RowFilter = string.Format("Convert([{0}], 'System.String') LIKE '{1}%'", columnName, filterValue);
            }

            lblRecords.Text = "Records : " + dgvDrivers.RowCount.ToString();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilters.SelectedIndex == 0)
            {
                e.Handled = true;
                return;
            }

            if (cbFilters.SelectedIndex == 1 || cbFilters.SelectedIndex == 2 || cbFilters.SelectedIndex == 6)
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
            _RefreshDriversList();
        }

        private void cmsShowPersonDetails_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonDetails((int)dgvDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
            _RefreshDriversList();
        }

        private void cmsShowPersonLicenseHistory_Click(object sender, EventArgs e)
        {
            clsDriver driver = clsDriver._GetDriverInfoByDriverID((int)dgvDrivers.CurrentRow.Cells[0].Value);

            Form frm = new frmShowLicenseHistory(driver.PersonID, driver.DriverID, -1);
            frm.ShowDialog();
        }
    }
}