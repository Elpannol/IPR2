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
        private string _password { get; }
        public bool IsDoctor { get; set; }
        public Log Log { get; set; }
        public List<Training> Traingingen { get; set; }
        public int _age { get; set; }

        public Client(string name, string password)
        {
            Traingingen = new List<Training>();
            Name = name;
            _password = password;
            IsDoctor = true;
            _age = 0;
            Log = new Log($"{name} log");
        }

        public Client(string name, string password, int age)
        {
            Traingingen = new List<Training>();
            Name = name;
            _password = password;
            _age = age;
            IsDoctor = false;
            Log = new Log($"{name} log");
       
        }

        public void AddTraining()
        {
            Traingingen.Add(new Training("Training" + Traingingen.Count + 1));
        }

        public override string ToString()
        {
            string text = $"Name: {Name}\n" +
                          $"Is Doctor: {IsDoctor}" +
                          $"Age: {_age}";
            return text;
        }

        public string GetPassword()
        {
            return _password;
        }

        public dynamic getJsonData()
        {
            dynamic data = new
            {
                name = Name,
                password = _password,
                age = _age,
                isdoctor = IsDoctor,
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
