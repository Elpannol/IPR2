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
        private TcpClient _client;
        private Client _realClient;

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
                        break;
                    case "add/logentry":
                        break;
                    case "add/measurement":
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
            string text = "";
            _realClient.Log.AddLogEntry(text);
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
            _realClient = new Client(message.data.name, message.data.password, message.data.id, message.data.isDoctor, new Log($"{message.data.name} log"));
        }
    }
}
