using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UtilityPack.Protocol {
    public class UART : IProtocol {

        SerialPort serial = null;

        public bool Open() {
            throw new NotImplementedException();
        }

        public bool Open(string visaaddress) {
            throw new NotImplementedException();
        }

        public bool Open(string portname, string baudrate, string databits, Parity parity, StopBits stopbits) {
            try {
                serial = new SerialPort();
                serial.PortName = portname;
                serial.BaudRate = int.Parse(baudrate);
                serial.DataBits = int.Parse(databits);
                serial.Parity = parity;
                serial.StopBits = stopbits;
                serial.Open();

                return serial.IsOpen;
            }
            catch {
                return false;
            }
        }

        public bool Open(string ip, string portnumber, string user, string password) {
            throw new NotImplementedException();
        }

        public bool IsConnected() {
            if (serial == null) return false;
            try {
                return serial.IsOpen;
            }
            catch {
                return false;
            }
        }


        public string Query(string cmd) {
            this.WriteLine(cmd);
            Thread.Sleep(100);
            return this.Read();
        }

        public string Read() {
            if (serial == null) return null;
            return serial.ReadExisting();
        }

        public bool Write(string cmd) {
            if (serial == null) return false;

            try {
                serial.Write(cmd);
                return true;
            }
            catch {
                return false;
            }
        }

        public bool WriteLine(string cmd) {
            return this.Write(cmd + "\n");
        }


        public bool Close() {
            try {
                if (serial != null) serial.Close();
                return true;
            }
            catch {
                return false;
            }
        }

    }
}
