using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IPR2
{
    class Server
    {
        public static DataBase DataBase { get; set; }
        public static List<ClientHandler> Handlers { get; set; }
        public List<Thread> Threads = new List<Thread>();

        private readonly TcpListener _listener;
        private IPAddress _currentId;

        public Server()
        {
            DataBase = new DataBase();
            IPAddress localIp = GetLocalIpAddress();
            Handlers = new List<ClientHandler>();

            bool ipOk = IPAddress.TryParse(localIp.ToString(), out _currentId);
            if (!ipOk)
            {
                Console.WriteLine("Couldn't parse the ip address. Exiting code.");
                Environment.Exit(1);
            }
            _listener = new TcpListener(_currentId, 1337);

        }

        public void Run()
        {
            _listener.Start();

            //making client handlers and adding them to the list
            while (true)
            {
                ClientHandler handler = new ClientHandler(CheckForClients(_listener));
                Thread thread = new Thread(handler.HandleClientThread);
                thread.Start();
                Threads.Add(thread);
                Handlers.Add(handler);
            }
        }

        private TcpClient CheckForClients(TcpListener listner)
        {
            Console.WriteLine("Waiting for clients");
            TcpClient client = listner.AcceptTcpClient();
            Console.WriteLine("Client connected!!");

            return client;
        }

        public static IPAddress GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            throw new Exception("Local IP Address Not Found!");
        }

        public void KillAllClient()
        {
            Console.WriteLine("Genocide is an option");
            foreach (var c in Handlers)
            {
                if(!c.Client.Connected) continue;
                c.Client.GetStream().Close();
                c.Client.Close();
            }
            Handlers.Clear();


        }
    }
}
