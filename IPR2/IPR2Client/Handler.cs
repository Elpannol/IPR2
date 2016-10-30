﻿using IPR2Client.Forms;
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
    }
}
