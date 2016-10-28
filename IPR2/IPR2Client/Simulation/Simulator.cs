using IPR2Client.Simulation;
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
    public partial class Simulator : Form
    {
        private readonly Measurement Measurement;
        private int _time;

        public Simulator()
        {
            _time = 0;
            SimpleTime temp = new SimpleTime(_time / 60, _time % 60);
            Measurement = new Measurement(20, 100, 50, temp.Minutes, temp.Seconds);
            InitializeComponent();
            FormClosing += Simulator_FormClosing;
            UpdateData();
        }

        private void Simulator_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void UpdateData()
        {

        }
    }
}
