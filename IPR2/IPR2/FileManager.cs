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

        public void SaveTraining(List<Client> clients)
        {
            WriteToJsonFile<Client>(clients);
        }

        public void LoadTraining(List<Client> clients)
        {
            ReadFromJson(clients);
        }

        private void WriteToJsonFile<T>(List<Client> clients, bool append = true)
        {
            TextWriter writer = null;
            String filePath = "";
            foreach (Client c in clients)
            {
                filePath = $"{_filePath}/{c.Name}.json";
                if (!File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                File.Create(filePath);
                try
                {
                    var contentsToWriteToFile = JsonConvert.SerializeObject(c);
                    writer = new StreamWriter(filePath, append);
                    writer.WriteLine(contentsToWriteToFile);
                }
                finally
                {
                    writer?.Close();
                }
            }

        }

        private void ReadFromJson(List<Client> clients)
        {
            TextReader reader = null;
            if (File.Exists(_filePath)) return;
            try
            {
                reader = new StreamReader(_filePath);
                var fileContent = reader.ReadToEnd();
                var c = JsonConvert.DeserializeObject<List<Client>>(fileContent);
                foreach (var toAdd in c)
                {
                    if (!clients.Contains(toAdd))
                    {
                        clients.AddRange(c);
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
