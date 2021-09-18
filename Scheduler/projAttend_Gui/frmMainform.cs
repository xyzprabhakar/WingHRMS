using projAttend_DLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projAttend_Gui
{
    public partial class frmMainform : Form
    {
        public clsCalculation ob = null;
        DataTable dt = null;
        public frmMainform()
        {
            InitializeComponent();
        }

        private void frmMainform_Load(object sender, EventArgs e)
        {
            Application.DoEvents();
            this.BeginInvoke(new MethodInvoker(LoadData));
        }
        private void LoadData()
        {
            lblStatus.Text = "Loading data..";
            Application.DoEvents();
            ob.Load_all_machine();
            lblStatus.Text = "done..";
            Application.DoEvents();
            ddlMachine.DataSource = ob.dt_Machine;
            ddlMachine.DisplayMember = "disptext";
            ddlMachine.ValueMember = "machine_id";
            lblStatus.Text = "Ready..";
            Application.DoEvents();
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "Loading data..";
                Application.DoEvents();
                int machineid = -1;
                Int32.TryParse(Convert.ToString(ddlMachine.SelectedValue), out machineid);
                dt = ob.get_all_machine_data(Convert.ToDateTime(dtpFromTime.Value.ToString("dd-MMM-yyyy 00:00:00")),
                     Convert.ToDateTime(dtpToTime.Value.AddDays(1).ToString("dd-MMM-yyyy 00:00:00")), machineid);
                dgvGridView.DataSource = dt;
                lblStatus.Text = "done..";
                MessageBox.Show("Load Successfully !!!");
                Application.DoEvents();
                dgvGridView.AutoResizeColumns();
                lblStatus.Text = "Done. Total Rows=" + dgvGridView.Rows.Count;
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnPushData_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dt == null)
                {
                    MessageBox.Show("no data for save");
                    return;
                }

                lblStatus.Text = "Loading data..";
                Application.DoEvents();
                ob.save_data(this.dt);
                lblStatus.Text = "done..";
                MessageBox.Show("Save !!!");
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select File";
            openFileDialog1.InitialDirectory = @"D:\";
            openFileDialog1.Filter = "Excel Files|*.dat;*.txt;";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                lblFile.Text = "" + openFileDialog1.FileName;
                ReaddatafromFile();
            }
        }
        void ReaddatafromFile()
        {

            if (lblFile.Text.Trim().Length < 4)
            {
                MessageBox.Show("Please select a valid file");
            }
            else
            {
                lblStatus.Text = "Loading..";
                this.dt=ob.ReadFromFile(lblFile.Text, dtpFromTime.Value, dtpToTime.Value,Convert.ToInt32( ddlMachine.SelectedValue));
                dgvGridView.DataSource = dt;
                dgvGridView.AutoResizeColumns();
                lblStatus.Text = "Done. Total Rows=" + dgvGridView.Rows.Count;
            }
        }

        private void btnExcelExport_Click(object sender, EventArgs e)
        {
            
            _ExcelExport ex = new _ExcelExport();
            ex.ExcelExportFromDGV(dgvGridView, lblStatus);
        }

        private void frmMainform_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
