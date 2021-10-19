using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityPack.VNPT {
    public class LogDetail {

        string dirLogDetail = "";
        string fileName = "";

        public LogDetail(string RootLogDirectory, string ProductName, string StationName, int StationIndex, int JigIndex) {
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
            //Create dir log detail folder
            this.dirLogDetail = Path.Combine(_dirTemplate, "LogDetail");
            if (!Directory.Exists(this.dirLogDetail)) Directory.CreateDirectory(this.dirLogDetail);
            //get file name
            this.fileName = String.Format("{0}_{1}_Station{2}_Jig{3}", ProductName, StationName, StationIndex, JigIndex);
        }


        /// <summary>
        /// Function lưu kết quả test của sản phẩm vào log detail (định dạng theo yêu cầu BCN).
        /// </summary>
        /// <param name="SoftwareVersion"></param>
        /// <param name="MacAddress"></param>
        /// <param name="TotalResult"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SaveToFile(VNPTTestInfo testInfo) {
            try {
                testInfo.MacAddress = testInfo.MacAddress == null || testInfo.MacAddress == "" || testInfo.MacAddress == string.Empty ? "NULL" : testInfo.MacAddress.Replace(":","");
                this.fileName = string.Format("{0}_{1}_{2}_{3}_{4}.txt", this.fileName, testInfo.MacAddress, DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss"), testInfo.TotalResult);
                string fileFullName = Path.Combine(this.dirLogDetail, this.fileName);

                using (StreamWriter sw = new StreamWriter(fileFullName, true, Encoding.Unicode)) {
                    sw.WriteLine(testInfo.SoftwareVersion);
                    sw.WriteLine(testInfo.SystemLog);
                }

                return true;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

    }
}
