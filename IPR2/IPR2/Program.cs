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
            server.KillAllClient();
            serverThread.Interrupt();
            serverThread.Abort();            

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

                if (isdoctor)
                {
                    Server.DataBase.AddClient(new Client(name, password));
                    return;
                }
                Console.WriteLine("enter age...");
                string textAge = Console.ReadLine();
                int age = 0;
                while (true)
                {
                    if (int.TryParse(textAge, out age)) break;
                    else
                    {
                        Console.Write("Age must be a number");
                        Console.Write("Continue? [y/n]");
                        string answer = Console.ReadLine();
                        if (answer.Equals("y"))
                        {
                            textAge = Console.ReadLine();
                            continue;
                        }
                        else if (answer.Equals("n"))
                        {
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Command not recognised");
                            continue;
                        }
                    }
                
                }
                bool isman = true;
                Console.WriteLine("Are you a man or a woman? [m/w]");
                string textMan = Console.ReadLine();
                while (true)
                {
                    if (textMan.ToLower().Equals("m") || textMan.ToLower().Equals("man"))
                    {
                        break;
                    }
                    else if (textMan.ToLower().Equals("w") || textMan.ToLower().Equals("woman"))
                    {
                        isman = false;
                        break;
                    }
                    else
                    {
                        Console.Write("Wrong command");
                        Console.Write("Continue? [y/n]");
                        string answer = Console.ReadLine();
                        if (answer.Equals("y"))
                        {
                            textAge = Console.ReadLine();
                            continue;
                        }
                        else if (answer.Equals("n"))
                        {
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Command not recognised");
                            continue;
                        }
                    }

                }
                Server.DataBase.AddClient(new Client(name,password,age, isman));
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
