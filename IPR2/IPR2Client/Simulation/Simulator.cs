using IPR2Client.Simulation;
using System;
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

            Timer timer1 = new Timer();
            timer1.Tick += new EventHandler(UpdateSim);
            timer1.Interval = 1000;
            timer1.Start();
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
            UpdateData();
        }

        private void weerstandPlus_Click(object sender, EventArgs e)
        {
            if (Measurement.Weerstand < 96)
                Measurement.Weerstand += 5;
            UpdateData();
        }

        private void hartslagMin_Click(object sender, EventArgs e)
        {
            if (Measurement.Hartslag > 4)
                Measurement.Hartslag -= 5;
            UpdateData();
        }

        private void hartslagPlus_Click(object sender, EventArgs e)
        {
            if (Measurement.Hartslag < 196)
                Measurement.Hartslag += 5;
            UpdateData();
        }

        private void rondesMin_Click(object sender, EventArgs e)
        {
            if (Measurement.Rondes > 4)
                Measurement.Rondes -= 5;
            UpdateData();
        }

        private void rondesPlus_Click(object sender, EventArgs e)
        {
            if (Measurement.Rondes < 196)
                Measurement.Rondes += 5;
            UpdateData();
        }

        public void UpdateSim(object sender, EventArgs e)
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
