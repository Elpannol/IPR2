using IPR2Client.Simulation;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;

namespace IPR2Client.Forms
{
    public delegate void AddTraining(Training training);

    public partial class NewTest : Form
    {
        private BicycleConnection connection;

        private string _name;
        private Results results;
        private Timer timer1;
        private AddTraining _addTraining;
        public List<Measurement> measurements;

        public NewTest(string name, Results results, AddTraining addTraining)
        {
            this.results = results;
            InitializeComponent();
            FormClosing += NewTest_FormClosing;

            // Create a connection to the bike using the simulator.
            this.connection = new BicycleConnection(name);
            // Initialise the rest of the attributes and start the timer
            this.Initialise(name, addTraining);
        }

        public NewTest(string name, Results results, SerialPort serialPort, AddTraining addTraining)
        {
            this.results = results;
            InitializeComponent();
            FormClosing += NewTest_FormClosing;

            // Create a connection to the real bike using the port.
            this.connection = new BicycleConnection(serialPort);
            // Initialise the rest of the attributes and start the timer
            this.Initialise(name, addTraining);
        }

        /**
         * Initialise attribues used by both constructors.
         */
        private void Initialise(string name, AddTraining addTraining) {
            // Some needed attributes
            measurements = new List<Measurement>();
            _name = name;
            _addTraining = addTraining;

            // Start the timer
            timer1 = new Timer();
            timer1.Tick += new EventHandler(React);
            timer1.Interval = 1000;
            timer1.Start();
        }

        /**
         * During a test this function is called every 1000 ms. (so, this
         * is the from where the logic for the test should be implemented).
         */
        private void React(object sender, EventArgs e)
        {
            try
            {
                //Changed the write and read line to send and receive command
                Console.WriteLine("Sending");
                this.connection.SendCommand("ST\n\r");

                Console.WriteLine("Reading...");
                var temp = this.connection.ReceiveCommand();

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

            this.connection.Close();

            if(timer1 != null)
            {
                timer1.Stop();
                Training training = new Training(measurements, _name);
                results.AddTraining(training);
            }

            this.Dispose();
        }

        /**
         * Update the from's text fields.
         */
        public void update(string weerstand, string hartslag, string rondes, string tijd)
        {
            weerstandLabel.Text = $"{weerstand} Watt";
            hartslagLabel.Text  = $"{hartslag} BPM";
            rondesLabel.Text    = $"{rondes} RPM";
            tijdLabel.Text      = $"{tijd} Minuten";
        }

        /**
         * This function creates a measurement from a string.
         * AND it sends the measurment to the server.
         */
        public Measurement ParseMeasurement(string inputString)
        {
            // Some string parsing
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

            // Create a measurement
            var tempTime = new SimpleTime(
                int.Parse(simpleTimeString[0]),
                int.Parse(simpleTimeString[1]));
            var tempMeasurement = new Measurement(list[2]/10,
                list[0],
                list[1],
                tempTime.Minutes,
                tempTime.Seconds);

            // This is where data is sent to the server.
            Login.Handler.AddMeasurement(tempMeasurement, _name);
            Login.Handler.ReadMessage();
            Login.Handler.AddLogEntry(tempMeasurement.ToString(), _name);
            Login.Handler.ReadMessage();

            // Update simply changes the text fields.
            update(tempMeasurement.Weerstand.ToString(),
                tempMeasurement.Hartslag.ToString(),
                tempMeasurement.Rondes.ToString(),
                tempMeasurement.Time.ToString());

            return tempMeasurement;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
