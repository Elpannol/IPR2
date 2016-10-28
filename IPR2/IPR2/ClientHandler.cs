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
        private readonly TcpClient _client;
        private string _id;

        public ClientHandler(TcpClient client)
        {
            _client = client;
        }

        public void HandleClientThread()
        {
            do
            {
                dynamic message = JsonConvert.DeserializeObject(ReadMessage(_client));
                switch ((string)message.id)
                {
                    case "client/new":
                        MakeClient();
                        break;
                    case "add/logentry":
                        Server.DataBase.SearchForClient(message.data.name).Log.AddLogEntry(message.data.text);
                        break;
                    case "add/measurement":
                        break;
                    case "send/log":
                        SendMessage(SearchForID(message.data.name), new
                        {
                            id = "log/send",
                            data = new
                            {
                                log = Server.DataBase.SearchForClient(message.data.name).Log.ToString()
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
            while (_client.Connected);
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
                Console.WriteLine("ReadMessage: " + read);
            } while (client.GetStream().DataAvailable);
            string message = Encoding.Unicode.GetString(buffer, 0, totalRead);
            Console.WriteLine(message);
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
            Server.DataBase.SearchForClient(variables.data.name).Log.AddLogEntry(text);
        }

        public void MakeClient()
        {
            SendMessage(_client,
            new
            {
                id = "make/client",
                data = new
                {

                }
            });

            dynamic message = ReadMessage(_client);

                Server.DataBase.AddClient(new Client(message.data.name, message.data.password,
                    message.data.isDoctor));
                _id = message.data.id;
        }

        private void KillClient(string id)
        {
            foreach (var c in Server._handlers)
            {
                if (c._id.Equals(id))
                {
                    c._client.GetStream().Close();
                    c._client.Close();
                    Server._handlers.Remove(c);
                    //you murderer
                }
            }
        }

        private void ClientSepukku()
        {
            foreach (var c in Server._handlers)
            {
                if (c._id.Equals(_id))
                {
                    //When you dishonor the family
                    c._client.GetStream().Close();
                    c._client.Close();
                    Server._handlers.Remove(c);
                }
            }
        }

        private TcpClient SearchForID(string id)
        {
            TcpClient client = null;
            foreach (var c in Server._handlers)
            {
                if (c._id.Equals(id))
                {
                    client = c._client;
                }
            }
            return client;
        }
    }
}
