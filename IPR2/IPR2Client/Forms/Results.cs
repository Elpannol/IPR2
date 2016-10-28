using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPR2Client.Forms
{
    public partial class Results : Form
    {
        public Results()
        {
            InitializeComponent();
        }

        private void newTestButton_Click(object sender, EventArgs e)
        {
            Visible = false;
            NewTest newTest = new NewTest();
            newTest.Visible = true;
        }
    }
}
