using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityPack.Protocol {
    public class SSH : IProtocol {
        public bool Close() {
            throw new NotImplementedException();
        }

        public bool IsConnected() {
            throw new NotImplementedException();
        }

        public bool Open() {
            throw new NotImplementedException();
        }

        public bool Open(string visaaddress) {
            throw new NotImplementedException();
        }

        public bool Open(string portname, string baudrate, string databits, Parity parity, StopBits stopbits) {
            throw new NotImplementedException();
        }

        public bool Open(string ip, string portnumber, string user, string password) {
            throw new NotImplementedException();
        }

        public string Query(string cmd) {
            throw new NotImplementedException();
        }

        public string Read() {
            throw new NotImplementedException();
        }

        public bool Write(string cmd) {
            throw new NotImplementedException();
        }

        public bool WriteLine(string cmd) {
            throw new NotImplementedException();
        }
    }
}
