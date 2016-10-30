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
        private TcpClient _client;
        private string _gebruikersnaam;
        private List<Training> trainingen;
        private List<User> users;
        private Training currentTraining;
        private Measurement currentMeasurement;

        public Doctor(TcpClient client, string gebruikersnaam)
        {
            _client = client;
            InitializeComponent();
            FormClosing += Doctor_FormClosing;
            loginLabel.Text = gebruikersnaam;
            _gebruikersnaam = gebruikersnaam;
            users = getUsers();
            fillUserBox();
        }

        private void Doctor_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                dynamic message = new
                {
                    id = "client/disconnect",
                    data = new
                    {

                    }
                };

                SendMessage(_client, message);


                _client.GetStream().Close();
                _client.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
            Application.Exit();
        }

        public void SendMessage(TcpClient client, dynamic message)
        {
            //make sure the other end decodes with the same format!
            message = JsonConvert.SerializeObject(message);
            byte[] bytes = Encoding.Unicode.GetBytes(message);
            client.GetStream().Write(bytes, 0, bytes.Length);
            client.GetStream().Flush();
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
            trainingen = getTrainingen(userListBox.Text);
        }

        private List<Training> getTrainingen(string gebruikersnaamclient)
        {
            //try
            //{
            //    dynamic message = new
            //    {
            //        id = "load/training",
            //        data = new
            //        {
            //            name = gebruikersnaamclient
            //        }
            //    };

            //    SendMessage(_client, message);

            //    dynamic feedback = JsonConvert.DeserializeObject(ReadMessage(_client));
            //    return feedback.data.trainingen;
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine(exception.StackTrace);
            return new List<Training>();
            //}
        }

        private List<User> getUsers()
        {
            //try
            //{
            //    dynamic message = new
            //    {
            //        id = "load/user",
            //        data = new
            //        {
            //        }
            //    };

            //    SendMessage(_client, message);

            //    dynamic feedback = JsonConvert.DeserializeObject(ReadMessage(_client));
            //    return feedback.data.users;
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine(exception.StackTrace);
                return new List<User>();
            //}
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
