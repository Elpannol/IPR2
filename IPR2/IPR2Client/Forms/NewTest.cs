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

        public int _age;
        public int currentPower;
        public TrainingState state;
        public TrainingChooser chooser;

        public NewTest(string name, Results results, AddTraining addTraining)
        {
            this.results = results;
            InitializeComponent();
            FormClosing += NewTest_FormClosing;
            _age = Login.Handler.GetAge(name);

            // Create a connection to the bike using the simulator.
            this.connection = new BicycleConnection(name);
            // Initialise the rest of the attributes and start the timer
            this.Initialise(name, addTraining);
            state = TrainingState.START;

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
            currentPower = 20;
            measurements = new List<Measurement>();
            _name = name;
            _addTraining = addTraining;
            chooser = new TrainingChooser(_age);

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
                connection.StartBicycle();

                Console.WriteLine("Reading...");
                var temp = connection.ReceiveCommand();
                Measurement meas = ParseMeasurement(temp);

                switch (state)
                {
                    case TrainingState.START:
                        if(meas.Rondes == 0)
                        {
                            return;
                        }
                        state = TrainingState.WARMINGUP;
                        break;
                    case TrainingState.WARMINGUP:
                        connection.EnableCommand();
                        connection.ChangePower($"{currentPower}");
                        if (meas.Time.Seconds == 0)
                        {
                            checkHeartRate(meas);
                        }
                        if (meas.Time.Minutes == 2)
                        {
                            state = TrainingState.REALTEST;
                        }
                        break;
                    case TrainingState.REALTEST:
                        if(meas.Rondes < 50)
                        {
                            currentPower = currentPower + 5;
                            connection.EnableCommand();
                            connection.ChangePower($"{currentPower}");
                        }
                        else if(meas.Rondes > 60)
                        {
                            currentPower = currentPower - 5;
                            connection.EnableCommand();
                            connection.ChangePower($"{currentPower}");
                        }

                        if(meas.Time.Minutes == 5)
                        {
                            if (meas.Time.Seconds % 15 == 0)
                            {
                                checkHeartRate(meas);
                            }
                        }
                        else
                        {
                            if (meas.Time.Seconds == 0)
                            {
                                checkHeartRate(meas);
                            }
                        }


                        if(meas.Time.Minutes == 6)
                        {
                            state = TrainingState.COOLINGDOWN;
                        }
                        break;
                    case TrainingState.COOLINGDOWN:
                        if (meas.Time.Seconds % 15 == 0)
                        {
                            checkHeartRate(meas);
                        }

                        if(meas.Time.Minutes == 7)
                        {
                            state = TrainingState.STOP;
                        }
                        break;
                    case TrainingState.STOP:
                        Close();
                        return;
                }

                measurements.Add(meas);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception! : {exception.StackTrace}");
            }   
        }

        private void checkHeartRate(Measurement meas)
        {
            if (meas.Hartslag > chooser.maxHeartRate)
            {
                //TODO: add warning for this
                currentPower = currentPower - 5;
                connection.EnableCommand();
                connection.ChangePower($"{currentPower}");
            }
            else if (meas.Hartslag < 130)
            {
                currentPower = currentPower + 5;
                connection.EnableCommand();
                connection.ChangePower($"{currentPower}");
            }

        }

        private void NewTest_FormClosing(object sender, FormClosingEventArgs e)
        {

            connection.Close();

            if(timer1 != null)
            {
                timer1.Stop();
                Training training = new Training(measurements, _name);
                results.AddTraining(training);
            }

            Login.Handler.EndTraining();

            results.getTrainings();
            results.Visible = true;

            Dispose();
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
            var tempMeasurement = new Measurement(list[7],
                list[0],
                list[1],
                tempTime.Minutes,
                tempTime.Seconds);

            // This is where data is sent to the server.
            if(tempMeasurement.Rondes > 0)
            {
                Login.Handler.AddMeasurement(tempMeasurement, _name);
                Login.Handler.ReadMessage();
                Login.Handler.AddLogEntry(tempMeasurement.ToString(), _name);
                Login.Handler.ReadMessage();
            }

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
