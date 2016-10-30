using IPR2Client.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPR2Client
{
    public partial class Login : Form
    {
        private TcpClient client = new TcpClient();
        private IPAddress _currentId;

        public Login()
        {
            IPAddress localIp = GetLocalIpAddress();
            bool ipOk = IPAddress.TryParse(localIp.ToString(), out _currentId);
            if (!ipOk)
            {
                Console.WriteLine("Couldn't parse the ip address. Exiting code.");
                Environment.Exit(1);
            }

            InitializeComponent();
            wrongLabel.Visible = false;
            FormClosing += Login_FormClosing;

            client.Connect(_currentId, 1337);
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            try {
            dynamic message = new {
                            id = "client/disconnect",
                            data = new
                            {

                            }};

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

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if(tryLogin(gebruikersNaam.Text, wachtwoord.Text))
            {
                Visible = false;
                Results results = new Forms.Results(client, gebruikersNaam.Text);
                results.Visible = true;
            }
            else
            wrongLabel.Visible = true;
        }

        private bool tryLogin(string gebruikersnaam, string wachtwoord)
        {
            dynamic message;

            try
            {
                message = new
                {
                    id = "check/client",
                    data = new
                    {
                        name = gebruikersNaam,
                        password = wachtwoord
                    }
                };
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }

            dynamic feedback = ReadMessage(client);
            return feedback.data.ack;
        }

        public static IPAddress GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            throw new Exception("Local IP Address Not Found!");
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

            byte[] buffer = new byte[128];
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
