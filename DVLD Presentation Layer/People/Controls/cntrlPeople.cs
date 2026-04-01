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
    public partial class cntrlPeople : UserControl
    {
        public cntrlPeople()
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

        public void _RefreshPeopleList()
        {
            dgvPeople.DataSource = clsPerson._GetAllPeople();
            lblRecords.Text = "Records : " + dgvPeople.RowCount.ToString();
        }

        private void CustomizeUI()
        {
            if (dgvPeople.Rows.Count > 0)
            {
                //Context Menu Strip
                cmsPeople.ForeColor = Color.White;
                cmsPeople.BackColor = Color.FromArgb(28, 15, 69);
                cmsPeople.Renderer = new PurpleMenuRenderer();

                // Data Grid View Style
                dgvPeople.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 11);
                dgvPeople.BackgroundColor = Color.Black;
                dgvPeople.BorderStyle = BorderStyle.None;
                dgvPeople.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgvPeople.GridColor = Color.FromArgb(45, 45, 48);
                dgvPeople.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvPeople.RowHeadersVisible = false;
                dgvPeople.EnableHeadersVisualStyles = false;

                dgvPeople.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                dgvPeople.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 15, 69);
                dgvPeople.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvPeople.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 12, FontStyle.Italic);
                dgvPeople.ColumnHeadersHeight = 45;

                dgvPeople.DefaultCellStyle.BackColor = Color.FromArgb(20, 20, 20);
                dgvPeople.DefaultCellStyle.ForeColor = Color.FromArgb(224, 224, 224);
                dgvPeople.DefaultCellStyle.SelectionBackColor = Color.FromArgb(38, 2, 184);
                dgvPeople.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvPeople.RowTemplate.Height = 35;

                dgvPeople.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(15, 15, 15);

                //Data Grid View Rows
                dgvPeople.Columns["PersonID"].Width = 90;
                dgvPeople.Columns["NationalNo"].Width = 100;
                dgvPeople.Columns["FirstName"].Width = 110;
                dgvPeople.Columns["SecondName"].Width = 110;
                dgvPeople.Columns["ThirdName"].Width = 110;
                dgvPeople.Columns["LastName"].Width = 110;
                dgvPeople.Columns["Gender"].Width = 90;
                dgvPeople.Columns["DateOfBirth"].Width = 150;
                dgvPeople.Columns["Country"].Width = 150;
                dgvPeople.Columns["Phone"].Width = 90;
                dgvPeople.Columns["Email"].Width = 300;

                dgvPeople.Columns["PersonID"].HeaderText = "Person ID";
                dgvPeople.Columns["NationalNo"].HeaderText = "National No";
                dgvPeople.Columns["FirstName"].HeaderText = "First Name";
                dgvPeople.Columns["SecondName"].HeaderText = "Second Name";
                dgvPeople.Columns["ThirdName"].HeaderText = "Third Name";
                dgvPeople.Columns["LastName"].HeaderText = "Last Name";
                dgvPeople.Columns["DateOfBirth"].HeaderText = "Date Of Birth";
            }
        }

        private void cntrlPeople_Load(object sender, EventArgs e)
        {
            cbFilters.SelectedIndex = 0;
            txtFilter.Text = "";
            _RefreshPeopleList();
            CustomizeUI();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            DataTable dtPeople = (DataTable)dgvPeople.DataSource;

            if (dtPeople == null)
                return;

            if (string.IsNullOrWhiteSpace(txtFilter.Text) || cbFilters.SelectedIndex == 0)
            {
                dtPeople.DefaultView.RowFilter = "";
                lblRecords.Text = "Records : " + dgvPeople.RowCount.ToString();
                return;
            }

            string columnName = "";

            switch (cbFilters.SelectedIndex)
            {
                case 1: columnName = "PersonID"; break;
                case 2: columnName = "NationalNo"; break;
                case 3: columnName = "FirstName"; break;
                case 4: columnName = "SecondName"; break;
                case 5: columnName = "ThirdName"; break;
                case 6: columnName = "LastName"; break;
                case 7: columnName = "Gender"; break;
                case 8: columnName = "DateOfBirth"; break;
                case 9: columnName = "Country"; break;
                case 10: columnName = "Phone"; break;
                case 11: columnName = "Email"; break;
                default: columnName = "None"; break;
            }

            string filterValue = txtFilter.Text.Trim().Replace("'", "''");

            if (columnName == "PersonID" || columnName == "DateOfBirth")
            {
                dtPeople.DefaultView.RowFilter = string.Format("Convert([{0}], 'System.String') LIKE '{1}%'", columnName, filterValue);
            }
            else
            {
                dtPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", columnName, filterValue);
            }

            lblRecords.Text = "Records : " + dgvPeople.RowCount.ToString();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilters.SelectedIndex == 0)
            {
                e.Handled = true;
                return;
            }

            if (cbFilters.SelectedIndex == 1 || cbFilters.SelectedIndex == 10)
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
            _RefreshPeopleList();
        }

        private void AddPerson_Click(object sender, EventArgs e)
        {
            Form frm = new frmAdd_EditPerson(-1);
            frm.ShowDialog();
            _RefreshPeopleList();
        }

        private void cmsShowPersonDetails_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonDetails((int)dgvPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshPeopleList();
        }

        private void cmsEdit_Click(object sender, EventArgs e)
        {
            Form frm = new frmAdd_EditPerson((int)dgvPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshPeopleList();
        }

        private void cmsDelete_Click(object sender, EventArgs e)
        {
            clsDriver driver = clsDriver._GetDriverInfoByPersonID((int)dgvPeople.CurrentRow.Cells[0].Value);
            clsUser user = clsUser.FindUserByPersonID((int)dgvPeople.CurrentRow.Cells[0].Value);
            clsApplication app = clsApplication._FindApplicationByPersonID((int)dgvPeople.CurrentRow.Cells[0].Value);

            if (driver != null || user != null || app != null)
            {
                MessageBox.Show("Cannot delete person with ID [" + dgvPeople.CurrentRow.Cells[0].Value.ToString() + "] as they have data linked to it!",
                    "Deletion Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete [" + dgvPeople.CurrentRow.Cells[0].Value.ToString()
                + "] permanently?", "Confirm Deletion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                clsPerson Person = clsPerson._FindPersonByID((int)dgvPeople.CurrentRow.Cells[0].Value);

                if (clsPerson._DeletePerson(Person.PersonID))
                {

                    if (File.Exists(Person.ImagePath))
                    {
                        try
                        {
                            File.Delete(Person.ImagePath);
                            Person.ImagePath = null;
                        }
                        catch
                        {

                        }
                    }
                    _RefreshPeopleList();
                }
            }
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