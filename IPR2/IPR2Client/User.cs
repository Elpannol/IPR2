using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR2Client.Forms
{
    public class User
    {
        public string _gebruikersnaam {get;set;}

        public User(string gebruikersnaam)
        {
            _gebruikersnaam = gebruikersnaam;
        }
    }
}
