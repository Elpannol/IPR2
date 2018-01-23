using IPR2Client.Simulation;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;

namespace IPR2Client.Forms
{
    public partial class Results : Form
    {
        private string _gebruikersnaam;
        private List<Training> trainingen;
        private Training currentTraining;
        private Measurement currentMeasurement;
        public string[] PortStrings { get; }
        private NewTest newTest;

        public Results(string gebruikersnaam)
        {
            InitializeComponent();
            FormClosing += Results_FormClosing;
            loginLabel.Text = gebruikersnaam;
            _gebruikersnaam = gebruikersnaam;
            trainingen = new List<Training>();
            getTrainings();

            PortStrings = SerialPort.GetPortNames();
            foreach (var port in PortStrings) {
                comportBox.Items.Add(port);
            }
            comportBox.Items.Add("Simulation");
        }

        public void getTrainings()
        {
            List<string> trainingNameList = Login.Handler.GetTrainingen(_gebruikersnaam);
            trainingen = new List<Training>();
            foreach (String s in trainingNameList)
            {
                trainingen.Add(new Training(new List<Measurement>(), s));
            }

            foreach(Training t in trainingen)
            {
                var message = Login.Handler.getLog(_gebruikersnaam, t._name);
                t.vo2 = (double)message.vo2;
                List<string> log = new List<string>();
                for (int i = 0; i < message.data.log.Count; i++)
                {
                    log.Add((string)message.data.log[i]);
                }

                foreach (String s in log)
                {
                    var list = s.Split();
                    var timeString = list[3];
                    list[3] = "0";
                    var timeList = timeString.Split(':');
                    t._measurements.Add(new Measurement(int.Parse(list[0]), int.Parse(list[1]), int.Parse(list[2]), int.Parse(timeList[0]), int.Parse(timeList[1])));
                }
            }
            refresh();

        }

        private void Results_FormClosing(object sender, FormClosingEventArgs e)
        {
            Login.Handler.Disconnect();
            Application.Exit();
        }

        private void newTestButton_Click(object sender, EventArgs e) {
            Login.Handler.StartTraining();

            if (comportBox.SelectedItem?.ToString() == "Simulation" || comportBox.SelectedItem?.ToString() == null)
            {
                newTest = new NewTest(_gebruikersnaam, this, AddTraining);
            }
            else if (comportBox.SelectedItem != null)
            {
                var serialPort = new SerialPort(comportBox.SelectedItem.ToString());
                serialPort.Open();

                newTest = new NewTest(_gebruikersnaam, this, serialPort, AddTraining);
            }
            Visible = false;
            newTest.Visible = true;
        }

        private void trainingListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setWarning("");
            currentTraining = trainingen[trainingListBox.SelectedIndex];
            currentMeasurement = currentTraining.getMeasurement(0);
            if (currentMeasurement == null) {
                trainingen.Remove(currentTraining);
                refresh();
                return;
            }

            if(currentTraining.vo2 == -1)
            {
                setWarning("Vo2 could not be calculated!!");
            }
            else
            {
                setWarning($"Vo2 calculated: {currentTraining.vo2:##.00}");
            }

            weerstandLabel.Text = "" +currentMeasurement.Weerstand; 
            hartslagLabel.Text = "" + currentMeasurement.Hartslag;
            rondesLabel.Text = "" + currentMeasurement.Rondes;
            tijdLabel.Text = currentMeasurement.Time.ToString();

            timeTrackBar.Update();
            timeTrackBar.Maximum = currentTraining.getLength();
        }
        
        public void AddTraining(Training training)
        {
            trainingen.Add(training);
            refresh();
        }

        public void refresh()
        {
            try {
                trainingListBox.Items.Clear();
            } catch (Exception exception) {
                Console.WriteLine(exception.StackTrace);
            }

            foreach(Training t in trainingen)
            {
                trainingListBox.Items.Add(t._name);
            }
        }

        private void timeTrackBar_Scroll(object sender, EventArgs e)
        {
            try
            {
                currentMeasurement = currentTraining.getMeasurement(timeTrackBar.Value);
                weerstandLabel.Text = "" + currentMeasurement.Weerstand;
                hartslagLabel.Text = "" + currentMeasurement.Hartslag;
                rondesLabel.Text = "" + currentMeasurement.Rondes;
                tijdLabel.Text = currentMeasurement.Time.ToString();
            }
            catch (NullReferenceException exception)
            {
                Console.WriteLine("Stupid slider exception!");
                //Console.WriteLine(exception.StackTrace);
            }
        }
        private void setWarning(string text)
        {
            warningLabel.Visible = true;
            warningLabel.Text = text;
        }
    }
}
