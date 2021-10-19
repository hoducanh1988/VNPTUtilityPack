using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UtilityPack.Utility {
    public class FileInFolderToListString {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder_path"></param>
        /// <param name="file_extension"></param>
        /// <returns></returns>
        public static List<string> Get(string folder_path, string file_extension) {
            if (!Directory.Exists(folder_path)) return null;
            DirectoryInfo directoryInfo = new DirectoryInfo(folder_path);
            var files = directoryInfo.GetFiles(string.Format("*.{0}", file_extension));
            return files.Select(f => f.Name).ToList();
        }


    }
}
