using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArduinoRFIDReader
{
    class SerialCommunication : SerialPort
    {
        private string receivedCommand;
        private string commandToSend;
        public SerialCommunication(string portName, int baudRate) : base(portName, baudRate)
        {

        }


        /// <summary>
        /// Gets UID of RFID card used on Arduino's RFID reader. Returns null if card hasn't been read within 30 sec.
        /// </summary>
        /// <returns></returns>
        public string GetCardUID() //should be used with try / catch in main code
        {
            string UID = null;
            this.Open();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.Write("<1>");
            while(stopwatch.ElapsedMilliseconds>30000 || UID == null)
            {
                UID = this.ReadExisting();
                Thread.Sleep(200);
            }
            return UID;
        }
    }
}
