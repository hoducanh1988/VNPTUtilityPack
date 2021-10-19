using NationalInstruments.VisaNS;
using System;
using System.IO.Ports;

namespace UtilityPack.Protocol {
    public class SCPI : IProtocol {

        MessageBasedSession mbSession = null;

        public bool Open() {
            throw new NotImplementedException();
        }
        public bool Open(string visaaddress) {
            try {
                mbSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open(visaaddress);
                return true;
            }
            catch {
                return false;
            }
        }
        public bool Open(string portname, string baudrate, string databits, System.IO.Ports.Parity parity, StopBits stopbits) {
            throw new NotImplementedException();
        }
        public bool Open(string ip, string portnumber, string user, string password) {
            throw new NotImplementedException();
        }
        public bool IsConnected() {
            if (mbSession == null) return false;

            try {
                string data = mbSession.Query("*IDN?\n");
                return data.Trim().Length != 0;
            }
            catch {
                return false;
            }
        }


        public bool Write(string cmd) {
            if (mbSession == null) return false;

            try {
                mbSession.Write(cmd);
                return true;
            }
            catch {
                return false;
            }
        }
        public bool WriteLine(string cmd) {
            if (mbSession == null) return false;

            try {
                mbSession.Write(cmd + "\n");
                return true;
            }
            catch {
                return false;
            }
        }
        public string Query(string cmd) {
            if (mbSession == null) return null;

            try {
                return mbSession.Query(cmd + "\n");
            }
            catch {
                return null;
            }
        }
        public string Read() {
            if (mbSession == null) return null;
            return mbSession.ReadString();
        }


        public bool Close() {
            try {
                if (mbSession != null) mbSession.Dispose();
                return true;
            }
            catch {
                return false;
            }
        }

    }
}
