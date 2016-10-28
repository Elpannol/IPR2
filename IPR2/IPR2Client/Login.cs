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
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if(tryLogin(gebruikersNaam.Text, wachtwoord.Text))
            {

            }
            else
            wrongLabel.Visible = true;
        }

        private bool tryLogin(string gebruikersnaam, string wachtwoord)
        {
            return false;
        }
    }
}
