using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IPR2
{
    class Client
    {
        public string Name { get; set; }
        readonly string Password;
        public int ID { get; set; }
        public bool IsDoctor { get; set; }
        public Log Log { get; set; }

        public Client(string name, string password, int Id, bool isDoctor, Log log)
        {
            Name = name;
            Password = password;
            ID = Id;
            IsDoctor = isDoctor;
        }
    }
}
