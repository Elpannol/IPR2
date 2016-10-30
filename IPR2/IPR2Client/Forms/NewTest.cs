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
    public partial class NewTest : Form
    {
        private TcpClient client;

        public NewTest(TcpClient client)
        {
            this.client = client;
            InitializeComponent();
            FormClosing += NewTest_FormClosing;
            update();
        }

        private void NewTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.GetStream().Close();
            client.Close();
            Application.Exit();
        }

        private void update()
        {
            weerstandLabel.Text = "50"      + " Watt";
            hartslagLabel.Text  = "120"     + " BPM";
            rondesLabel.Text    = "100"     + " RPM";
            tijdLabel.Text      = "01:30"   + " Minuten";
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
