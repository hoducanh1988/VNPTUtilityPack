using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace UtilityPack.VNPT {

    public class VNPTTestInfo : INotifyPropertyChanged {
        //implement interface
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public VNPTTestInfo() {
            DAteTime = MachineName = MacAddress = ProductSerial = Operator = TotalResult = ErrorMessage = SystemLog = "--";
        }

        string _datetime;
        public string DAteTime {
            get { return _datetime; }
            set {
                _datetime = value;
                OnPropertyChanged(nameof(DAteTime));
            }
        }
        string _machinename;
        public string MachineName {
            get { return _machinename; }
            set {
                _machinename = value;
                OnPropertyChanged(nameof(MachineName));
            }
        }
        string _softwareversion;
        public string SoftwareVersion {
            get { return _softwareversion; }
            set {
                _softwareversion = value;
                OnPropertyChanged(nameof(SoftwareVersion));
            }
        }
        string _macaddress;
        public string MacAddress {
            get { return _macaddress; }
            set {
                _macaddress = value;
                OnPropertyChanged(nameof(MacAddress));
            }
        }
        string _productserial;
        public string ProductSerial {
            get { return _productserial; }
            set {
                _productserial = value;
                OnPropertyChanged(nameof(ProductSerial));
            }
        }
        string _operator;
        public string Operator {
            get { return _operator; }
            set {
                _operator = value;
                OnPropertyChanged(nameof(Operator));
            }
        }
        string _totalresult;
        public string TotalResult {
            get { return _totalresult; }
            set {
                _totalresult = value;
                OnPropertyChanged(nameof(TotalResult));
            }
        }
        string _errormessage;
        public string ErrorMessage {
            get { return _errormessage; }
            set {
                _errormessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        string _systemlog;
        public string SystemLog {
            get { return _systemlog; }
            set {
                _systemlog = value;
                OnPropertyChanged(nameof(SystemLog));
            }
        }
    }


    public class VNPTTestItemInfo : INotifyPropertyChanged {
        //implement interface
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public VNPTTestItemInfo() {
            LowerLimit = UpperLimit = Value = Result = "--";
        }
        string _lowerlimit;
        public string LowerLimit {
            get { return _lowerlimit; }
            set {
                _lowerlimit = value;
                OnPropertyChanged(nameof(LowerLimit));
            }
        }
        string _uppperlimit;
        public string UpperLimit {
            get { return _uppperlimit; }
            set {
                _uppperlimit = value;
                OnPropertyChanged(nameof(UpperLimit));
            }
        }
        string _value;
        public string Value {
            get { return _value; }
            set {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
        string _result;
        public string Result {
            get { return _result; }
            set {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }
    }


    public class VNPTLogMoreInfo : INotifyPropertyChanged {
        //implement interface
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public VNPTLogMoreInfo() {
            Info1 = Info2 = Info3 = Info4 = Info5 = "--";
        }

        string _info1;
        public string Info1 {
            get { return _info1; }
            set {
                _info1 = value;
                OnPropertyChanged(nameof(Info1));
            }
        }
        string _info2;
        public string Info2 {
            get { return _info2; }
            set {
                _info2 = value;
                OnPropertyChanged(nameof(Info2));
            }
        }
        string _info3;
        public string Info3 {
            get { return _info3; }
            set {
                _info3 = value;
                OnPropertyChanged(nameof(Info3));
            }
        }
        string _info4;
        public string Info4 {
            get { return _info4; }
            set {
                _info4 = value;
                OnPropertyChanged(nameof(Info4));
            }
        }
        string _info5;
        public string Info5 {
            get { return _info5; }
            set {
                _info5 = value;
                OnPropertyChanged(nameof(Info5));
            }
        }
    }

}
