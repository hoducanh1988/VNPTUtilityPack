using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ADOX;

namespace UtilityPack.Converter {

    public class myConverter {

        public static ADOX.DataTypeEnum FromVSTypeToTableAccessDataType(string typeStr) {

            switch (typeStr.ToLower()) {
                case "bool": return DataTypeEnum.adBoolean;
                case "byte": return DataTypeEnum.adInteger;
                case "decimal": return DataTypeEnum.adDecimal;
                case "double": return DataTypeEnum.adDouble;
                case "float": return DataTypeEnum.adDouble;
                case "int32": return DataTypeEnum.adInteger;
                case "int64": return DataTypeEnum.adInteger;
                case "int": return DataTypeEnum.adInteger;
                case "long": return DataTypeEnum.adInteger;
                case "sbyte": return DataTypeEnum.adInteger;
                case "short": return DataTypeEnum.adInteger;
                case "uint": return DataTypeEnum.adUnsignedInt;
                case "ulong": return DataTypeEnum.adUnsignedInt;
                case "ushort": return DataTypeEnum.adUnsignedInt;
                case "char": return DataTypeEnum.adChar;
                case "string": return DataTypeEnum.adLongVarWChar;
                case "datetime": return DataTypeEnum.adDate;
                default: return DataTypeEnum.adEmpty;
            }

            //return DataTypeEnum.adLongVarWChar;
        }

        public static System.IO.Ports.Parity FromStringToSerialParity(string strParity) {

            switch (strParity.ToLower()) {
                case "none": return System.IO.Ports.Parity.None;
                case "even": return System.IO.Ports.Parity.Even;
                case "odd": return System.IO.Ports.Parity.Odd;
                case "mark": return System.IO.Ports.Parity.Mark;
                case "space": return System.IO.Ports.Parity.Space;
                default: return System.IO.Ports.Parity.None;
            }

        }

        public static System.IO.Ports.StopBits FromStringToSerialStopBits(string strStopBit) {

            switch (strStopBit.ToLower()) {
                case "none":
                case "0": return System.IO.Ports.StopBits.None;
                case "one":
                case "1": return System.IO.Ports.StopBits.One;
                case "onepointfive":
                case "1.5": return System.IO.Ports.StopBits.OnePointFive;
                case "two":
                case "2": return System.IO.Ports.StopBits.Two;

                default: return System.IO.Ports.StopBits.One;
            }
        }

        public static string FromIDToSerialNumber(string ID, string ProductNumber, string ProductVersion, string ProductColor, string Factory) {
            try {
                //SN = ProductNumber[0-2] + Factory[2-3] + Year[3-4] + Week[4-6] + ProductVersion[6-7] + ProductColor[7-8] + ID[8-15]
                string year = "", week = "", id = "";
                int t = int.Parse(DateTime.Now.ToString("yyyy").Substring(2, 2)) - 13;
                if (t < 0) return null;
                year = t < 10 ? t.ToString() : Convert.ToChar(55 + t).ToString().Trim();
                CultureInfo ciCurr = CultureInfo.CurrentCulture;
                int weekNum = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                week = weekNum.ToString().Trim();
                id = ID.Substring(0, 6).ToUpper().Trim();
                //
                return string.Format("{0}{1}{2}{3}{4}{5}{6}",
                                     ProductNumber.ToUpper().Trim(),
                                     Factory.ToUpper().Trim(),
                                     year,
                                     week,
                                     ProductVersion.ToUpper().Trim(),
                                     ProductColor.ToUpper().Trim(),
                                     id);
            }
            catch { return null; }
        }

        public static string FromMACToSerialNumber(string MAC, string ProductNumber, string ProductVersion, string ProductColor, string Factory) {
            try {
                //SN = ProductNumber[0-2] + Factory[2-3] + Year[3-4] + Week[4-6] + ProductVersion[6-7] + ProductColor[7-8] + MAC[8-15]
                string year = "", week = "", id = "";
                int t = int.Parse(DateTime.Now.ToString("yyyy").Substring(2, 2)) - 13;
                if (t < 0) return null;
                year = t < 10 ? t.ToString() : Convert.ToChar(55 + t).ToString().Trim();
                CultureInfo ciCurr = CultureInfo.CurrentCulture;
                int weekNum = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                week = weekNum < 10 ? string.Format("0{0}", weekNum)  : weekNum.ToString().Trim();
                id = MAC.Substring(6, 6).ToUpper().Trim();
                //
                return string.Format("{0}{1}{2}{3}{4}{5}{6}",
                                     ProductNumber.ToUpper().Trim(),
                                     Factory.ToUpper().Trim(),
                                     year,
                                     week,
                                     ProductVersion.ToUpper().Trim(),
                                     ProductColor.ToUpper().Trim(),
                                     id);
            }
            catch { return null; }
        }

        public static string FromMACToSerialNumberNewFormat(string MAC, string ProductNumber, string ProductVersion, string ProductMacCode, string Factory) {
            try {
                //SN = ProductNumber[0-2] + Factory[2-3] + Year[3-4] + Week[4-6] + ProductVersion[6-7] + ProductColor[7-8] + MAC[8-15]
                string year = "", week = "", id = "", mac_header="", mac_code="";
                int t = int.Parse(DateTime.Now.ToString("yyyy").Substring(2, 2)) - 13;
                if (t < 0) return null;
                year = t < 10 ? t.ToString() : Convert.ToChar(55 + t).ToString().Trim();
                CultureInfo ciCurr = CultureInfo.CurrentCulture;
                int weekNum = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                week = weekNum < 10 ? string.Format("0{0}", weekNum) : weekNum.ToString().Trim();
                id = MAC.Substring(6, 6).ToUpper().Trim();
                mac_header = MAC.Substring(0, 6).ToUpper().Trim();

                ProductMacCode = ProductMacCode.Replace("\r", "").Replace("\n", "").Trim();
                string[] buffer = ProductMacCode.Split(',');
                foreach (var b in buffer) {
                    if (b.ToUpper().Contains(mac_header)) {
                        mac_code = b.Split('=')[1].ToUpper();
                        break;
                    }
                }
                if (string.IsNullOrEmpty(mac_code) || string.IsNullOrWhiteSpace(mac_code)) return null;

                //
                return string.Format("{0}{1}{2}{3}{4}{5}{6}",
                                     ProductNumber.ToUpper().Trim(),
                                     Factory.ToUpper().Trim(),
                                     year,
                                     week,
                                     ProductVersion.ToUpper().Trim(),
                                     mac_code.ToUpper().Trim(),
                                     id);
            }
            catch { return null; }
        }

        public static string FromMACToUIDCode(string MAC, string vnpt_uid_header) {
            try {
                string md5 = stringToMD5(MAC);
                return string.Format("{0}{1}", vnpt_uid_header, md5);
            }
            catch { return null; }
        }

        public static string MacToGponSerial(string mac) {
            try {
                string mac_Header = mac.Substring(0, 6);
                string low_MAC = mac.Substring(6, 6);
                string origalByteString = Convert.ToString(HexToBin(low_MAC)[0], 2).PadLeft(8, '0');
                string VNPT_SERIAL_ONT = null;

                origalByteString = origalByteString + "" + Convert.ToString(HexToBin(low_MAC)[1], 2).PadLeft(8, '0');
                origalByteString = origalByteString + "" + Convert.ToString(HexToBin(low_MAC)[2], 2).PadLeft(8, '0');
                //----HEX to BIN Cach 2-------
                string value = low_MAC;
                var s = String.Join("", low_MAC.Select(x => Convert.ToString(Convert.ToInt32(x + "", 16), 2).PadLeft(4, '0')));
                //----HEX to BIN Cach 2-------
                string shiftByteString = "";
                shiftByteString = origalByteString.Substring(1, origalByteString.Length - 1) + origalByteString[0];

                string[] lines = System.IO.File.ReadAllLines(string.Format("{0}GponFormat.dll", AppDomain.CurrentDomain.BaseDirectory));
                foreach (var line in lines) {
                    if (line.ToUpper().Contains(mac_Header.ToUpper())) {
                        VNPT_SERIAL_ONT = line.Split('=')[1].ToUpper() + BinToHex(shiftByteString);
                        break;
                    }
                }

                return VNPT_SERIAL_ONT;
            }
            catch {
                return null;
            }
        }

        public static string MacToWpsPin(string mac) {
            try {
                long tempPIN;
                long PinCodeDevice;
                long accum = 0;
                long digit;

                string StrInput = EncodeString(mac);

                tempPIN = long.Parse(StrInput, System.Globalization.NumberStyles.HexNumber);
                PinCodeDevice = tempPIN % 9999999;

                PinCodeDevice *= 10;
                accum += 3 * ((PinCodeDevice / 10000000) % 10);
                accum += 1 * ((PinCodeDevice / 1000000) % 10);
                accum += 3 * ((PinCodeDevice / 100000) % 10);
                accum += 1 * ((PinCodeDevice / 10000) % 10);
                accum += 3 * ((PinCodeDevice / 1000) % 10);
                accum += 1 * ((PinCodeDevice / 100) % 10);
                accum += 3 * ((PinCodeDevice / 10) % 10);

                digit = (accum % 10);
                accum = (10 - digit) % 10;
                PinCodeDevice += accum;
                string str = string.Empty;
                str = PinCodeDevice.ToString("D8");

                return str;
            }
            catch {
                return null;
            }
        }

        public static string MacToWpsPin_040H(string mac) {
            try {
                int[] macAddr = StringArrayToIntArray(MacToSixArray(mac));

                int iPin, checksum;
                iPin = macAddr[0] * 256 * 256 + macAddr[4] * 256 + macAddr[5];
                iPin = iPin % 10000000;
                checksum = ComputeChecksum(iPin);
                iPin = iPin * 10 + checksum;

                string result = iPin.ToString();
                for (int i = 0; i < 8; i++) {
                    if (result.Length < 8) {
                        result = "0" + result;
                    }
                    else break;
                }

                return result;
            }
            catch {
                return null;
            }
        }

        private static string EncodeString(string mac) {
            string result = string.Empty;
            string temp = string.Empty;
            StringBuilder str = new StringBuilder();

            MD5CryptoServiceProvider myMD5 = new MD5CryptoServiceProvider();
            byte[] myPass = System.Text.Encoding.UTF8.GetBytes(mac);
            myPass = myMD5.ComputeHash(myPass);
            StringBuilder s = new StringBuilder();
            foreach (byte p in myPass) {
                s.Append(p.ToString("x").ToLower());
            }
            temp = s.ToString();
            for (int i = 0; i < 6; i++) {
                str.Append(temp[i * 3 + 3]);
            }
            return str.ToString().Trim();
        }

        private static int ComputeChecksum(int PIN) {
            int digit_s;
            int accum = 0;

            PIN *= 10;
            accum += 3 * ((PIN / 10000000) % 10);
            accum += 1 * ((PIN / 1000000) % 10);
            accum += 3 * ((PIN / 100000) % 10);
            accum += 1 * ((PIN / 10000) % 10);
            accum += 3 * ((PIN / 1000) % 10);
            accum += 1 * ((PIN / 100) % 10);
            accum += 3 * ((PIN / 10) % 10);

            digit_s = (accum % 10);
            return ((10 - digit_s) % 10);
        }

        public static string[] MacToSixArray(string mac) {
            string[] buffer = new string[6];
            for (int i = 0; i < mac.Length; i += 2) {
                buffer[i / 2] = mac.Substring(i, 2);
            }

            return buffer;
        }

        public static int[] StringArrayToIntArray(string[] arr) {
            int[] buffer = new int[arr.Length];
            for (int i = 0; i < arr.Length; i++) {
                buffer[i] = HexToDec(arr[i]);
            }

            return buffer;
        }

        public static string DecToHex(int dec) {
            string hex = "";
            hex = dec.ToString("X4");
            if (hex.Length > 4) {
                hex = hex.Substring(hex.Length - 4, 4);
            }
            return hex;
        }

        public static int HexToDec(string Hex) {
            int decValue = int.Parse(Hex, System.Globalization.NumberStyles.HexNumber);
            return decValue;
        }

        public static string BinToHex(string bin) {
            string output = "";
            try {
                int rest = bin.Length % 4;
                bin = bin.PadLeft(rest, '0'); //pad the length out to by divideable by 4

                for (int i = 0; i <= bin.Length - 4; i += 4) {
                    output += string.Format("{0:X}", Convert.ToByte(bin.Substring(i, 4), 2));
                }

                return output;
            }
            catch {
                return null;
            }
        }

        public static Byte[] HexToBin(string pHexString) {
            if (String.IsNullOrEmpty(pHexString))
                return new Byte[0];

            if (pHexString.Length % 2 != 0)
                throw new Exception("Hexstring must have an even length");

            Byte[] bin = new Byte[pHexString.Length / 2];
            int o = 0;
            int i = 0;
            for (; i < pHexString.Length; i += 2, o++) {
                switch (pHexString[i]) {
                    case '0': bin[o] = 0x00; break;
                    case '1': bin[o] = 0x10; break;
                    case '2': bin[o] = 0x20; break;
                    case '3': bin[o] = 0x30; break;
                    case '4': bin[o] = 0x40; break;
                    case '5': bin[o] = 0x50; break;
                    case '6': bin[o] = 0x60; break;
                    case '7': bin[o] = 0x70; break;
                    case '8': bin[o] = 0x80; break;
                    case '9': bin[o] = 0x90; break;
                    case 'A': bin[o] = 0xa0; break;
                    case 'a': bin[o] = 0xa0; break;
                    case 'B': bin[o] = 0xb0; break;
                    case 'b': bin[o] = 0xb0; break;
                    case 'C': bin[o] = 0xc0; break;
                    case 'c': bin[o] = 0xc0; break;
                    case 'D': bin[o] = 0xd0; break;
                    case 'd': bin[o] = 0xd0; break;
                    case 'E': bin[o] = 0xe0; break;
                    case 'e': bin[o] = 0xe0; break;
                    case 'F': bin[o] = 0xf0; break;
                    case 'f': bin[o] = 0xf0; break;
                    default: throw new Exception("Invalid character found during hex decode");
                }

                switch (pHexString[i + 1]) {
                    case '0': bin[o] |= 0x00; break;
                    case '1': bin[o] |= 0x01; break;
                    case '2': bin[o] |= 0x02; break;
                    case '3': bin[o] |= 0x03; break;
                    case '4': bin[o] |= 0x04; break;
                    case '5': bin[o] |= 0x05; break;
                    case '6': bin[o] |= 0x06; break;
                    case '7': bin[o] |= 0x07; break;
                    case '8': bin[o] |= 0x08; break;
                    case '9': bin[o] |= 0x09; break;
                    case 'A': bin[o] |= 0x0a; break;
                    case 'a': bin[o] |= 0x0a; break;
                    case 'B': bin[o] |= 0x0b; break;
                    case 'b': bin[o] |= 0x0b; break;
                    case 'C': bin[o] |= 0x0c; break;
                    case 'c': bin[o] |= 0x0c; break;
                    case 'D': bin[o] |= 0x0d; break;
                    case 'd': bin[o] |= 0x0d; break;
                    case 'E': bin[o] |= 0x0e; break;
                    case 'e': bin[o] |= 0x0e; break;
                    case 'F': bin[o] |= 0x0f; break;
                    case 'f': bin[o] |= 0x0f; break;
                    default: throw new Exception("Invalid character found during hex decode");
                }
            }
            return bin;
        }

        public static string StringToHex(string hexstring) {
            StringBuilder sb = new StringBuilder();
            foreach (char t in hexstring) {
                //Note: X for upper, x for lower case letters
                sb.Append(Convert.ToInt32(t).ToString("x"));
            }
            return sb.ToString();
        }

        public static string HexToString(string hexString) {
            var sb = new StringBuilder();
            for (var i = 0; i < hexString.Length; i += 2) {
                var hexChar = hexString.Substring(i, 2);
                sb.Append((char)Convert.ToByte(hexChar, 16));
            }
            return sb.ToString();
        }

        public static string uniCodeToBinary(string unicode_text) {
            return String.Join(String.Empty, Encoding.Unicode.GetBytes(unicode_text).Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0'))); // must ensure 8 digits.
        }

        public static string binaryToUnicode(string binary_text) {
            return Encoding.Unicode.GetString(System.Text.RegularExpressions.Regex.Split(binary_text, "(.{8})").Where(binary => !String.IsNullOrEmpty(binary)).Select(binary => Convert.ToByte(binary, 2)).ToArray());
        }
        
        public static string stringToMD5(string input_str) {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input_str));

            for (int i = 0; i < bytes.Length; i++) {
                hash.Append(bytes[i].ToString("x2"));
            }

            return hash.ToString();
        }

        public static string intToTimeSpan(int time_ms) {
            TimeSpan result = TimeSpan.FromMilliseconds(time_ms);
            return result.ToString("hh':'mm':'ss");
        }


    }
}
