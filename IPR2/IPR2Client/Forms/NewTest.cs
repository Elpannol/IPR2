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
        private Simulator simulator;
        private Results results;

        public NewTest(TcpClient client, string name, Results results)
        {
            this.results = results;
            this.client = client;
            InitializeComponent();
            FormClosing += NewTest_FormClosing;

            simulator = new Simulator(client, this, name, results);
            simulator.Visible = true;
        }

        private void NewTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            results.Visible = true;
            simulator.stop();
            simulator.Dispose();
            this.Dispose();
        }

        public void update(string weerstand, string hartslag, string rondes, string tijd)
        {
            weerstandLabel.Text = weerstand    + " Watt";
            hartslagLabel.Text  = hartslag     + " BPM";
            rondesLabel.Text    = rondes       + " RPM";
            tijdLabel.Text      = tijd         + " Minuten";
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
