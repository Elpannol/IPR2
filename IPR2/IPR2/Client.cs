using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IPR2
{
    public class Client
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsDoctor { get; set; }
        public int Age { get; set; }
        public Log Log { get; set; }
        public bool isMan { get; set; }
        [JsonProperty("Trainingen")]
        public List<Training> Traingingen { get; set; }

        public Client(string name, string password)
        {
            Traingingen = new List<Training>();
            Name = name;
            Password = password;
            IsDoctor = true;
            Age = 0;
            Log = new Log($"{name} log");
            Server.DataBase.SaveClient(this);
        }

        public Client(string name, string password, int age, bool isMan)
        {
            Traingingen = new List<Training>();
            Name = name;
            Password = password;
            Age = age;
            IsDoctor = false;
            Log = new Log($"{name} log");
            this.isMan = isMan;
            Server.DataBase.SaveClient(this);
        }

        [JsonConstructor]
        public Client(string name, string password, bool isDoctor,  int age, Log log, List<Training> trainingen, bool isman)
        {
            Name = name;
            Password = password;
            IsDoctor = isDoctor;
            Log = log;
            Traingingen = trainingen;
            Age = age;
            isMan = isman;
        }

        public void AddTraining()
        {
            Traingingen.Add(new Training($"Training: {Traingingen.Count + 1}"));
        }

        public override string ToString()
        {
            string text = $"Name: {Name}\n" +
                          $"Is Doctor: {IsDoctor}" +
                          $"Age: {Age}" +
                          $"Isman: {isMan}";
            return text;
        }

        public string GetPassword()
        {
            return Password;
        }

        public dynamic getJsonData()
        {
            dynamic data = new
            {
                name = Name,
                password = Password,
                age = Age,
                isdoctor = IsDoctor,
                isman = isMan
            };
            return data;
        }

        public Training getTraining(string name)
        {
            Training training = null;
            foreach(Training t in Traingingen)
            {
                if (t._name.Equals(name))
                {
                    training = t;
                }
            }
            return training;
        }
    }
}
