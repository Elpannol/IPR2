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
        public bool isConnected = false;

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
        }

        public bool waitForConnection()
        {
            try{
                Client.Connect(_currentId, 1337);
                isConnected = true;
                return isConnected;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        public IPAddress GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            throw new Exception("Local IP Address Not Found!");
        }

        public dynamic ReadMessage()
        {
            byte[] buffer = new byte[32768];
            int totalRead = 0;
             do
             {
                int read = Client.GetStream().Read(buffer, totalRead, buffer.Length - totalRead);
                totalRead += read;
                if(read > 0)
                {
                    Console.WriteLine("ReadMessage: " + read);
                }
             } while (Client.GetStream().DataAvailable);
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

                SendMessage(message);

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
                SendMessage(message);
                dynamic feedback = JsonConvert.DeserializeObject(ReadMessage());
                var ack = feedback.data.ack;
                return ack;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
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
                SendMessage(message);
                dynamic feedback = JsonConvert.DeserializeObject(ReadMessage());
                var ack = feedback.data.ack;
                return ack;

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return false;
            }
        }

        public dynamic getLog(string name, string training)
        {
            try
            {
                SendMessage(new
                {
                    id = "send/traininglog",
                    data = new
                    {
                        name = name,
                        training = training
                    }
                });

                dynamic message = JsonConvert.DeserializeObject(ReadMessage());
                return message;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return new List<string>();
            }
        }

        public dynamic GetAge(string gebruikersnaam)
        {
            try
            {
                dynamic message = new
                {
                    id = "get/age",
                    data = new
                    {
                        name = gebruikersnaam,
                    }
                };
                SendMessage(message);
                return JsonConvert.DeserializeObject(ReadMessage());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return -1;
            }
        }

        public void SendVo2(string clientName ,double vo2)
        {
            try
            {
                dynamic message = new
                {
                    id = "send/vo2",
                    data = new
                    {
                        vo2 = vo2,
                        name = clientName
                    }
                };
                SendMessage(message);
                ReadMessage();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }
    

        public void AddLogEntry(string text, string name)
        {
            dynamic message = new
            {
                id = "add/logentry",
                data = new
                {
                    text = text,
                    name = name
                }
            };

            SendMessage(message);
        }

        public void StartTraining()
        {
            SendMessage(new
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
                    name = _name,
                    weerstand = measurement.Weerstand,
                    hartslag = measurement.Hartslag,
                    rondes = measurement.Rondes,
                    timeM = measurement.Time.Minutes,
                    timeS = measurement.Time.Seconds
                }
            };

            SendMessage(message);
        }

        public List<string> GetTrainingen(string name)
        {
            try
            {
                dynamic message = new
                {
                    id = "get/trainings",
                    data = new
                    {
                        name = name
                    }
                };

                SendMessage(message);

                dynamic readmessage = JsonConvert.DeserializeObject(ReadMessage());
                List<string> trainingnames = new List<string>();
                for (int i = 0; i < readmessage.training.Count; i++)
                {
                    trainingnames.Add((string)readmessage.training[i]);
                }
                return trainingnames;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return new List<string>();
            }
        }

        public void EndTraining()
        {
            try
            {
                dynamic message = new
                {
                    id = "end/training"
                };

                SendMessage(message);
                ReadMessage();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }

        public List<string> GetUsers()
        {
            try
            {
                dynamic message = new
                {
                    id = "load/user"
                };

                SendMessage(message);

                dynamic readmessage = JsonConvert.DeserializeObject(ReadMessage());
                List<string> patientnames = new List<string>();
                for(int i = 0; i < readmessage.patient.Count; i++)
                {
                    patientnames.Add((string)readmessage.patient[i]);
                }
                return patientnames;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
                return new List<string>();
            }
        }
    }
}
