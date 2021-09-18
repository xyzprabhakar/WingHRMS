using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using projAttend_DLL;

namespace projAttend_Gui
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //clsCalculation ob = new clsCalculation();
            //ob.get_machine_data();
            //return;
            lblStatus.Text = "Please wait..";
            Application.DoEvents();
            clsCalculation ob = new clsCalculation();
            ob.username = txtUserName.Text;
            ob.password = txtPassword.Text;
            ob.login_set_tokken();
            if (clsCalculation.is_login_success == 1)
            {
                frmMainform _frmMainform = new frmMainform();
                this.Hide();
                _frmMainform.ob = ob;
                _frmMainform.Show();                

            }
            else
            {
                MessageBox.Show("Username or password Incorrect");
            }
            lblStatus.Text = "";
            Application.DoEvents();
        }

    
    }
}
