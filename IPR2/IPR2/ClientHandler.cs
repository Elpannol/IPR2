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
                switch ((string) message.id)
                {
                    case "create/client":
                        Server.DataBase.AddClient(new Client((string)message.data.name, (string)message.data.password,
                            (bool)message.data.isDoctor));
                        _name = message.data.name;
                        break;
                    case "check/client":
                        Console.WriteLine("Checking client");
                        if (Server.DataBase.CheckClientLogin((string) message.data.name, (string) message.data.password))
                        {
                            Console.WriteLine("good client");
                            SendAck(Client);
                            _name = (string) message.data.name;
                            Console.WriteLine(
                                $"Client logged in: {message.data.name}");
                        }
                        else
                        {
                            Console.WriteLine("bad client");
                            SendNotAck(Client);
                            Console.WriteLine("Client doesn't exists");
                        }
                        break;
                    case "check/doctor":
                        if (Server.DataBase.SearchForClient((string) message.data.name).IsDoctor)
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
                    case "start/training":
                        Server.DataBase.SearchForClient(_name).AddTraining();
                        break;
                         
                    case "client/disconnect":
                        try
                        {
                            Console.WriteLine($"{_name} disconnected");
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
                        Server.DataBase.SearchForClient((string) message.data.name)
                            .Log.AddLogEntry((string) message.data.text);
                        SendAck(Client);
                        break;
                    case "add/measurement":
                        AddMeasurementToLog(message);
                        SendAck(Client);
                        break;
                    case "send/log":
                        SendMessage(SearchForName((string) message.data.name), new
                        {
                            id = "send/log",
                            data = new
                            {
                                log = Server.DataBase.SearchForClient((string) message.data.name).Log._log.ToArray()
                            }
                        });
                        break;
                    case "kill/client":
                        if (Server.DataBase.SearchForClient(_name).IsDoctor)
                        {
                            Server.DataBase.DeleteClient((string) message.data.name);
                            KillClient((string) message.data.name);
                        }
                        break;
                    case "commit/sepukku":
                        Server.DataBase.DeleteClient(_name);
                        ClientSepukku();
                        break;
                    case "save/training":
                        Server.DataBase.SaveTraining(message);
                        break;
                    case "load/training":
                        Server.DataBase.LoadTraining(message);
                        break;
                    default:
                        Console.WriteLine("You're not suppose to be here");
                        break;
                }
            } while (Client.Connected);
        }

        public dynamic ReadMessage(TcpClient client)
        {
            byte[] buffer = new byte[1028];
            int totalRead = 0;
            do
            {
                int read = client.GetStream().Read(buffer, totalRead, buffer.Length - totalRead);
                totalRead += read;
                Console.WriteLine("ReadMessage: " + read);
            } while (client.GetStream().DataAvailable);
            dynamic message = Encoding.Unicode.GetString(buffer, 0, totalRead);
            return message;

        }

        public void SendMessage(TcpClient client, dynamic message)
        {
            //make sure the other end decodes with the same format!
            message = JsonConvert.SerializeObject(message);
            byte[] bytes = Encoding.Unicode.GetBytes(message);
            client.GetStream().Write(bytes, 0, bytes.Length);
            Client.GetStream().Flush();

        }

        private void AddMeasurementToLog(dynamic variables)
        {
            try
            {
                var tempMes = new Measurement(
                    (int)variables.data.weerstand,
                    (int)variables.data.hartslag,
                    (int)variables.data.rondes,
                    (int)variables.data.timeM,
                    (int)variables.data.timeS);
                var trainingen = Server.DataBase.SearchForClient((string) variables.data.name).Traingingen;
                trainingen.ElementAt(trainingen.Count-1)._measurements.Add(tempMes);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
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
            bool ack = true;
            SendMessage(client, new
            {
                id = "ack",
                data = new
                {
                    ack = ack
                }
            });
        }

        private void SendNotAck(TcpClient client)
        {
            bool ack = false;
            SendMessage(client, new
            {
                id = "ack",
                data = new
                {
                    ack = ack
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
