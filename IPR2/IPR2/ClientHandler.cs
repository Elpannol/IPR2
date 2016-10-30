using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IPR2
{
    class ClientHandler
    {
        public TcpClient Client { get; set; }
        private string _name;

        public ClientHandler(TcpClient client)
        {
            Client = client;
        }

        public void HandleClientThread()
        {
            do
            {
                dynamic message = JsonConvert.DeserializeObject(ReadMessage(Client));
                switch ((string)message.id)
                {
                    case "check/client":
                        if (Server.DataBase.CheckClientLogin(message.data.name, message.data.password))
                        {
                            SendAck(Client);
                        }
                        else
                        {
                            SendNotAck(Client);
                        }
                        break;
                    case "client/new":
                        MakeClient();
                        break;
                    case "client/disconnect":
                        try
                        {
                            Client.GetStream().Close();
                            Client.Close();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.StackTrace);
                        }
                        break;
                    case "send/clients":
                        SendPatients();
                        break;
                    case "add/logentry":
                        Server.DataBase.SearchForClient(message.data.name).Log.AddLogEntry(message.data.text);
                        break;
                    case "add/measurement":
                        AddMeasurementToLog(message);
                        break;
                    case "send/log":
                        SendMessage(SearchForName(message.data.name), new
                        {
                            id = "log/send",
                            data = new
                            {
                                log = Server.DataBase.SearchForClient(message.data.name).Log
                            }
                        });
                        break;
                    case "kill/client":
                        if (Server.DataBase.SearchForClient(message.data.name).IsDoctor)
                        {
                            Server.DataBase.DeleteClient(message.data.name);
                            KillClient(message.data.name);
                        }
                        break;
                    case "commit/sepukku":
                        if (Server.DataBase.SearchForClient(message.data.name).IsDoctor)
                        {
                            Server.DataBase.DeleteClient(message.data.name);
                            ClientSepukku();
                        }
                        break;
                    default:
                        Console.WriteLine("You're not suppose to be here");
                        break;
                }
            }
            while (Client.Connected);
        }

        //TODO: both read and send message need to work with Json
        public dynamic ReadMessage(TcpClient client)
        {

            byte[] buffer = new byte[128];
            int totalRead = 0;

            //read bytes until stream indicates there are no more
            do
            {
                int read = client.GetStream().Read(buffer, totalRead, buffer.Length - totalRead);
                totalRead += read;
            } while (client.GetStream().DataAvailable);
            string message = Encoding.Unicode.GetString(buffer, 0, totalRead);
            return message;
        }

        public void SendMessage(TcpClient client, dynamic message)
        {
            //make sure the other end decodes with the same format!
            message = JsonConvert.SerializeObject(message);
            byte[] bytes = Encoding.Unicode.GetBytes(message);
            client.GetStream().Write(bytes, 0, bytes.Length);
            client.GetStream().Flush();
        }

        private void AddMeasurementToLog(dynamic variables)
        {
            //TODO: needs to work for measurements
            string text = variables;
            Server.DataBase.SearchForClient(variables.data.measurement.ToString()).Log.AddLogEntry(text);
        }

        public void MakeClient()
        {
            SendMessage(Client,
            new
            {
                id = "make/client",
                data = new
                {

                }
            });

            dynamic message = ReadMessage(Client);
            Server.DataBase.AddClient(new Client(message.data.name, message.data.password,
                message.data.isDoctor));
            _name = message.data.name;
        }

        private static void KillClient(string id)
        {
            ClientHandler client = null;
            foreach (var c in Server.Handlers)
            {
                if (!c._name.Equals(id)) continue;

                c.SendMessage(c.Client, new
                {
                    id = "client/close",
                    data = new
                    {
                        
                    }
                });

                c.Client.GetStream().Close();
                c.Client.Close();
                client = c;
                //you murderer
            }
            Server.Handlers.Remove(client);
        }

        private void ClientSepukku()
        {
            //When you dishonor the family
            foreach (var c in Server.Handlers)
            {
                if (!c._name.Equals(_name) || !c.Client.Connected) continue;
                c.Client.GetStream().Close();
                c.Client.Close();
            }
            Server.Handlers.Remove(this);
        }

        private static TcpClient SearchForName(string name)
        {
            TcpClient client = null;
            foreach (var c in Server.Handlers)
            {
                if (c._name.Equals(name))
                {
                    client = c.Client;
                }
            }
            return client;
        }

        private void SendAck(TcpClient client)
        {
            SendMessage(client, new
            {
                id = "ack",
                data = new
                {
                    ack = true
                }
            });
        }

        private void SendNotAck(TcpClient client)
        {
            SendMessage(client, new
            {
                id = "ack",
                data = new
                {
                    ack = false
                }
            });
        }

        private void SendPatients()
        {
            SendMessage(Client, new
            {
                id = "patient/send",
                data = new
                {
                    patients = Server.DataBase.Clients.ToArray()
                }
            });
        }
    }
}
