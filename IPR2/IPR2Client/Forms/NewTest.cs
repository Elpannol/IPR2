using IPR2Client.Simulation;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace IPR2Client.Forms
{
    public delegate void AddTraining(Training training);
    delegate void StringArgReturningVoidDelegate(Label box, string text);

    public partial class NewTest : Form
    {
        private BicycleConnection connection;

        private string _name;
        private int _interval;
        private Results results;
        private Thread runThread;

        private AddTraining _addTraining;
        public List<Measurement> measurements;
        private List<int> heartRates;

        public int _age;
        public bool _isMan;
        public int currentPower;
        public int wishedPower;
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
            TrainingStateLabel.Text = "Start";

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
        private void Initialise(string name, AddTraining addTraining)
        {
            // Some needed attributes

            currentPower = 20;
            wishedPower = 20;
            measurements = new List<Measurement>();
            _name = name;
            _addTraining = addTraining;

            heartRates = new List<int>();
            var message = Login.Handler.GetAge(name);
            _age = (int)message.data.age;
            _isMan = (bool)message.data.isman;
            chooser = new TrainingChooser(_age, _isMan);
            state = TrainingState.START;

            runThread = new Thread(React);
            runThread.Start();
        }

        /**
         * During a test this function is called every 1000 ms. (so, this
         * is the from where the logic for the test should be implemented).
         */
        private void React()
        {
            while (true)
            {
                try
                {
                    bool giveMeas = true;
                    //Changed the write and read line to send and receive command
                    connection.StartBicycle();

                    Measurement meas = null;
                    if (state != TrainingState.STOP)
                    {
                        var temp = connection.ReceiveCommand();
                        if (temp == "RUN\r")
                        {
                            giveMeas = false;
                        }
                        else meas = ParseMeasurement(temp);
                    }

                    if(meas != null && meas.Time.Seconds % 5 == 0)
                    {
                        if (currentPower > wishedPower)
                        {
                            currentPower -= 5;
                            connection.ChangePower($"{currentPower}");
                        }
                        else if (currentPower < wishedPower)
                        {
                            currentPower += 5;
                            connection.ChangePower($"{currentPower}");
                        }
                    }

                    switch (state)
                    {
                        case TrainingState.START:
                            if (meas.Rondes == 0)
                            {
                                giveMeas = false;
                                break;
                            }
                            SetText(TrainingStateLabel, "Warming up");
                            state = TrainingState.WARMINGUP;
                            connection.ChangePower($"{currentPower}");
                            break;
                        case TrainingState.WARMINGUP:
                            if (meas.Time.Seconds == 0)
                            {
                                heartRates.Add(meas.Hartslag);
                            }
                            if (meas.Time.Minutes == 2)
                            {
                                wishedPower = 50;
                                state = TrainingState.REALTEST;
                                SetText(TrainingStateLabel, "Test");
                            }
                            break;
                        case TrainingState.REALTEST:
                            if (meas.Time.Seconds % 5 == 0)
                            {
                                checkPower(meas);
                            }

                            if (meas.Time.Minutes == 5)
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


                            if (meas.Time.Minutes == 6)
                            {
                                SetText(TrainingStateLabel, "Cooling down");
                                state = TrainingState.COOLINGDOWN;
                                double vo2 = chooser.CalculateVo2(heartRates, wishedPower);
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

                                if (wishedPower > 100) wishedPower -= 50;
                                else wishedPower = 50;
                            }
                            break;
                        case TrainingState.COOLINGDOWN:
                            if (meas.Time.Minutes == 7)
                            {
                                SetText(TrainingStateLabel, "Test stop");
                                state = TrainingState.STOP;
                            }
                            break;
                        case TrainingState.STOP:
                            giveMeas = false;
                            break;
                    }

                    if (giveMeas) measurements.Add(meas);
                    Thread.Sleep(1000);
                }
                catch (Exception exception)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"Exception! : {exception.StackTrace}");
                }
            }
        }

        private void checkPower(Measurement meas)
        {
            if (meas.Hartslag > chooser.maxHeartRate)
            {
                state = TrainingState.STOP;
                SetText(TrainingStateLabel, "Test stop");
                setWarning("WANRING: Heartrate too high");

            }
            else if (meas.Hartslag <= 130)
            {
                if (_isMan) wishedPower += 50;
                else wishedPower += 25;
            }
            else
            {
                if (meas.Rondes < 50)
                {
                    if (_isMan) wishedPower += 25;
                    else wishedPower += 12;
                }
                else if (meas.Rondes > 60)
                {
                    if (_isMan) wishedPower -= 25;
                    else wishedPower -= 12;
                }
            }
        }

        private void setWarning(string text)
        {
            SetText(warningLabel, text);
        }


        private void NewTest_FormClosing(object sender, FormClosingEventArgs e)
        {

            connection.ResetBicycle();
            connection.Close();

            if (runThread != null)
            {
                runThread.Interrupt();
                runThread.Abort();
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
            SetText(weerstandLabel, $"{weerstand} Watt");
            SetText(hartslagLabel, $"{hartslag} BPM");
            SetText(rondesLabel, $"{rondes} BPM");
            SetText(tijdLabel, $"{tijd} Minuten");
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
            if (tempMeasurement.Rondes > 0)
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

        private void SetText(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetText);
                this.Invoke(d, new object[] { label, text });
            }
            else
            {
                label.Visible = true;
                label.Text = text;
            }
        }
    }
}
