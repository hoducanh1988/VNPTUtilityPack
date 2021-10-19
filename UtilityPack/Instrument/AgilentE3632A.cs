using System.IO.Ports;
using UtilityPack.Protocol;


namespace UtilityPack.Instrument {
    public class AgilentE3632A {


        IProtocol device;

        public bool IsConnected {
            get {
                if (device == null) return false;
                return device.IsConnected();
            }
        }

        public bool Open() {
            return false;
        }

        public bool Open(string visaaddress) {
            device = new SCPI();
            return device.Open(visaaddress);
        }

        public bool Open(string portname, string baudrate, string databits, Parity parity, StopBits stopbits) {
            device = new UART();
            return device.Open(portname, baudrate, databits, parity, stopbits);
        }

        public bool Open(string ip, string portnumber, string user, string password) {
            return false;
        }

        public bool Close() {
            if (device == null) return false;
            return device.Close();
        }

        public int Preset() {
            return device.WriteLine("*RST") == true ? 0 : 1;
        }

        public int clear_Current_Protection_Flag() {
            return device.WriteLine("CURRent:PROTection:CLEar") == true ? 0 : 1;
        }

        public int clear_Voltage_Protection_Flag() {
            return device.WriteLine("VOLTage:PROTection:CLEar") == true ? 0 : 1;
        }

        public string get_Current_Protection_Flag() {
            return device.Query("CURRent:PROTection:TRIPped?").Replace("\r", "").Replace("\n", "").Trim();
        }

        public string get_Current_Protection_Level() {
            return device.Query("CURRent:PROTection:LEVel?").Replace("\r", "").Replace("\n", "").Trim();
        }

        public string get_Current_Protection_State() {
            return device.Query("CURRent:PROTection:STATe?").Replace("\r", "").Replace("\n", "").Trim();
        }

        public string get_Current_Step() {
            return device.Query("CURR:STEP?").Replace("\r", "").Replace("\n", "").Trim();
        }

        public string get_Output_State() {
            return device.Query("OUTPut:STATe?").Replace("\r", "").Replace("\n", "").Trim();
        }

        public string get_Output_Value() {
            return device.Query("APPLy?").Replace("\r", "").Replace("\n", "").Trim();
        }

        public string get_Trigger_Delay() {
            return device.Query("TRIGger:DELay?").Replace("\r", "").Replace("\n", "").Trim();
        }

        public string get_Voltage_Protection_Flag() {
            return device.Query("VOLTage:PROTection:TRIPped?").Replace("\r", "").Replace("\n", "").Trim();
        }

        public string get_Voltage_Protection_Level() {
            return device.Query("VOLTage:PROTection:LEVel?").Replace("\r", "").Replace("\n", "").Trim();
        }

        public string get_Voltage_Protection_State() {
            return device.Query("VOLTage:PROTection:STATe?").Replace("\r", "").Replace("\n", "").Trim();
        }

        public string get_Voltage_Range() {
            return device.Query("VOLTage:RANGe?").Replace("\r", "").Replace("\n", "").Trim();
        }

        public string get_Voltage_Step() {
            return device.Query("VOLT:STEP?").Replace("\r", "").Replace("\n", "").Trim();
        }

        public int set_Current_Protection_Level(double value) {
            if (!device.WriteLine(string.Format("CURRent:PROTection:LEVel {0}", value))) return 1;

            string data = this.get_Current_Protection_Level();
            double v;
            bool r = double.TryParse(data, out v);
            if (!r) return 1;
            return v == value ? 0 : 1;
        }

        public int set_Current_Protection_State(bool flag) {
            string target = flag == true ? "1" : "0";
            if (!device.WriteLine(string.Format("CURRent:PROTection:STATe {0}", target))) return 1;

            string data = this.get_Current_Protection_State();
            return data == target ? 0 : 1;
        }

        public int set_Current_Step(double value) {
            return device.WriteLine(string.Format("CURR:STEP {0}", value)) == true ? 0 : 1;
        }

        public int set_Output_State(bool flag) {
            string target = flag == true ? "1" : "0";
            if (!device.WriteLine(string.Format("OUTPut:STATe {0}", target))) return 1;

            string data = this.get_Output_State();
            return data == target ? 0 : 1;
        }

        public int set_Output_Value(double volt, double current) {
            if (!device.WriteLine(string.Format("APPLy {0},{1}", volt, current))) return 1;

            string data = this.get_Output_Value().Split(',')[0];
            return data.Contains(volt.ToString()) ? 0 : 1;
        }

        public int set_Trigger_Delay(double value) {
            return device.WriteLine(string.Format("TRIGger:DELay {0}", value)) == true ? 0 : 1;
        }

        public int set_Voltage_Protection_Level(double value) {
            if (!device.WriteLine(string.Format("VOLTage:PROTection:LEVel {0}", value))) return 1;

            string data = this.get_Voltage_Protection_Level();
            double v;
            bool r = double.TryParse(data, out v);
            if (!r) return 1;
            return v == value ? 0 : 1;
        }

        public int set_Voltage_Protection_State(bool flag) {
            string target = flag == true ? "1" : "0";
            if (!device.WriteLine(string.Format("VOLTage:PROTection:STATe {0}", target))) return 1;

            string data = this.get_Voltage_Protection_State();
            return data == target ? 0 : 1;
        }

        public int set_Voltage_Range(double value) {
            string target = value >= 15 ? "P30V" : "P15V";
            if (!device.WriteLine(string.Format("VOLTage:RANGe {0}", target))) return 1;

            string data = this.get_Voltage_Range();
            return data == target ? 0 : 1;
        }

        public int set_Voltage_Step(double value) {
            return device.WriteLine(string.Format("VOLT:STEP {0}", value)) == true ? 0 : 1;
        }

        public string get_Current_Limit_Value() {
            return device.Query("CURRent?").Replace("\r", "").Replace("\n", "").Trim();
        }

        public string get_Current_Actual_Value() {
            return device.Query("MEASure:CURRent:DC?").Replace("\r", "").Replace("\n", "").Trim();
        }

        public string get_Voltage_Limit_Value() {
            return device.Query("VOLTage?").Replace("\r", "").Replace("\n", "").Trim();
        }

        public string get_Voltage_Actual_Value() {
            return device.Query("MEASure:VOLTage:DC?").Replace("\r", "").Replace("\n", "").Trim();
        }


    }
}
