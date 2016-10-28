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

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            UpdateSim();
        }

        private void Simulator_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void UpdateData()
        {
            weerstand.Text      = Measurement.Weerstand + "";
            hartslag.Text       = Measurement.Hartslag + "";
            rondes.Text         = Measurement.Rondes + "";
            tijd.Text           = Measurement.Time.ToString();
        }

        private void weerstandMin_Click(object sender, EventArgs e)
        {
            if (Measurement.Weerstand > 4)
                Measurement.Weerstand -= 5;
        }

        private void weerstandPlus_Click(object sender, EventArgs e)
        {
            if (Measurement.Weerstand < 96)
                Measurement.Weerstand += 5;
        }

        private void hartslagMin_Click(object sender, EventArgs e)
        {
            if (Measurement.Hartslag > 4)
                Measurement.Hartslag -= 5;
        }

        private void hartslagPlus_Click(object sender, EventArgs e)
        {
            if (Measurement.Hartslag < 196)
                Measurement.Hartslag += 5;
        }

        private void rondesMin_Click(object sender, EventArgs e)
        {
            if (Measurement.Rondes > 4)
                Measurement.Rondes -= 5;
        }

        private void rondesPlus_Click(object sender, EventArgs e)
        {
            if (Measurement.Rondes < 196)
                Measurement.Rondes += 5;
        }

        public void UpdateSim()
        {
            if (_time < 5999)
            {
                _time++;
                Measurement.Time = new SimpleTime(_time / 60, _time % 60);
            }
            UpdateData();
        }
    }
}
