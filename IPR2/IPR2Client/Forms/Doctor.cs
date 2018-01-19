using IPR2Client.Simulation;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace IPR2Client.Forms
{
    public partial class Doctor : Form
    {
        private string _gebruikersnaam;

        private string selectedName;
        private List<Training> trainingen;
        private List<string> users;
        private List<string> log;
        private Training currentTraining;
        private Measurement currentMeasurement;

        public Doctor(string gebruikersnaam)
        {
            InitializeComponent();
            FormClosing += Doctor_FormClosing;
            loginLabel.Text = gebruikersnaam;
            _gebruikersnaam = gebruikersnaam;
            users = Login.Handler.GetUsers();
            trainingen = new List<Training>();
            fillUserBox();
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
            }
            catch (Exception exception) { }
        }

        private void trainingListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            log = Login.Handler.getLog(selectedName, trainingen[trainingListBox.SelectedIndex]._name);
            Training training = trainingen[trainingListBox.SelectedIndex];
            training._measurements = new List<Measurement>();

            foreach(String s in log)
            {
                var list = s.Split();
                var timeString = list[3];
                list[3] = "0";
                var timeList = timeString.Split(':');
                training._measurements.Add(new Measurement(int.Parse(list[0]), int.Parse(list[1]), int.Parse(list[2]), int.Parse(timeList[0]), int.Parse(timeList[1])));
            }
        }

        private void userListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> list = Login.Handler.GetTrainingen(users[userListBox.SelectedIndex]);
            trainingen = new List<Training>();
            foreach(String s in list)
            {
                trainingen.Add(new Training(new List<Measurement>(), s));
            }
            selectedName = users[userListBox.SelectedIndex];
            fillTrianingBox();
        }

        private void fillUserBox()
        {
            string[] userArray = new string[users.Count];

            for(int i = 0; i<users.Count; i++)
            {
                userArray[i] = users[i];
            }
            Array.Sort(userArray);

            userListBox.Items.Clear();

            for (int i = 0; i < users.Count; i++)
            {
                userListBox.Items.Add(userArray[i].ToString());
            }
        }

        private void fillTrianingBox()
        {
            string[] trainingArray = new string[trainingen.Count];

            for (int i = 0; i < trainingen.Count; i++)
            {
                trainingArray[i] = trainingen[i]._name;
            }
            Array.Sort(trainingArray);

            trainingListBox.Items.Clear();

            for (int i = 0; i < trainingen.Count; i++)
            {
                trainingListBox.Items.Add(trainingArray[i].ToString());
            }
        }
    }
}
