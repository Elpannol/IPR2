using IPR2Client.Forms;
using System;
using System.IO.Ports;

namespace IPR2Client
{
    class BicycleConnection
    {
        public bool isSimulating { get; set; }

        private SerialPort serialPort;
        private Simulator simulator;

        public BicycleConnection(SerialPort serialPort) {
            this.serialPort = serialPort;
            isSimulating = false;
        }

        public BicycleConnection (string name) {
            // Create a connection using the simulator
            simulator = new Simulator(name)
            {
                Visible = true
            };
            isSimulating = true;
        }

        public void StartBicycle()
        {
            SendCommand("ST");
        }

        public void ResetBicycle()
        {
            SendCommand("RS");
        }

        public void EnableCommand()
        {
            SendCommand("CM");
        }

        public void ChangePower(string watt)
        {
            SendCommand($"PW {watt}");
        }

        public void SendCommand(string command) {
            if (isSimulating)
            {
                if (command.Contains("PW"))
                {
                    var list = command.Split();
                    simulator.Measurement.Weerstand = int.Parse(list[1]);
                }
            } else {
                if ((serialPort != null) && serialPort.IsOpen)
                    serialPort.WriteLine(command + "\n\r");
                else
                    throw new Exception("The serial port is not properly initialised!");
            }
        }

        public string ReceiveCommand() {
            if (isSimulating) {
                return simulator.ReadLine();
            } else {
                if ((serialPort != null) && serialPort.IsOpen)
                    return serialPort.ReadLine();
                throw new Exception("The serial port is not properly initialised!");
            }
        }

        /**
         * Close the connection to either the simulator or the bike.
         */
        public void Close() {
            if (isSimulating) {
                simulator.Dispose();
            } else {
                serialPort.Close();
            }
        }

   }
}
