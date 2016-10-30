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

        public Results(TcpClient client)
        {
            this.client = client;
            InitializeComponent();
            FormClosing += Results_FormClosing;
        }

        private void Results_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.GetStream().Close();
            client.Close();
            Application.Exit();
        }

        private void newTestButton_Click(object sender, EventArgs e)
        {
            Visible = false;
            NewTest newTest = new NewTest(client);
            newTest.Visible = true;
            Simulator simulator = new Simulator(client);
            simulator.Visible = true;
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
