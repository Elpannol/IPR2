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
        private TcpClient client;
        private string _gebruikersnaam;
        private List<Training> trainingen;
        private Training currentTraining;
        private Measurement currentMeasurement;

        public Results(TcpClient client, string gebruikersnaam)
        {
            this.client = client;
            InitializeComponent();
            FormClosing += Results_FormClosing;
            loginLabel.Text = gebruikersnaam;
            _gebruikersnaam = gebruikersnaam;
            trainingen = getTrainingen();
        }

        private void Results_FormClosing(object sender, FormClosingEventArgs e)
        {
            Save();

            try
            {
                dynamic message = new
                {
                    id = "client/disconnect",
                    data = new
                    {

                    }
                };

                SendMessage(client, message);


                client.GetStream().Close();
                client.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
            Application.Exit();
        }

        private void newTestButton_Click(object sender, EventArgs e)
        {
            Visible = false;
            NewTest newTest = new NewTest(client, _gebruikersnaam, this);
            newTest.Visible = true;
        }

        public void SendMessage(TcpClient client, dynamic message)
        {
            //make sure the other end decodes with the same format!
            message = JsonConvert.SerializeObject(message);
            byte[] bytes = Encoding.Unicode.GetBytes(message);
            client.GetStream().Write(bytes, 0, bytes.Length);
            client.GetStream().Flush();
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

        private void Save()
        {
            //try
            //{
            //    dynamic message = new
            //    {
            //        id = "save/training",
            //        data = new
            //        {
            //            name = _gebruikersnaam,
            //            trainingen = trainingen
            //        }
            //    };

            //    SendMessage(client, message);
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine(exception.StackTrace);
            //}
        }

        private List<Training> getTrainingen()
        {
            //try
            //{
            //    dynamic message = new
            //    {
            //        id = "load/training",
            //        data = new
            //        {
            //            name = _gebruikersnaam
            //        }
            //    };

            //    SendMessage(client, message);

            //    dynamic feedback = JsonConvert.DeserializeObject(ReadMessage(client));
            //    return feedback.data.trainingen;
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine(exception.StackTrace);
                return new List<Training>();
            //}
        }

        public dynamic ReadMessage(TcpClient client)
        {

            byte[] buffer = new byte[1024];
            int totalRead = 0;

            //read bytes until stream indicates there are no more
            do
            {
                int read = client.GetStream().Read(buffer, totalRead, buffer.Length - totalRead);
                totalRead += read;
                Console.WriteLine("ReadMessage: " + read);
            } while (client.GetStream().DataAvailable);
            string message = Encoding.Unicode.GetString(buffer, 0, totalRead);
            Console.WriteLine(message);
            return message;
        }
    }
}
