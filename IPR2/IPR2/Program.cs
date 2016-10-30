using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IPR2
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            Server server = new Server();

            var serverThread = new Thread(server.Run);
            serverThread.Start();

            Console.WriteLine("type 'help' to show available commands.");
            ConsoleLoop();

            foreach (var t in server.Threads)
            {
                t.Interrupt();
                t.Abort();
            }

            server.KillAllClient();

            //murder the server
            System.Environment.Exit(1);

        }

        private void ConsoleLoop()
        {
            while (true)
            {
                switch (Console.ReadLine()?.ToLower())
                {
                    case "exit":
                        return;
                    case "newpatient":
                        CreateUser(false);
                        break;
                    case "newdoctor":
                        CreateUser(true);
                        break;
                    case "deleteuser":
                        DeleteUser();
                        break;
                    case "help":
                        Console.WriteLine("\ncommands :" +
                                          "\n- exit" +
                                          "\n- newpatient" +
                                          "\n- newdoctor" +
                                          "\n- deleteuser" +
                                          "\n- help" +
                                          "\n- showclients");
                        break;
                    case "showclients":
                        ShowClients();
                        break;
                    default:
                        Console.WriteLine("Command not recognised....");
                        break;
                }
            }
        }

        private void CreateUser(bool isdoctor)
        {
            Console.WriteLine("new user...\n" + "enter name:");
            string name = Console.ReadLine();
               
            if (Server.DataBase.CheckClient(name))
            {
                while (true)
                {
                    Console.WriteLine("Account already exist\n"
                                      + "Continue? [y/n]");
                    var readLine = Console.ReadLine();
                    if (readLine == null) continue;
                    var answer = readLine.ToLower();
                    if (answer.Equals("y"))
                    {
                        CreateUser(isdoctor);
                    }
                    else if (answer.Equals("n"))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Command not recognised");
                        continue;
                    }
                }
            }

            else
            {
                Console.WriteLine("enter password...");
                var password = Console.ReadLine();
                Server.DataBase.AddClient(new Client(name,password,isdoctor));
            }
                        
                    
        }

        private void DeleteUser()
        {
            
            Console.WriteLine("give the name of the target");
            var targetName = Console.ReadLine();
            if (Server.DataBase.CheckClient(targetName))
            {
                Server.DataBase.DeleteClient(targetName);
            }
            else
            {
                while (true)
                {
                    Console.WriteLine("target can't be found\n" +
                                      "do you want to search for another target? [y/n]");
                    var readLine = Console.ReadLine();
                    if (readLine != null)
                    {
                        var answer = readLine.ToLower();
                        if (answer.Equals("y"))
                        {
                            DeleteUser();
                        }
                        else if (answer.Equals("n"))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Command not recognised");
                            continue;
                        }
                    }
                }
            }
        }

        private void ShowClients()
        {
            if (Server.DataBase.Clients.Count == 0)
            {
                Console.WriteLine("There are no clients to show");
                return;
            }
            foreach (var c in Server.DataBase.Clients)
            {
                Console.WriteLine(c.ToString());
            }
        }
    }

}
