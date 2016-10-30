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

        public Results(TcpClient client, string gebruikersnaam)
        {
            this.client = client;
            InitializeComponent();
            FormClosing += Results_FormClosing;
            loginLabel.Text = gebruikersnaam;
            _gebruikersnaam = gebruikersnaam;
        }

        private void Results_FormClosing(object sender, FormClosingEventArgs e)
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
            NewTest newTest = new NewTest(client, _gebruikersnaam);
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
    }
}
