using IPR2Client.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPR2Client
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            wrongLabel.Visible = false;
            FormClosing += Login_FormClosing;
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if(tryLogin(gebruikersNaam.Text, wachtwoord.Text))
            {
                Visible = false;
                Results results = new Forms.Results();
                results.Visible = true;
            }
            else
            wrongLabel.Visible = true;
        }

        private bool tryLogin(string gebruikersnaam, string wachtwoord)
        {
            return true;
        }
    }
}
