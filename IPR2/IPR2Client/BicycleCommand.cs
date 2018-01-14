using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR2Client
{
    class BicycleCommand
    {
        public static void SendCommand(string command, SerialPort serialPort)
        {
            if ((serialPort != null) && serialPort.IsOpen)
                serialPort.WriteLine(command);
            else
                Console.WriteLine("Failed to send command");
        }

        public static string ReceiveCommand(SerialPort serialPort)
        {
            if ((serialPort != null) && serialPort.IsOpen)
                return serialPort.ReadLine();
            return null;
        }
   }
}
