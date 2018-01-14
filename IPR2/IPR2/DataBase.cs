using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace IPR2
{
    public class DataBase
    {
        public List<Client> Clients { get; set; }

        public DataBase()
        {
            Clients =  new List<Client>();
        }

        public void AddClient(Client client)
        {
            Clients.Add(client);
        }

        public Client SearchForClient(string name)
        {
            Client client = null;
            foreach (var c in Clients)
            {
                if (c.Name.Equals(name))
                {
                    client = c;
                }
            }

            return client;
        }

        public bool CheckClient(string name)
        {
            bool isClient = false;
            foreach (var c in Clients)
            {
                if (c.Name.Equals(name))
                {
                    isClient = true;
                }
            }
            return isClient;
        }
        public void DeleteClient(string name)
        {
            Client client = null;
            foreach (var c in Clients)
            {
                if (c.Name.Equals(name))
                {
                    client = c;
                }
            }
            Clients.Remove(client);
        }

        public bool CheckClientLogin(string name, string password)
        {
            bool isClient = false;
            foreach (var c in Clients)
            {
                if (c.Name.Equals(name) && c.GetPassword().Equals(password))
                {
                    isClient = true;
                }
            }
            return isClient;
        }

        private string SetFilePath()
        {
            return @"..\..\PatientData\clients.save";
        }

        public void SaveTraining(dynamic message)
        {
            WriteToJsonFile<Client>(SetFilePath(), Clients);
        }

        public void LoadTraining(dynamic message)
        {
            ReadFromJson(SetFilePath());
        }

        private static void WriteToJsonFile<T>(string filePath, List<Client> clients, bool append = true)
        {
            TextWriter writer = null;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                try
                {
                    var contentsToWriteToFile = JsonConvert.SerializeObject(clients);
                    writer = new StreamWriter(filePath, append);
                    writer.WriteLine(contentsToWriteToFile);
                }
                finally
                {
                    writer?.Close();
                }
            }
            else
            {
                try
                {
                    var contentsToWriteToFile = JsonConvert.SerializeObject(clients);
                    writer = new StreamWriter(filePath, append);
                    writer.WriteLine(contentsToWriteToFile);
                }
                finally
                {
                    writer?.Close();
                }
            }
        }

        public List<Client> GiveAllDoctors()
        {
            List<Client> doctors = new List<Client>();
            foreach (var c in Clients)
            {
                if (c.IsDoctor)
                {
                    doctors.Add(c);
                }
            }
            return doctors;
        }

        public List<Client> GiveAllPatients()
        {
            List<Client> patients = new List<Client>();
            foreach (var c in Clients)
            {
                if (c.IsDoctor)
                {
                    patients.Add(c);
                }
            }
            return patients;
        }

        private void ReadFromJson(string filePath)
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContent = reader.ReadToEnd();
                var c = JsonConvert.DeserializeObject<List<Client>>(fileContent);
                foreach (var toAdd in c)
                {
                    if (!Clients.Contains(toAdd))
                    {
                        Clients.AddRange(c);
                    }
                }
            }
            finally
            {
                reader?.Close();
            }
        }
    }
}
