using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace UtilityPack.Utility {

    public class GetFileWriteLatestInFolder {

        string directory = null;
        string[] fileExtensions = null;

        public GetFileWriteLatestInFolder(string _directory, string[] _extensions) {
            this.directory = _directory;
            this.fileExtensions = _extensions;
        }
        
        FileInfo _getFile(string extension) {
            try {
                return new DirectoryInfo(this.directory).GetFiles("*." + extension).OrderByDescending(f => f.LastWriteTime).First();
            } catch {
                return null;
            }
        }


        public string GetFileName() {
            try {
                if (this.fileExtensions.Length == 0) return null;
                if (this.fileExtensions.Length == 1) {
                    return _getFile(this.fileExtensions[0]).Name;
                }
                else {
                    FileInfo f = null;
                    DateTime d = DateTime.MinValue;

                    foreach (var ext in this.fileExtensions) {
                        var file = _getFile(ext);
                        if (DateTime.Compare(d, file.LastWriteTime) < 0) {
                            f = file;
                            d = file.LastWriteTime;
                        }
                    }
                    return f.Name;
                }
            }
            catch {
                return null;
            }
        }


        public FileInfo GetFileInfo() {
            try {
                if (this.fileExtensions.Length == 0) return null;
                if (this.fileExtensions.Length == 1) {
                    return _getFile(this.fileExtensions[0]);
                }
                else {
                    FileInfo f = null;
                    DateTime d = DateTime.MinValue;

                    foreach (var ext in this.fileExtensions) {
                        var file = _getFile(ext);
                        if (DateTime.Compare(d, file.LastWriteTime) > 0) {
                            f = file;
                            d = file.LastWriteTime;
                        }
                    }
                    return f;
                }
            }
            catch {
                return null;
            }
        }

    }
}
