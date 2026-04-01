using DVLD.Properties;
using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD
{
    public partial class frmScheduleTest : Form
    {
        int _LocalLicenseID;
        int _TestTypeID;
        cntrlApplicationDetails appInfo;

        public frmScheduleTest(int locallicenseID, int testtypeID)
        {
            InitializeComponent();
            _LocalLicenseID = locallicenseID;
            _TestTypeID = testtypeID;
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

        public void _RefreshAppointmentsList()
        {
            dgvAppointments.DataSource = clsTestAppointment._GetPersonTestAppointmentsByTestTypeID(_LocalLicenseID, _TestTypeID);
            lblRecords.Text = "Records : " + dgvAppointments.RowCount.ToString();

            appInfo._UpdateControlData();

            lblNoAppointmentsYet.Visible = (dgvAppointments.Rows.Count == 0);
        }

        private void CustomizeUI()
        {
            if (dgvAppointments.Rows.Count > 0)
            {
                //Context Menu Strip
                cmsAppointments.ForeColor = Color.White;
                cmsAppointments.BackColor = Color.FromArgb(28, 15, 69);
                cmsAppointments.Renderer = new PurpleMenuRenderer();

                // Data Grid View Style
                dgvAppointments.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11);
                dgvAppointments.BackgroundColor = Color.Black;
                dgvAppointments.BorderStyle = BorderStyle.None;
                dgvAppointments.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvAppointments.GridColor = Color.FromArgb(45, 45, 48);
                dgvAppointments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvAppointments.RowHeadersVisible = false;
                dgvAppointments.EnableHeadersVisualStyles = false;

                dgvAppointments.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dgvAppointments.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 15, 69);
                dgvAppointments.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvAppointments.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 12, FontStyle.Italic);
                dgvAppointments.ColumnHeadersHeight = 45;

                dgvAppointments.DefaultCellStyle.BackColor = Color.FromArgb(20, 20, 20);
                dgvAppointments.DefaultCellStyle.ForeColor = Color.FromArgb(224, 224, 224);
                dgvAppointments.DefaultCellStyle.SelectionBackColor = Color.FromArgb(38, 2, 184);
                dgvAppointments.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvAppointments.RowTemplate.Height = 35;

                dgvAppointments.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(15, 15, 15);

                //Data Grid View Rows

                dgvAppointments.Columns["TestAppointmentID"].HeaderText = "Appointment ID";
                dgvAppointments.Columns["AppointmentDate"].HeaderText = "Appointment Date";
                dgvAppointments.Columns["PaidFees"].HeaderText = "Paid Fees";
                dgvAppointments.Columns["IsLocked"].HeaderText = "Is Locked";
            }
        }

        void _LoadTestTitle()
        {
            if (_TestTypeID == 1)
                lblTestTitle.Text = "Vision Test Appointments";
            else if (_TestTypeID == 2)
                lblTestTitle.Text = "Written Test Appointments";
            else
                lblTestTitle.Text = "Street Test Appointments";
        }

        void _LoadTestPicture()
        {
            if (_TestTypeID == 1)
                pbTestPhoto.Image = Resources.vision_test;
            else if (_TestTypeID == 2)
                pbTestPhoto.Image = Resources.written_test;
            else
                pbTestPhoto.Image = Resources.street_test;
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            _LoadTestPicture();
            _LoadTestTitle();

            lblNoAppointmentsYet.Visible = false;

            appInfo = new cntrlApplicationDetails(_LocalLicenseID);
            appInfo.Dock = DockStyle.Fill;
            pnlApplicationInfo.Controls.Add(appInfo);

            _RefreshAppointmentsList();
            CustomizeUI();
        }

        private void pbAddAppointment_Click(object sender, EventArgs e)
        {
            if (clsTestAppointment._IsAppointmentActive(_LocalLicenseID, _TestTypeID))
            {
                MessageBox.Show("Person already has an active appointment! Please settle the current one first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (clsTest._IsTestPassed(_LocalLicenseID, _TestTypeID))
            {
                MessageBox.Show("Person already passed this test!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmAdd_EditAppointment frm = new frmAdd_EditAppointment(-1, _LocalLicenseID, _TestTypeID);
            frm.ShowDialog();

            _RefreshAppointmentsList();
            CustomizeUI();
        }

        private void cmsAppointments_Opening(object sender, CancelEventArgs e)
        {
            int AppointmentID = (int)dgvAppointments.CurrentRow.Cells[0].Value;

            clsTestAppointment appointment = clsTestAppointment._FindByTestAppointmentID(AppointmentID);

            cmsTakeTest.Enabled = !appointment.IsLocked;
        }

        private void cmsEdit_Click(object sender, EventArgs e)
        {
            Form frm = new frmAdd_EditAppointment((int)dgvAppointments.CurrentRow.Cells[0].Value, _LocalLicenseID, _TestTypeID);
            frm.ShowDialog();
            _RefreshAppointmentsList();
        }

        private void cmsTakeTest_Click(object sender, EventArgs e)
        {
            Form frm = new frmTakeTest((int)dgvAppointments.CurrentRow.Cells[0].Value, _LocalLicenseID, _TestTypeID);
            frm.ShowDialog();
            _RefreshAppointmentsList();
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}