using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace UtilityPack.VNPT {

    public class LogTotal {

        string dirLogTotal = "";
        string fileName = "";

        public LogTotal(string RootLogDirectory, string ProductName, string StationName, int StationIndex, int JigIndex) {
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
            //Create dir log total folder
            this.dirLogTotal = Path.Combine(_dirTemplate, "LogTotal");
            if (!Directory.Exists(this.dirLogTotal)) Directory.CreateDirectory(this.dirLogTotal);
            //get file name
            this.fileName = String.Format("{0}_{1}_Station{2}_Jig{3}_{4}.csv", ProductName, StationName, StationIndex, JigIndex, DateTime.Now.ToString("yyyyMMdd"));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="testInfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool SaveToFile(VNPTTestInfo testInfo, VNPTLogMoreInfo info) {
            try {
                string title = "Date_Time_Create,MacAddress,ProductCode,NhanVien,Infor1,Infor2,Infor3,Infor4,Infor5,TestSubject,LowerLimit,UpperLimit,GiaTriDo,PhanDinh";
                string fileFullName = Path.Combine(this.dirLogTotal, this.fileName);
                bool IsCreateTitle = !File.Exists(fileFullName);

                //write data to file
                using (StreamWriter sw = new StreamWriter(fileFullName, true, Encoding.Unicode)) {
                    //write title
                    if (IsCreateTitle == true) sw.WriteLine(title);

                    foreach (PropertyInfo propertyInfo in testInfo.GetType().GetProperties()) {
                        if (propertyInfo.PropertyType == typeof(VNPTTestItemInfo)) {
                            VNPTTestItemInfo itemInfo = (VNPTTestItemInfo)propertyInfo.GetValue(testInfo, null);

                            if (itemInfo.Result != "--") {
                                string content = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}",
                                                           DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss ffff"),
                                                           testInfo.MacAddress.Replace(":", "").ToUpper().Replace(",", ";"),
                                                           testInfo.ProductSerial.Replace(",", ";"),
                                                           testInfo.Operator.Replace(",", ";"),
                                                           info.Info1,
                                                           info.Info2,
                                                           info.Info3,
                                                           info.Info4,
                                                           info.Info5,
                                                           propertyInfo.Name.Replace(",", ";"),
                                                           itemInfo.LowerLimit.Replace(",", ";"),
                                                           itemInfo.UpperLimit.Replace(",", ";"),
                                                           itemInfo.Value.Replace(",", ";"),
                                                           itemInfo.Result.Replace(",", ";")
                                                           );

                                //write content
                                sw.WriteLine(content);
                            }
                            
                        }
                    }
                }

                return true;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

    }

}
