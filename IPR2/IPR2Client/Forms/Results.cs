using IPR2Client.Simulation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPR2Client.Forms
{
    public partial class Results : Form
    {
        private string _gebruikersnaam;
        private List<Training> trainingen;
        private Training currentTraining;
        private Measurement currentMeasurement;

        public Results(string gebruikersnaam)
        {
            InitializeComponent();
            FormClosing += Results_FormClosing;
            loginLabel.Text = gebruikersnaam;
            _gebruikersnaam = gebruikersnaam;
            //trainingen = Login.Handler.GetTrainingen(_gebruikersnaam);
        }

        private void Results_FormClosing(object sender, FormClosingEventArgs e)
        {
            Login.Handler.Disconnect();
            Application.Exit();
        }

        private void newTestButton_Click(object sender, EventArgs e)
        {
            Visible = false;
            Login.Handler.StartTraining();
            NewTest newTest = new NewTest(_gebruikersnaam, this);
            newTest.Visible = true;
        }

        private void trainingListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentTraining = trainingen[trainingListBox.SelectedIndex];
            currentMeasurement = currentTraining.getMeasurement(timeTrackBar.Value);
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
            try { trainingListBox.Items.Clear(); }
            catch (Exception exception) { Console.WriteLine(exception.StackTrace); }

            //trainingen = Login.Handler.GetTrainingen(_gebruikersnaam);

            for(int i = 0; i<trainingen.Count; i++)
            {
                trainingListBox.Items.Add(string.Format("Training " + (i+1)));
            }
        }

        private void timeTrackBar_Scroll(object sender, EventArgs e)
        {
            currentMeasurement = currentTraining.getMeasurement(0);
            weerstandLabel.Text = "" + currentMeasurement.Weerstand;
            hartslagLabel.Text = "" + currentMeasurement.Hartslag;
            rondesLabel.Text = "" + currentMeasurement.Rondes;
            tijdLabel.Text = currentMeasurement.Time.ToString();
        }

    }
}
