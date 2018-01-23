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
                    case "check/client":
                        Console.WriteLine("Checking client");
                        if (Server.DataBase.CheckClientLogin((string) message.data.name, (string) message.data.password))
                        {
                            Console.WriteLine("good client");
                            SendAck();
                            _name = (string) message.data.name;
                            Console.WriteLine(
                                $"Client logged in: {message.data.name}");
                        }
                        else
                        {
                            Console.WriteLine("bad client");
                            SendNotAck();
                            Console.WriteLine("Client doesn't exists");
                        }
                        break;
                    case "check/doctor":
                        if (Server.DataBase.SearchForClient((string) message.data.name).IsDoctor)
                        {
                            SendAck();
                        }
                        else
                        {
                            SendNotAck();
                        }
                        break;
                    case "client/new":
                        MakeClient();
                        break;
                    case "get/age":
                        string name = message.data.name;
                        Client client = Server.DataBase.SearchForClient(name);
                        SendMessage(
                            new
                            {
                                id = "recieve/age",
                                data = new
                                {
                                    age = (int)client.Age,
                                    isman = (bool)client.isMan
                                }
                            });
                        break;
                    case "start/training":
                        Server.DataBase.SearchForClient(_name).AddTraining();
                        break;
                    case "end/training":
                        var currentTrainings = Server.DataBase.SearchForClient(_name).Traingingen;
                        var currentTraining = currentTrainings[currentTrainings.Count - 1];
                        if(currentTraining._measurements.Count == 0)
                        {
                            currentTrainings.Remove(currentTraining);
                        }
                        else
                        {
                            Server.DataBase.SaveClient(Server.DataBase.SearchForClient(_name));
                        }
                        SendAck();
                        break;
                    case "client/disconnect":
                        try
                        {
                            Console.WriteLine($"{_name} disconnected");
                            ClientSepukku();
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
                        SendAck();
                        break;
                    case "add/measurement":
                        AddMeasurementToLog(message);
                        SendAck();
                        break;
                    case "send/vo2":
                        AddVo2ToTraining(message);
                        SendAck();
                        break;
                    case "send/log":
                        SendMessage(new
                        {
                            id = "send/log",
                            data = new
                            {
                                log = Server.DataBase.SearchForClient((string) message.data.name).Log._log.ToArray()
                            }
                        });
                        break;
                    case "send/traininglog":
                        Training training = Server.DataBase.SearchForClient((string)message.data.name).getTraining((string)message.data.training);

                        SendMessage(new {
                                vo2 = training._vo2,
                                data = new
                                {
                                    log = training._measurements.Select(m => m.ToRawData()).ToArray()
                                }
                            }
                        );

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
                    case "load/user":
                        SendPatients();
                        break;
                    case "get/trainings":
                        Client trainingclient = Server.DataBase.SearchForClient((string)message.data.name);
                        SendMessage(new
                        {
                            training = trainingclient.Traingingen.Select(t => t._name).ToArray()
                        });
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
            } while (client.GetStream().DataAvailable);
            dynamic message = Encoding.Unicode.GetString(buffer, 0, totalRead);
            return message;

        }

        public void SendMessage(dynamic message)
        {
            //make sure the other end decodes with the same format!
            message = JsonConvert.SerializeObject(message);
            byte[] bytes = Encoding.Unicode.GetBytes(message);
            Client.GetStream().Write(bytes, 0, bytes.Length);
            Client.GetStream().Flush();

        }

        private void AddVo2ToTraining(dynamic message)
        {
            try
            {
                var trainingen = Server.DataBase.SearchForClient((string)message.data.name).Traingingen;
                trainingen.ElementAt(trainingen.Count - 1)._vo2 = (double)message.data.vo2;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
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
            SendMessage(
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

                c.SendMessage(new
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
            Client.GetStream().Close();
            Client.Close();
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

        private void SendAck()
        {
            bool ack = true;
            SendMessage (new
            {
                id = "ack",
                data = new
                {
                    ack = ack
                }
            });
        }

        private void SendNotAck()
        {
            bool ack = false;
            SendMessage(new
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
            List<string> patients = new List<string>();

            foreach (Client c in Server.DataBase.Clients)
            {
                if (!c.IsDoctor)
                {
                    patients.Add(c.Name);
                }
            }

            SendMessage(new
            {
                patient = patients.ToArray()
            });
        }
    }
}
