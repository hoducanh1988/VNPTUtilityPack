using System;
using System.IO;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UtilityPack.Protocol {
    public class TELNET : IProtocol {

        TcpClient clients;
        enum Verbs { WILL = 251, WONT = 252, DO = 253, DONT = 254, IAC = 255 }
        enum Options { SGA = 3 }
        public string host;
        public int port;
        bool _isConnected { get; set; }

        private void configTCP() {
            // Don't allow another process to bind to this port.
            this.clients.ExclusiveAddressUse = false;
            // sets the amount of time to linger after closing, using the LingerOption public property.
            this.clients.LingerState = new LingerOption(false, 0);
            // Sends data immediately upon calling NetworkStream.Write.
            this.clients.NoDelay = true;
            // Sets the receive buffer size using the ReceiveBufferSize public property.
            this.clients.ReceiveBufferSize = 1024;
            // Sets the receive time out using the ReceiveTimeout public property.
            this.clients.ReceiveTimeout = 5000;
            // Sets the send buffer size using the SendBufferSize public property.
            this.clients.SendBufferSize = 1024;
            // sets the send time out using the SendTimeout public property.
            this.clients.SendTimeout = 5000;
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

        public bool Open(string ip, string portnumber, string user, string password, string flag_success, bool isbreak) {
            this.host = ip;
            this.port = int.Parse(portnumber);

            //connect 
            bool r = _Connection();
            if (!r) return false;

            //login
            r = _Login(user, password, flag_success, isbreak);
            return r;
        }

        
        bool _Connection() {
            _isConnected = false;
            this.clients = new TcpClient();
            this.configTCP();
            try {
                _isConnected = this.clients.ConnectAsync(host, port).Wait(3000);
            }
            catch {
                _isConnected = false;
            }
            return _isConnected;
        }

        bool _Login(string user, string pass, string flag, bool isbreak) {
            try {
                string msg = "";
                bool r = false;

                //send user
                RE_USER:
                msg = this.Read();
                r = msg.ToLower().Contains("login:");
                if (!r) {
                    this.WriteLine("");
                    Thread.Sleep(100);
                    goto RE_USER;
                }
                else this.WriteLine(user);

                //send password
                RE_PASS:
                msg = this.Read();
                r = msg.ToLower().Contains("password:");
                if (!r) {
                    this.WriteLine("");
                    Thread.Sleep(100);
                    goto RE_PASS;
                }
                else this.WriteLine(pass);

                if (!isbreak) {
                    //judge
                    msg = this.Read();
                    return msg.ToLower().Contains(flag.ToLower());
                }

                //write break
                RE_BREAK:
                msg = this.Read();
                r = msg.ToLower().Contains("root@:#");
                if (!r) {
                    this.WriteBreak();
                    Thread.Sleep(100);
                    goto RE_BREAK;
                }

                return r;
            } catch {
                return false;
            }
        }


        public bool IsConnected() {
            return _isConnected;
        }

        public string Query(string cmd) {
            this.WriteLine(cmd);
            Thread.Sleep(100);
            return this.Read();
        }

        public string Read() {
            if (!_isConnected) return string.Empty;
            NetworkStream stream = this.clients.GetStream();
            
            //wait data available
            WAIT_AVAILABLE:
            bool _isAvailable = stream.DataAvailable;
            if (!_isAvailable) { Thread.Sleep(10); goto WAIT_AVAILABLE; }

            //read data to end
            StringBuilder sb = new StringBuilder();
            int input = 0;
            WAIT_READ:
            input = this.clients.GetStream().ReadByte();
            sb.Append((char)input);
            if (this.clients.Available > 0) goto WAIT_READ;

            //return data
            return sb.ToString();
        }

        public bool Write(string cmd) {
            try {
                if (!_isConnected) return false;
                byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(cmd);
                this.clients.GetStream().Write(buf, 0, buf.Length);
                return true;
            }
            catch {
                return false;
            }
        }

        public bool WriteLine(string cmd) {
            try {
                this.Write(cmd + "\n");
                return true;
            }
            catch { return false; }
        }

        public bool WriteBreak() {
            try {
                byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(new char[] { Convert.ToChar(03) });
                this.clients.GetStream().Write(buf, 0, buf.Length);
                return true;
            }
            catch {
                return false;
            }
        }

        public bool Close() {
            try {
                if (this.clients != null) this.clients.Close();
                return true;
            }
            catch { return false; }

        }
    }
}
