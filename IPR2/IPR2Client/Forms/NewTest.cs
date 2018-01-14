using IPR2Client.Simulation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPR2Client.Forms
{
    public delegate void AddTraining(Training training);

    public partial class NewTest : Form
    {
        private string _name;
        private Simulator simulator;
        private Results results;
        private SerialPort serialPort;
        private Timer timer1;
        private readonly AddTraining _addTraining;
        public List<Measurement> measurements;

        public NewTest(string name, Results results)
        {
            this.results = results;
            InitializeComponent();
            FormClosing += NewTest_FormClosing;
            simulator = new Simulator(this, name, results);
            simulator.Visible = true;
            _name = name;

            measurements = new List<Measurement>();
        }

        public NewTest(string name, Results results, SerialPort serialPort, AddTraining addTraining)
        {
            this.results = results;
            InitializeComponent();
            FormClosing += NewTest_FormClosing;
            this.serialPort = serialPort;
            _name = name;

            measurements = new List<Measurement>();

            _addTraining = addTraining;

            timer1 = new Timer();
            timer1.Tick += new EventHandler(React);
            timer1.Interval = 1000;
            timer1.Start();
        }

        private void React(object sender, EventArgs e)
        {
            try
            {
                //Changed the write and read line to send and receive command
                Console.WriteLine("Sending");
                BicycleCommand.SendCommand("ST\n\r", serialPort);

                Console.WriteLine("Reading...");
                var temp = BicycleCommand.ReceiveCommand(serialPort);

                measurements.Add(ParseMeasurement(temp));
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception! : {exception.StackTrace}");
            }   
        }

        private void NewTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            results.refresh();
            results.Visible = true;

            if(simulator != null)
            {
                simulator.stop();
                simulator.Dispose();
            }

            if(timer1 != null)
            {
                timer1.Stop();
                Training training = new Training(measurements, _name);
                results.AddTraining(training);
            }

            this.Dispose();
        }

        public void update(string weerstand, string hartslag, string rondes, string tijd)
        {
            weerstandLabel.Text = weerstand    + " Watt";
            hartslagLabel.Text  = hartslag     + " BPM";
            rondesLabel.Text    = rondes       + " RPM";
            tijdLabel.Text      = tijd         + " Minuten";
        }

        public Measurement ParseMeasurement(string inputString)
        {
            inputString = inputString.Trim();
            var splitString = inputString.Split();
            var simpleTimeString = splitString[6].Split(':');

            splitString[6] = "0";
            int[] list = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var index = 0;
            foreach (var s in splitString)
            {
                list[index] = int.Parse(s);
                index++;
            }

            var tempTime = new SimpleTime(int.Parse(simpleTimeString[0]), int.Parse(simpleTimeString[1]));
            var tempMeasurement = new Measurement(list[2]/10, list[0], list[1],tempTime.Minutes, tempTime.Seconds);
            Login.Handler.AddMeasurement(tempMeasurement, _name);
            Login.Handler.ReadMessage();
            Login.Handler.AddLogEntry(tempMeasurement.ToString(),_name);
            Login.Handler.ReadMessage();
            update(tempMeasurement.Weerstand.ToString(), tempMeasurement.Hartslag.ToString(), tempMeasurement.Rondes.ToString(), tempMeasurement.Time.ToString());

            return tempMeasurement;
        }
    }
}
