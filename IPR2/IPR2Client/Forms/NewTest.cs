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
        private int _interval;
        private Results results;
        private Timer timer1;

        private AddTraining _addTraining;
        public List<Measurement> measurements;
        private List<int> heartRates;

        public int _age;
        public bool _isMan;
        public int currentPower;
        public TrainingState state;
        public TrainingChooser chooser;

        public NewTest(string name, Results results, AddTraining addTraining)
        {
            this.results = results;
            InitializeComponent();
            FormClosing += NewTest_FormClosing;
            _interval = 1000;
            TrainingStateLabel.Text = "Start";

            // Create a connection to the bike using the simulator.
            connection = new BicycleConnection(name);
            // Initialise the rest of the attributes and start the timer
            Initialise(name, addTraining);
        }

        public NewTest(string name, Results results, SerialPort serialPort, AddTraining addTraining)
        {
            this.results = results;
            InitializeComponent();
            FormClosing += NewTest_FormClosing;

            //TODO: change this if it makes the bicycle buffer fucked up
            _interval = 1000;

            // Create a connection to the real bike using the port.
            connection = new BicycleConnection(serialPort);
            // Initialise the rest of the attributes and start the timer
            Initialise(name, addTraining);
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

            heartRates = new List<int>();
            var message = Login.Handler.GetAge(name);
            _age = (int)message.data.age;
            _isMan = (bool)message.data.isman;
            chooser = new TrainingChooser(_age, _isMan);
            state = TrainingState.START;

            // Start the timer
            timer1 = new Timer();
            timer1.Tick += new EventHandler(React);
            timer1.Interval = _interval;
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
                connection.StartBicycle();
                Console.WriteLine("Sending");

                Measurement meas = null;
                if (state != TrainingState.STOP)
                {
                    Console.WriteLine("Reading...");
                    var temp = connection.ReceiveCommand();
                    meas = ParseMeasurement(temp);
                }

                switch (state)
                {
                    case TrainingState.START:
                        if(meas.Rondes == 0)
                        {
                            return;
                        }
                        TrainingStateLabel.Text = "Warming up";
                        state = TrainingState.WARMINGUP;
                        break;
                    case TrainingState.WARMINGUP:
                        connection.EnableCommand();
                        connection.ChangePower($"{currentPower}");
                        if (meas.Time.Seconds == 0)
                        {
                            heartRates.Add(meas.Hartslag);
                        }
                        if (meas.Time.Minutes == 2)
                        {
                            currentPower = 50;
                            connection.ChangePower($"{currentPower}");
                            state = TrainingState.REALTEST;
                            TrainingStateLabel.Text = "Test";
                        }
                        break;
                    case TrainingState.REALTEST:
                        if (meas.Time.Seconds % 5 == 0)
                        {
                            checkPower(meas);
                        }

                        if(meas.Time.Minutes == 5)
                        {
                            if (meas.Time.Seconds % 15 == 0)
                            {
                                heartRates.Add(meas.Hartslag);
                            }
                        }
                        else
                        {
                            if (meas.Time.Seconds == 0)
                            {
                                heartRates.Add(meas.Hartslag);
                            }
                        }


                        if(meas.Time.Minutes == 6)
                        {
                            TrainingStateLabel.Text = "Cooling down";
                            state = TrainingState.COOLINGDOWN;
                            double vo2 = chooser.CalculateVo2(heartRates, currentPower);
                            if (vo2 == -1)
                            {
                                setWarning("Can't calculate vo2, average heartrate too low");
                                Login.Handler.SendVo2(_name, vo2);
                            }
                            else
                            {
                                setWarning($"Vo2 calculated: {vo2:##.00}");
                                Login.Handler.SendVo2(_name, vo2);
                            }

                            if (currentPower > 100) currentPower -= 50;
                            else currentPower = 50;
                            connection.EnableCommand();
                            connection.ChangePower($"{currentPower}");
                        }
                        break;
                    case TrainingState.COOLINGDOWN:
                        if(meas.Time.Minutes == 7)
                        {
                            TrainingStateLabel.Text = "Test stop";
                            state = TrainingState.STOP;
                        }
                        break;
                    case TrainingState.STOP:
                        return;
                }

                measurements.Add(meas);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception! : {exception.StackTrace}");
            }   
        }

        private void checkPower(Measurement meas)
        {
            if (meas.Hartslag > chooser.maxHeartRate)
            {
                state = TrainingState.STOP;
                TrainingStateLabel.Text = "Test stop";
                setWarning("WANRING: Heartrate too high");

            }
            else if (meas.Hartslag <= 130)
            {
                if (_isMan) currentPower += 50;
                else currentPower += 25;

                connection.EnableCommand();
                connection.ChangePower($"{currentPower}");
            }
            else
            {
                if (meas.Rondes < 50)
                {
                    if (_isMan) currentPower += 25;
                    else currentPower += 12;
                    connection.EnableCommand();
                    connection.ChangePower($"{currentPower}");
                }
                else if (meas.Rondes > 60)
                {
                    if (_isMan) currentPower -= 25;
                    else currentPower -= 12;
                    connection.EnableCommand();
                    connection.ChangePower($"{currentPower}");
                }
            }
        }

        private void setWarning(string text)
        {
            warningLabel.Visible = true;
            warningLabel.Text = text;
        }
        

        private void NewTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            connection.ResetBicycle();
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

            int minusTime = 0;
            switch (state)
            {
                case TrainingState.REALTEST:
                    minusTime = 2;
                    break;
                case TrainingState.COOLINGDOWN:
                    minusTime = 6;
                    break;

            }

            string time = $"{tempMeasurement.Time.Minutes - minusTime:00}:{tempMeasurement.Time.Seconds:00}";

            // Update simply changes the text fields.
            update(tempMeasurement.Weerstand.ToString(),
                tempMeasurement.Hartslag.ToString(),
                tempMeasurement.Rondes.ToString(),
                time);

            return tempMeasurement;
        }
    }
}
