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
    public partial class Doctor : Form
    {
        private string _gebruikersnaam;
        private List<Training> trainingen;
        private List<User> users;
        private Training currentTraining;
        private Measurement currentMeasurement;

        public Doctor(string gebruikersnaam)
        {
            InitializeComponent();
            FormClosing += Doctor_FormClosing;
            loginLabel.Text = gebruikersnaam;
            _gebruikersnaam = gebruikersnaam;
            //users = Login.Handler.GetUsers();
            //fillUserBox();
        }

        private void Doctor_FormClosing(object sender, FormClosingEventArgs e)
        {
            Login.Handler.Disconnect();
            Application.Exit();
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
            catch (Exception exception) { }
        }

        private void trainingListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentTraining = trainingen[trainingListBox.SelectedIndex];
            currentMeasurement = currentTraining.getMeasurement(0);
            weerstandLabel.Text = "" + currentMeasurement.Weerstand;
            hartslagLabel.Text = "" + currentMeasurement.Hartslag;
            rondesLabel.Text = "" + currentMeasurement.Rondes;
            tijdLabel.Text = currentMeasurement.Time.ToString();
            timeTrackBar.Update();
            timeTrackBar.Maximum = currentTraining.getLength();
        }

        private void userListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            trainingen = Login.Handler.GetTrainingen(userListBox.Text);
        }

        private void fillUserBox()
        {
            string[] userArray = new string[users.Count];

            for(int i = 0; i<users.Count; i++)
            {
                userArray[i] = users[i]._gebruikersnaam;
            }
            Array.Sort(userArray);

            userListBox.Items.Clear();

            for (int i = 0; i < users.Count; i++)
            {
                userListBox.Items.Add(userArray[i].ToString());
            }
        }
    }
}
