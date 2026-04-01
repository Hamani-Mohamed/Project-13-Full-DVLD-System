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
    public partial class cntrlApplicationTypes : UserControl
    {
        public cntrlApplicationTypes()
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

        public void _RefreshApplicationTypesList()
        {
            dgvApplicationTypes.DataSource = clsApplicationType._GetAllApplicationTypes();
            lblRecords.Text = "Records : " + dgvApplicationTypes.RowCount.ToString();
        }

        private void CustomizeUI()
        {
            if (dgvApplicationTypes.Rows.Count > 0)
            {
                //Context Menu Strip
                cmsApplicationTypes.ForeColor = Color.White;
                cmsApplicationTypes.BackColor = Color.FromArgb(28, 15, 69);
                cmsApplicationTypes.Renderer = new PurpleMenuRenderer();

                // Data Grid View Style
                dgvApplicationTypes.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11);
                dgvApplicationTypes.BackgroundColor = Color.Black;
                dgvApplicationTypes.BorderStyle = BorderStyle.None;
                dgvApplicationTypes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvApplicationTypes.GridColor = Color.FromArgb(45, 45, 48);
                dgvApplicationTypes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvApplicationTypes.RowHeadersVisible = false;
                dgvApplicationTypes.EnableHeadersVisualStyles = false;

                dgvApplicationTypes.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dgvApplicationTypes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 15, 69);
                dgvApplicationTypes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvApplicationTypes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 12, FontStyle.Italic);
                dgvApplicationTypes.ColumnHeadersHeight = 45;

                dgvApplicationTypes.DefaultCellStyle.BackColor = Color.FromArgb(20, 20, 20);
                dgvApplicationTypes.DefaultCellStyle.ForeColor = Color.FromArgb(224, 224, 224);
                dgvApplicationTypes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(38, 2, 184);
                dgvApplicationTypes.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvApplicationTypes.RowTemplate.Height = 35;

                dgvApplicationTypes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(15, 15, 15);

                dgvApplicationTypes.Columns["ApplicationTypeID"].HeaderText = "Application Type ID";
                dgvApplicationTypes.Columns["ApplicationTypeTitle"].HeaderText = "Application Type Title";
                dgvApplicationTypes.Columns["ApplicationFees"].HeaderText = "Application Fees";
            }
        }

        private void cntrlApplicationTypes_Load(object sender, EventArgs e)
        {
            _RefreshApplicationTypesList();
            CustomizeUI();
        }

        private void cmsEdit_Click(object sender, EventArgs e)
        {
            Form frm = new frmEditApplicationType((int)dgvApplicationTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshApplicationTypesList();
        }
    }
}