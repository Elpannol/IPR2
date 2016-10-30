using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IPR2
{
    public class DataBase
    {
        public List<Client> Clients { get; set; }

        public DataBase()
        {
            Clients =  new List<Client>();
        }

        public  void AddClient(Client client)
        {
            Clients.Add(client);
        }

        public Client SearchForClient(string name)
        {
            Client client = null;
            foreach (var c in Clients)
            {
                if (c.Name.Equals(name))
                {
                    client = c;
                }
            }

            return client;
        }

        public bool CheckClient(string name)
        {
            bool isClient = false;
            foreach (var c in Clients)
            {
                if (c.Name.Equals(name))
                {
                    isClient = true;
                }
            }
            return isClient;
        }
        public void DeleteClient(string name)
        {
            Client client = null;
            foreach (var c in Clients)
            {
                if (c.Name.Equals(name))
                {
                    client = c;
                }
            }
            Clients.Remove(client);
        }

        public bool CheckClientLogin(string name, string password)
        {
            bool isClient = false;
            foreach (var c in Clients)
            {
                if (c.Name.Equals(name) && c.GetPassword().Equals(password))
                {
                    isClient = true;
                }
            }
            return isClient;
        }
    }
}
