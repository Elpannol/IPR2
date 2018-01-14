﻿using IPR2Client.Forms;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPR2Client
{
    class BicycleConnection
    {
        public bool isSimulating { get; set; }

        private SerialPort serialPort;
        private Simulator simulator;

        public BicycleConnection(SerialPort serialPort) {
            this.serialPort = serialPort;
            this.isSimulating = false;
        }

        public BicycleConnection (string name) {
            // Create a connection using the simulator
            this.simulator = new Simulator(name)
            {
                Visible = true
            };
            this.isSimulating = true;
        }
 
        public void SendCommand(string command) {
            if (this.isSimulating) {
                // TODO: communication with simulator
            } else {
                if ((serialPort != null) && serialPort.IsOpen)
                    serialPort.WriteLine(command);
                else
                    throw new Exception("The serial port is not properly initialised!");
            }
        }

        public string ReceiveCommand() {
            if (this.isSimulating) {
                return this.simulator.ReadLine();
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
            if (this.isSimulating) {
                simulator.Dispose();
            } else {

            }
        }

   }
}
