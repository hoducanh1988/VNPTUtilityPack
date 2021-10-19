using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UtilityPack.Utility {
    public class FileToListString {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file_full_name"></param>
        /// <returns></returns>
        public static List<string> Get(string file_full_name) {
            if (!File.Exists(file_full_name)) return null;
            else return File.ReadAllLines(file_full_name).ToList();
        }


    }
}
