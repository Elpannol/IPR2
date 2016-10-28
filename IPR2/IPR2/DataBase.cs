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
        public static List<Client> Clients { get; set; }

        public DataBase()
        {
            Clients =  new List<Client>();
        }

        public  void AddClient(Client client)
        {
            Clients.Add(client);
        }

        public Client SearchForClient(string id)
        {
            Client client = null;
            foreach (var c in Clients)
            {
                if (c.ID.Equals(id))
                {
                    client = c;
                }
            }

            return client;
        }

        public void DeleteClient(string id)
        {
            foreach (var c in Clients)
            {
                if (c.ID.Equals(id))
                {
                    Clients.Remove(c);
                }
            }
        }

    }
}
