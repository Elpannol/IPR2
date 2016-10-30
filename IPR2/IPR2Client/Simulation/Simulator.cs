using IPR2Client.Simulation;
using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace IPR2Client.Forms
{
    public partial class Simulator : Form
    {
        private readonly Measurement Measurement;
        private int _time;
        private TcpClient client;

        public Simulator(TcpClient client)
        {
            this.client = client;
            _time = 0;
            SimpleTime temp = new SimpleTime(_time / 60, _time % 60);
            Measurement = new Measurement(20, 100, 50, temp.Minutes, temp.Seconds);
            InitializeComponent();
            FormClosing += Simulator_FormClosing;

            weerstand.Text = Measurement.Weerstand + "";
            hartslag.Text = Measurement.Hartslag + "";
            rondes.Text = Measurement.Rondes + "";

            Timer timer1 = new Timer();
            timer1.Tick += new EventHandler(UpdateSim);
            timer1.Interval = 1000;
            timer1.Start();
        }

        private void Simulator_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.GetStream().Close();
            client.Close();
            Application.Exit();
        }

        private void weerstandMin_Click(object sender, EventArgs e)
        {
            if (Measurement.Weerstand > 4)
                Measurement.Weerstand -= 5;
            weerstand.Text = Measurement.Weerstand + "";
        }

        private void weerstandPlus_Click(object sender, EventArgs e)
        {
            if (Measurement.Weerstand < 96)
                Measurement.Weerstand += 5;
            weerstand.Text = Measurement.Weerstand + "";
        }

        private void hartslagMin_Click(object sender, EventArgs e)
        {
            if (Measurement.Hartslag > 4)
                Measurement.Hartslag -= 5;
            hartslag.Text = Measurement.Hartslag + "";
        }

        private void hartslagPlus_Click(object sender, EventArgs e)
        {
            if (Measurement.Hartslag < 196)
                Measurement.Hartslag += 5;
            hartslag.Text = Measurement.Hartslag + "";
        }

        private void rondesMin_Click(object sender, EventArgs e)
        {
            if (Measurement.Rondes > 4)
                Measurement.Rondes -= 5;
            rondes.Text = Measurement.Rondes + "";
        }

        private void rondesPlus_Click(object sender, EventArgs e)
        {
            if (Measurement.Rondes < 196)
                Measurement.Rondes += 5;
            rondes.Text = Measurement.Rondes + "";
        }

        public void UpdateSim(object sender, EventArgs e)
        {
            if (_time < 5999)
            {
                _time++;
                Measurement.Time = new SimpleTime(_time / 60, _time % 60);
            }
            tijd.Text = Measurement.Time.ToString();
        }

        public void SendMessage(TcpClient client, dynamic message)
        {
            //make sure the other end decodes with the same format!
            message = JsonConvert.SerializeObject(message);
            byte[] bytes = Encoding.Unicode.GetBytes(message);
            client.GetStream().Write(bytes, 0, bytes.Length);
            client.GetStream().Flush();
        }
    }
}
