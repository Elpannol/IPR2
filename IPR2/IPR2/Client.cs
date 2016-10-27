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
        public TcpClient _Client { get; set; }
        public string Name { get; set; }
        public int ID { get; set; }
        public bool IsDoctor { get; set; }
        public Client(TcpClient client, string name, int Id, bool isDoctor)
        {
            _Client = client;
            Name = name;
            ID = Id;
            IsDoctor = isDoctor;
        }
    }
}
