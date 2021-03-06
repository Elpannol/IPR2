﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IPR2
{
    public class DataBase
    {
        public List<Client> Clients { get; set; }
        public FileManager _fileManager { get; set; }

        public DataBase()
        {
            _fileManager = new FileManager();
            Clients = _fileManager.LoadAllClients();
        }

        public  void AddClient(Client client)
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

        public List<Client> GetAllDoctors()
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

        public List<Client> GetAllPatients()
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

        public void SaveClient(Client client)
        {
            _fileManager.SaveTraining(client);
        }
    }
}
