using IPR2Client.Forms;
using IPR2Client.Simulation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IPR2Client
{
    public class Handler
    {
        private TcpClient Client;
        private IPAddress _currentId;
        private byte[] _messageBuffer;

        public Handler()
        {
            Client = new TcpClient();
            IPAddress localIp = GetLocalIpAddress();
            bool ipOk = IPAddress.TryParse(localIp.ToString(), out _currentId);
            if (!ipOk)
            {
                Console.WriteLine("Couldn't parse the ip address. Exiting code.");
                Environment.Exit(1);
            }

            Client.Connect(_currentId, 1337);
        }

        public IPAddress GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            throw new Exception("Local IP Address Not Found!");
        }

        public dynamic ReadMessage(TcpClient client)
        {
            byte[] buffer = new byte[1024];
            var numberOfBytesRead = Client.GetStream().Read(buffer, 0, buffer.Length);
            _messageBuffer = ConCat(_messageBuffer, buffer, numberOfBytesRead);

            if (_messageBuffer.Length <= 4) return null;
            var packetLegth = BitConverter.ToInt32(_messageBuffer, 0);

            if (_messageBuffer.Length < packetLegth + 4) return null;
            var resultMessage = GetMessageFromBuffer(_messageBuffer, packetLegth);
            return resultMessage;
        }

        public void SendMessage(TcpClient client, dynamic message)
        {
            //make sure the other end decodes with the same format!
            message = JsonConvert.SerializeObject(message);
            var buffer = Encoding.Default.GetBytes(message);
            var bufferPrepend = BitConverter.GetBytes(buffer.length);

            client.GetStream().Write(bufferPrepend, 0, bufferPrepend.Length);
            client.GetStream().Write(buffer, 0, buffer.length);
            client.GetStream().Flush();
        }

        public void Disconnect()
        {
            try
            {
                dynamic message = new
                {
                    id = "client/disconnect",
                    data = new
                    {

                    }
                };

                SendMessage(Client, message);


                Client.GetStream().Close();
                Client.Close();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }

        public bool Login(string gebruikersnaam, string wachtwoord)
        {
            try
            {
                dynamic message = new
                {
                    id = "check/client",
                    data = new
                    {
                        name = gebruikersnaam,
                        password = wachtwoord
                    }
                };
                SendMessage(Client, message);
                dynamic feedback = JsonConvert.DeserializeObject(ReadMessage(Client));
                return feedback.data.ack;

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return false;
            }

        }

        public bool IsDoctor(string gebruikersnaam)
        {
            try
            {
                dynamic message = new
                {
                    id = "check/doctor",
                    data = new
                    {
                        name = gebruikersnaam,
                    }
                };
                SendMessage(Client, message);
                dynamic feedback = JsonConvert.DeserializeObject(ReadMessage(Client));
                return feedback.data.ack;

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return false;
            }
        }

        public void AddLogEntry(string text, string _name)
        {
            dynamic message = new
            {
                id = "add/logentry",
                data = new
                {
                    text = text,
                    name = _name
                }
            };

            SendMessage(Client, message);
        }

        public void StartTraining()
        {
            SendMessage(Client, new
            {
                id = "start/training"
            });
        }

        public void AddMeasurement(Measurement measurement, string _name)
        {
            dynamic message = new
            {
                id = "add/measurement",
                data = new
                {
                    weerstand = measurement.Weerstand,
                    hartslag = measurement.Hartslag,
                    rondes = measurement.Rondes,
                    timeM = measurement.Time.Minutes,
                    timeS = measurement.Time.Seconds,
                    name = _name
                }
            };

            SendMessage(Client, message);
        }

        public List<Training> GetTrainingen(string _name)
        {
            try
            {
                dynamic message = new
                {
                    id = "load/training",
                    data = new
                    {
                        name = _name
                    }
                };

                SendMessage(Client, message);

                dynamic feedback = JsonConvert.DeserializeObject(ReadMessage(Client));
                return feedback.data.trainingen;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return new List<Training>();
            }
        }

        public List<User> GetUsers()
        {
            try
            {
                dynamic message = new
                {
                    id = "load/user",
                    data = new
                    {
                    }
                };

                SendMessage(Client, message);

                dynamic feedback = JsonConvert.DeserializeObject(ReadMessage(Client));
                return (List<User>)feedback.data.users;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return new List<User>();
            }
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
    }
}
