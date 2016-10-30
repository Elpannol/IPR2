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
        private byte[] _messageBuffer;
        public NetworkStream NetworkStream { get; set; }

        public ClientHandler(TcpClient client)
        {
            Client = client;
            NetworkStream = Client.GetStream();
            _messageBuffer = new byte[0];
        }

        public void HandleClientThread()
        {
            byte[] buffer = new byte[1024];
            do
            {
                var numberOfBytesRead = Client.GetStream().Read(buffer, 0, buffer.Length);
                _messageBuffer = ConCat(_messageBuffer, buffer, numberOfBytesRead);

                if(_messageBuffer.Length <= 4) continue;
                var packetLegth = BitConverter.ToInt32(_messageBuffer, 0);

                if(_messageBuffer.Length < packetLegth + 4) continue;
                var resultMessage = GetMessageFromBuffer(_messageBuffer, packetLegth);
                dynamic message = JsonConvert.DeserializeObject(resultMessage);
                switch ((string) message.id)
                {
                    case "check/client":
                        if (Server.DataBase.CheckClientLogin((string) message.data.name, (string) message.data.password))
                        {
                            SendAck(Client);
                            _name = (string) message.data.name;
                            Console.WriteLine(
                                $"Client logged in: {message.data.name}");
                        }
                        else
                        {
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
                        break;
                    case "add/measurement":
                        AddMeasurementToLog(message);
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

            byte[] buffer = new byte[1024];

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
            var buffer = Encoding.Default.GetBytes(message);
            var bufferPrepend = BitConverter.GetBytes(buffer.length);

            client.GetStream().Write(bufferPrepend, 0, bufferPrepend.Length);
            client.GetStream().Write(buffer, 0, buffer.length);
        }

        private string GetMessageFromBuffer(byte[] array, int count)
        {
            var newArray = new byte[array.Length - (count + 4)];

            var message = new StringBuilder();
            message.AppendFormat("{0}", Encoding.ASCII.GetString(array, 4, count));

            for (var i = 0; i < newArray.Length; i++)
            {
                newArray[i] = array[i + count + 4];
            }

            return message.ToString();
        }

        public byte[] ConCat(byte[] arrayOne, byte[] arrayTwo, int count)
        {
            var newArray = new byte[arrayOne.Length + count];
            Buffer.BlockCopy(arrayOne, 0, newArray, 0, arrayOne.Length);
            Buffer.BlockCopy(arrayTwo, 0, newArray, arrayOne.Length, count);
            return newArray;
        }

        public byte[] SubArray(byte[] data, int index, int length)
        {
            var result = new byte[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public byte[] NotConCat(byte[] array, int count)
        {
            var tempArray = new byte[array.Length - count];
            for (var i = 0; i < array.Length - count; i++)
            {
                tempArray[i] = array[count + i];
            }
            return tempArray;
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

            dynamic message = ReadMessage(Client);
            Server.DataBase.AddClient(new Client((string)message.data.name, (string)message.data.password,
                (bool)message.data.isDoctor));
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
