using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using NAudio.Wave;
using System.IO;

namespace UtilityPack.Validation {

    public class Parse {

        public static bool IsIPAddress(string ip) {
            string _pattern = "^[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}$";
            return Regex.IsMatch(ip, _pattern, RegexOptions.IgnoreCase);
        }

        public static bool IsMacAddress(string mac) {
            return Regex.IsMatch(mac.ToUpper(), "^[A-F,0-9]{12}$");
        }

        public static bool IsVnptProductSerialNumber(string SerialNumber) {
            try {
                //check chieu dai chuoi sn = 15
                if (SerialNumber.Length != 15) return false;
                //check ki tu SN
                return true;
            }
            catch {
                return false;
            }
        }

        public static bool IsMatchingMacCode(string SerialNumber, string MacAddress, string MacCode) {
            //serial: 12011221G010203
            //mac : A4F4C2010203
            //mac code: A06518=G,A4F4C2=H,D49AA0=I

            try {
                string mac_header = MacAddress.Substring(0, 6).ToUpper();
                string m_code = "";
                string[] buffer = MacCode.Split(',');
                foreach (var b in buffer) {
                    if (b.ToUpper().Contains(mac_header)) {
                        m_code = b.Split('=')[1].ToUpper();
                        break;
                    }
                }

                if (string.IsNullOrEmpty(m_code) || string.IsNullOrWhiteSpace(m_code)) return false;
                string s_code = SerialNumber.Substring(8, 1).ToUpper();

                return m_code.Equals(s_code);
            } catch { return false; }
        }


        public static bool IsVnptProductSerialNumber (string SerialNumber, string ID, int IDLength) {
            string tmpStr = "";

            //check null or empty
            if (string.IsNullOrEmpty(SerialNumber)) return false;
            //check length
            if (SerialNumber.Length != 15) return false;
            //check mac
            tmpStr = SerialNumber.Substring(9, 6);
            string pattern = IDLength == 12 ? ID.Substring(6, 6) : ID.Substring(0, 6);
            if (!tmpStr.ToLower().Equals(pattern.ToLower())) return false;

            return true;
        }

        public static bool IsSmartHomeDeviceID(string deviceid) {
            //check null or empty
            if (string.IsNullOrEmpty(deviceid)) return false;

            //check length
            if (deviceid.Length != 16) return false;

            //check format
            string pattern = "^[0-9,A-F]{16}$";
            return Regex.IsMatch(deviceid.ToUpper(), pattern);
        }


        /// <summary>
        /// Validate video file: [0 = sound video] // [1 = silent video] // [-1 = system error]
        /// </summary>
        /// <param name="file_path"></param>
        /// <param name="file_name"></param>
        /// <returns></returns>
        public static int IsSoundVideoFile(string file_path, string file_name) {
            string code = "0xc00d36b3";
            string error = "";
            string videofile = System.IO.Path.Combine(file_path, file_name);
            string audiofile = System.IO.Path.Combine(file_path, "unknown.wav");
            int kq = int.MinValue;

            try {
                using (var video = new MediaFoundationReader(videofile)) {
                    WaveFileWriter.CreateWaveFile(audiofile, video);
                }
                kq = 0;
            }
            catch (Exception ex) { error = ex.ToString(); }

            if (kq == int.MinValue) kq = error.ToLower().Contains(code) ? 1 : -1; //
            else System.IO.File.Delete(audiofile); //delete file audio

            return kq;
        }

        public static bool IsVnptMacAddress(string mac, string vnpt_mac_header) {
            if (mac.Length != 12) return false; //check length
            if (!IsMacAddress(mac)) return false; //check format
            string ss = mac.Substring(0, 6); //check header
            bool r = vnpt_mac_header.ToLower().Contains(ss.ToLower());

            return r; //return value
        }

        public static bool IsVnptUidCode(string uid, string vnpt_uid_header) {
            if (uid.Length != 38) return false; //check length
            string ss = uid.Substring(0, vnpt_uid_header.Length); //check header
            bool r = vnpt_uid_header.ToLower().Contains(ss.ToLower());
            return r; //return value
        }

        public static bool IsVnptProductNumber(string s) {
            if (s.Length != 3) return false; //check length
            string pattern = "^[0-9]{3}$"; //check format
            
            return Regex.IsMatch(s.ToUpper(), pattern); //return value
        }

        public static bool IsVnptProductVersion(string s) {
            int x;
            bool r = int.TryParse(s, out x);
            if (!r) return r;

            return x > 0;
        }

        public static bool IsVnptFactory (string s) {
            int x;
            bool r = int.TryParse(s, out x);
            if (!r) return r;

            return x > 0;
        }

        public static bool IsVnptLineCode(string s) {
            int x;
            bool r = int.TryParse(s, out x);
            if (!r) return r;

            return x > 0;
        }

        public static bool IsVnptStationName(string s) {
            //check null or empty
            bool r = false;
            r = !string.IsNullOrEmpty(s);
            if (!r) return false;

            //check null or white space
            r = !string.IsNullOrWhiteSpace(s);
            if (!r) return false;

            return r;
        }

        public static bool IsVnptJigNumber(string s) {
            int x;
            bool r = int.TryParse(s, out x);
            if (!r) return r;

            return x > 0;
        }

        public static bool IsVnptWorkOrder (string s) {
            //check null or empty
            bool r = false;
            r =! string.IsNullOrEmpty(s);
            if (!r) return false;

            //check null or white space
            r =! string.IsNullOrWhiteSpace(s);
            if (!r) return false;

            return r;
        }

        public static bool IsVnptWorker(string s) {
            //check null or empty
            bool r = false;
            r = !string.IsNullOrEmpty(s);
            if (!r) return false;

            //check null or white space
            r = !string.IsNullOrWhiteSpace(s);
            if (!r) return false;

            return r;
        }

        public static bool IsVnptProductColor (string s) {
            if (s.Length != 1) return false;
            string pattern = "^[1-9,A-Z]$"; //check format
            return Regex.IsMatch(s.ToUpper(), pattern); //return value
        }

        public static bool IsVnptProductName (string s) {
            //check null or empty
            bool r = false;
            r = !string.IsNullOrEmpty(s);
            if (!r) return false;

            //check null or white space
            r = !string.IsNullOrWhiteSpace(s);
            if (!r) return false;

            return r;
        }

        public static bool IsVnptProductCode(string s) {
            //check null or empty
            bool r = false;
            r = !string.IsNullOrEmpty(s);
            if (!r) return false;

            //check null or white space
            r = !string.IsNullOrWhiteSpace(s);
            if (!r) return false;

            return r;
        }

        public static bool IsVnptSubStamp(string s) {
            //check null or empty
            bool r = false;
            r = !string.IsNullOrEmpty(s);
            if (!r) return false;

            //check null or white space
            r = !string.IsNullOrWhiteSpace(s);
            if (!r) return false;

            return r;
        }

        public static bool IsDirectoryPath (string s) {
            //check null or empty
            bool r = false;
            r = !string.IsNullOrEmpty(s);
            if (!r) return false;

            //check null or white space
            r = !string.IsNullOrWhiteSpace(s);
            if (!r) return false;

            return r;
        }

        public static bool isNumber(string s) {
            int x;
            bool r = int.TryParse(s, out x);
            if (!r) return r;

            return x > 0;
        }

        public static bool isZero(string s) {
            int x;
            bool r = int.TryParse(s, out x);
            if (!r) return r;

            return x == 0;
        }

        public static bool isString(string s) {
            bool r = false;

            //check null or empty
            r = !string.IsNullOrEmpty(s);
            if (!r) return false;

            //check null or white space
            r = !string.IsNullOrWhiteSpace(s);
            if (!r) return false;

            return r;
        }

        public static bool isText(string s) {
            if (!isString(s)) return false;

            bool r = true;
            s = s.ToUpper();
            foreach (char c in s) {
                if ((int)c < 65 || (int)c > 90) {
                    r = false;
                    break;
                }
            }
            return r;
        }

        public static bool isHexa(string s) {
            if (!isString(s)) return false;

            bool r = true;
            s = s.ToUpper();
            foreach (char c in s) {
                int ic = (int)c;
                if (ic < 48 || (ic > 57 && ic < 65) || ic > 70) { r = false; break; }
            }
            return r;
        }

        public static bool isMix(string s) {
            if (!isString(s)) return false;

            bool r = true;
            s = s.ToUpper();
            foreach (char c in s) {
                int ic = (int)c;
                if (ic < 48 || (ic > 57 && ic < 65) || ic > 90) { r = false; break; }
            }
            return r;
        }

        public static bool compareStringLength(string s, string header, string std_len) {
            if (std_len == "*") return true;
            if ((!isNumber(std_len)) && (!isZero(std_len))) return false;
            string data = header == "" ? s : s.ToUpper().Replace(header.ToUpper(), "").Trim();
            return data.Length == int.Parse(std_len);
        }

        public static bool compareStringText(string s, string pattern) {
            if (string.IsNullOrEmpty(pattern)) return true;
            return s.Contains(pattern);
        }

        static bool _tryCreateFile() {
            try {
                string dir = AppDomain.CurrentDomain.BaseDirectory;
                string f = string.Format("{0}test.txt", dir);
                using (var sw = new StreamWriter(f)) {
                    sw.WriteLine("test");
                }
                File.Delete(f);
                return true;
            }
            catch {
                return false;
            }
        }

        static bool _checkPathLength(int add_number) {
            int d = AppDomain.CurrentDomain.BaseDirectory.Length + add_number;
            return d < 255;
        }

        public static bool isLogPathValid(int add_number) {
            return _tryCreateFile() && _checkPathLength(add_number);
        }
        

    }
}
