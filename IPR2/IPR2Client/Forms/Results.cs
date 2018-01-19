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

            PortStrings = SerialPort.GetPortNames();
            foreach (var port in PortStrings) {
                comportBox.Items.Add(port);
            }
            comportBox.Items.Add("Simulation");
        }

        private void Results_FormClosing(object sender, FormClosingEventArgs e)
        {
            Login.Handler.saveAllData();
            Login.Handler.Disconnect();
            Application.Exit();
        }

        private void newTestButton_Click(object sender, EventArgs e) {
            Visible = false;
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
            newTest.Visible = true;
        }

        private void trainingListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentTraining = trainingen[trainingListBox.SelectedIndex];
            currentMeasurement = currentTraining.getMeasurement(0);

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

            for(int i = 0; i<trainingen.Count; i++)
            {
                trainingListBox.Items.Add(string.Format("Training " + (i+1)));
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
    }
}
