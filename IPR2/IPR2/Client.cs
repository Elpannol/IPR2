using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IPR2
{
    public class Client
    {
        public string Name { get; set; }
        private string _password { get; }
        public bool IsDoctor { get; set; }
        public Log Log { get; set; }

        public Client(string name, string password, bool isDoctor)
        {
            Name = name;
            _password = password;
            IsDoctor = isDoctor;
            Log = new Log($"{name} log");
        }

        public override string ToString()
        {
            string text = $"Name: {Name}";
                          
            return text;
        }

        public string GetPassword()
        {
            return _password;
        }
    }
}
