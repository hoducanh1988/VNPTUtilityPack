using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityPack.VNPT {
    public class LogSingle {

        string dirLogSingle = "";
        string fileName = "";

        public LogSingle(string RootLogDirectory, string ProductName, string StationName, int StationIndex, int JigIndex) {
            string _dirTemplate = "";

            //Create RootLogDirectory folder
            if (!Directory.Exists(RootLogDirectory)) Directory.CreateDirectory(RootLogDirectory);
            //Create ProductName Folder
            _dirTemplate = Path.Combine(RootLogDirectory, ProductName);
            if (!Directory.Exists(_dirTemplate)) Directory.CreateDirectory(_dirTemplate);
            //Create StationName Folder
            _dirTemplate = Path.Combine(_dirTemplate, StationName);
            if (!Directory.Exists(_dirTemplate)) Directory.CreateDirectory(_dirTemplate);
            //Create StationIndex Folder
            _dirTemplate = Path.Combine(_dirTemplate, string.Format("Station_{0}", StationIndex));
            if (!Directory.Exists(_dirTemplate)) Directory.CreateDirectory(_dirTemplate);
            //Create JigIndex Folder
            _dirTemplate = Path.Combine(_dirTemplate, string.Format("JIG_{0}", JigIndex));
            if (!Directory.Exists(_dirTemplate)) Directory.CreateDirectory(_dirTemplate);
            //Create dir log single folder
            this.dirLogSingle = Path.Combine(_dirTemplate, "LogSingle");
            if (!Directory.Exists(this.dirLogSingle)) Directory.CreateDirectory(this.dirLogSingle);
            //get file name
            this.fileName = String.Format("{0}_{1}_Station{2}_Jig{3}", ProductName, StationName, StationIndex, JigIndex);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="testInfo"></param>
        /// <returns></returns>
        public bool SaveToFile(VNPTTestInfo testInfo) {
            try {
                VNPTTestInfo info = new VNPTTestInfo();
                info = testInfo;
                this.fileName = string.Format("{0}_{1}_{2}_{3}_{4}.xml", this.fileName, info.MacAddress, DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss"), info.TotalResult);
                string fileFullName = Path.Combine(this.dirLogSingle, this.fileName);

                //remove SystemLog from testInfo
                var property = info.GetType().GetProperty("SystemLog");
                property.SetValue(info, null, null);

                //save to xml file
                IO.XmlHelper<VNPTTestInfo>.ToXmlFile(info, fileFullName);

                return true;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

    }
}
