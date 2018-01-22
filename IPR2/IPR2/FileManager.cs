using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR2
{
    public class FileManager
    {
        public String _filePath { get; set; }
        public FileManager()
        {
            _filePath = @"..\..\PatientData\";
        }

        public void SaveTraining(Client client)
        {
            WriteToJsonFile<Client>(client);
        }

        private void WriteToJsonFile<T>(Client client)
        {
            StreamWriter writer = null;
            String filePath = "";

            filePath = $"{_filePath}/{client.Name}.json";
            if (!File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            
            writer = File.CreateText(filePath);
            try
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, client);
            }
            finally
            {
                writer?.Close();
            }
        }

        public List<Client> LoadAllClients()
        {
            TextReader reader = null;

            var clients = new List<Client>();
            if (File.Exists(_filePath)) return clients;
            try
            {
                string[] paths = Directory.GetFiles(_filePath);

                foreach(string s in paths)
                {
                    reader = new StreamReader(s);
                    var fileContent = reader.ReadToEnd();
                    var client = JsonConvert.DeserializeObject<Client>(fileContent);
                    clients.Add(client);
                }
            }
            finally
            {
                reader?.Close();
            }
            return clients;
        }
    }


}
