using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArduinoRFIDReader
{
    public class MFRC522ReaderWriter : SerialPort
    {
        public MFRC522ReaderWriter()
        {
            this.BaudRate = 9600;
            this.ReadTimeout = 5000;
        }

        public MFRC522ReaderWriter(IContainer container) : base(container)
        {
        }

        public MFRC522ReaderWriter(string portName) : base(portName)
        {
        }

        public MFRC522ReaderWriter(string portName, int baudRate) : base(portName, baudRate)
        {
        }

        public MFRC522ReaderWriter(string portName, int baudRate, Parity parity) : base(portName, baudRate, parity)
        {
        }

        public MFRC522ReaderWriter(string portName, int baudRate, Parity parity, int dataBits) : base(portName, baudRate, parity, dataBits)
        {
        }

        public MFRC522ReaderWriter(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits) : base(portName, baudRate, parity, dataBits, stopBits)
        {
        }

        public string ReadUidCommand { get; set; } = "<1>";
        public string CancelCommand { get; set; } = "<-1>";
        public bool ContinueOperation { get; set; } = true;

        public string ReadUID()
        {
            string uid = null;
            this.ReadTimeout = 5000;
            try
            {
                this.Open();
                this.WriteLine(ReadUidCommand);
                uid = this.ReadLine();
                if(uid.Contains(">") && uid.Contains("<"))
                {
                    uid = uid.Replace("<", "").Replace(">", "");
                    this.Close();
                    return uid;
                }
            }
                
            catch (TimeoutException)
            {
                this.WriteLine(CancelCommand);
                this.Close();
            }
            catch (System.IO.IOException)
            {
                if (this.IsOpen == false) this.Open();
                this.WriteLine(CancelCommand);
                this.Close();
            }
            return uid;
        }

        public async Task<string> ReadUIDAsync()
        {
            return await Task.Run(() => ReadUID());
        }

        public bool IsAvailable()
        {
            try
            {
                this.Open();
                this.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
