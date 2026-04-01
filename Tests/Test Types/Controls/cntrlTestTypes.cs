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

namespace DVLD.Applications.Test_Types.Controls
{
    public partial class cntrlTestTypes : UserControl
    {
        public cntrlTestTypes()
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

        public void _RefreshTestTypesList()
        {
            dgvTestTypes.DataSource = clsTestType._GetAllTestTypes();
            lblRecords.Text = "Records : " + dgvTestTypes.RowCount.ToString();
        }

        private void CustomizeUI()
        {
            if (dgvTestTypes.Rows.Count > 0)
            {
                //Context Menu Strip
                cmsApplicationTypes.ForeColor = Color.White;
                cmsApplicationTypes.BackColor = Color.FromArgb(28, 15, 69);
                cmsApplicationTypes.Renderer = new PurpleMenuRenderer();

                // Data Grid View Style
                dgvTestTypes.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11);
                dgvTestTypes.BackgroundColor = Color.Black;
                dgvTestTypes.BorderStyle = BorderStyle.None;
                dgvTestTypes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvTestTypes.GridColor = Color.FromArgb(45, 45, 48);
                dgvTestTypes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvTestTypes.RowHeadersVisible = false;
                dgvTestTypes.EnableHeadersVisualStyles = false;

                dgvTestTypes.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dgvTestTypes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 15, 69);
                dgvTestTypes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvTestTypes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 12, FontStyle.Italic);
                dgvTestTypes.ColumnHeadersHeight = 45;

                dgvTestTypes.DefaultCellStyle.BackColor = Color.FromArgb(20, 20, 20);
                dgvTestTypes.DefaultCellStyle.ForeColor = Color.FromArgb(224, 224, 224);
                dgvTestTypes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(38, 2, 184);
                dgvTestTypes.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvTestTypes.RowTemplate.Height = 35;

                dgvTestTypes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(15, 15, 15);

                dgvTestTypes.Columns["TestTypeID"].HeaderText = "Test Type ID";
                dgvTestTypes.Columns["TestTypeTitle"].HeaderText = "Test Type Title";
                dgvTestTypes.Columns["TestTypeDescription"].HeaderText = "Test Type Description";
                dgvTestTypes.Columns["TestTypeFees"].HeaderText = "Test Type Fees";

                dgvTestTypes.Columns["TestTypeDescription"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvTestTypes.Columns["TestTypeID"].Width = 100;
                dgvTestTypes.Columns["TestTypeTitle"].Width = 130;
                dgvTestTypes.Columns["TestTypeFees"].Width = 140;
            }
        }

        private void cntrlTestTypes_Load(object sender, EventArgs e)
        {
            _RefreshTestTypesList();
            CustomizeUI();
        }

        private void cmsEdit_Click(object sender, EventArgs e)
        {
            Form frm = new frmEditTestType((int)dgvTestTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshTestTypesList();
        }
    }
}