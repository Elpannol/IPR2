﻿using System;
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
    public partial class NewTest : Form
    {
        public NewTest()
        {
            InitializeComponent();
            FormClosing += NewTest_FormClosing;
            update();
        }

        private void NewTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void update()
        {
            weerstandLabel.Text = "50"      + " Watt";
            hartslagLabel.Text  = "120"     + " BPM";
            rondesLabel.Text    = "100"     + " RPM";
            tijdLabel.Text      = "01:30"   + " Minuten";
        }
    }
}
