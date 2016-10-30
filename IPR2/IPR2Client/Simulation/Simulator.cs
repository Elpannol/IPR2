using IPR2Client.Simulation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private NewTest newTest;
        private string _name;
        private Results results;
        private Timer timer1;
        private List<Measurement> measurements = new List<Measurement>();

        public Simulator(TcpClient client, NewTest newTest, string name, Results results)
        {
            this.results = results;
            _name = name;
            this.newTest = newTest;
            this.client = client;
            _time = 0;
            SimpleTime temp = new SimpleTime(_time / 60, _time % 60);
            Measurement = new Measurement(20, 100, 50, temp.Minutes, temp.Seconds);
            InitializeComponent();
            FormClosing += Simulator_FormClosing;

            weerstand.Text = Measurement.Weerstand + "";
            hartslag.Text = Measurement.Hartslag + "";
            rondes.Text = Measurement.Rondes + "";

            timer1 = new Timer();
            timer1.Tick += new EventHandler(UpdateSim);
            timer1.Interval = 1000;
            timer1.Start();
        }

        private void Simulator_FormClosing(object sender, FormClosingEventArgs e)
        {
            results.Visible = true;
            newTest.Dispose();
            stop();
            this.Dispose();
        }

        public void stop()
        {
            timer1.Stop();
            Training training = new Training(measurements, _name);
            results.AddTraining(training);
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
            newTest.update(weerstand.Text, hartslag.Text, rondes.Text, tijd.Text);

           AddLogEntry(Measurement.ToString());

            dynamic message = new
            {
                id = "add/measurement",
                data = new
                {
                    weerstand = Measurement.Weerstand,
                    hartslag = Measurement.Hartslag,
                    rondes = Measurement.Rondes,
                    timeM = Measurement.Time.Minutes,
                    timeS = Measurement.Time.Seconds,
                    name = _name
                }
            };

            SendMessage(client, message);

            measurements.Add(new Measurement(Measurement.Weerstand, Measurement.Hartslag, Measurement.Rondes, Measurement.Time));
        }

        public void AddLogEntry(string text)
        {
            dynamic message = new
            {
                id = "add/logentry",
                data = new
                {
                    text = text,
                    name = _name
                }
            };

            SendMessage(client, message);
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
