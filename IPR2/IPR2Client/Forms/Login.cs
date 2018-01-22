using IPR2Client.Forms;
using System;
using System.Windows.Forms;

namespace IPR2Client
{
    public partial class Login : Form
    {
        public static Handler Handler;

        public Login()
        {
            Handler = new Handler();
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
            if (Handler.isConnected || Handler.waitForConnection())
            {
                if (Handler.Login(gebruikersNaam.Text, wachtwoord.Text))
                {
                    Visible = false;
                    if (Handler.IsDoctor(gebruikersNaam.Text))
                    {
                        Doctor doctor = new Doctor(gebruikersNaam.Text);
                        doctor.Visible = true;
                    }
                    else
                    {
                        Results results = new Results(gebruikersNaam.Text);
                        results.Visible = true;
                    }
                }
                else
                {
                    wrongLabel.Text = "Gebruikersnaam of wachtwoord onjuist!";
                    wrongLabel.Visible = true;

                }
            }
            else
            {
                wrongLabel.Text = "Kan geen connectie met de server maken!";
                wrongLabel.Visible = true;
            }
        }
    }
}
