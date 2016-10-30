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
        public static Handler Handler;

        public Login()
        {
            Handler = new IPR2Client.Handler();
            InitializeComponent();
            wrongLabel.Visible = false;
            FormClosing += Login_FormClosing;
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Handler.Disconnect();
            Application.Exit();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if(Handler.Login(gebruikersNaam.Text, wachtwoord.Text))
            {
                Visible = false;
                if (Handler.IsDoctor(gebruikersNaam.Text))
                {
                    Doctor doctor = new Forms.Doctor(gebruikersNaam.Text);
                    doctor.Visible = true;
                }
                else
                {
                    Results results = new Results(gebruikersNaam.Text);
                    results.Visible = true;
                }
            }
            else
            wrongLabel.Visible = true;
        }
    }
}
