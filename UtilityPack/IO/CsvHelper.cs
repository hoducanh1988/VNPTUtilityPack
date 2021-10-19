using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;

namespace UtilityPack.IO {

    public class CsvHelper<T> where T : new() {


        /// <summary>
        /// Save IEnumerable<> data to CSV file
        /// </summary>
        /// <param name="items"></param>
        /// <param name="path"></param>
        public static void ToCsvFile(IEnumerable<T> items, string path, Encoding encoding, bool append) {
            string title = "";
            string line = "";
            bool iswritetitle = false;

            //get all properties in T
            Type itemType = typeof(T);
            var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //get title from property name
            foreach (var p in properties) { title += string.Format("\"{0}\",", p.Name); }
            title = title.Substring(0, title.Length - 1);

            //check write title or not
            if (append == true) { if (!File.Exists(path)) iswritetitle = true; } else iswritetitle = true;

            using (var writer = new StreamWriter(path, append, encoding)) {
                //write title
                if (iswritetitle) writer.WriteLine(title);

                //write content
                foreach (var item in items) {

                    line = "";
                    foreach (var p in properties) { line += string.Format("\"{0}\",", p.GetValue(item, null)); }
                    line = line.Substring(0, line.Length - 1);

                    writer.WriteLine(line);
                }
            }
        }


        /// <summary>
        /// Save IEnumerable<> data to CSV file
        /// </summary>
        /// <param name="items"></param>
        /// <param name="path"></param>
        public static void ToCsvFile_FormatVertical(IEnumerable<T> items, string path, Encoding encoding) {
            string line = "";

            //get all properties in T
            Type itemType = typeof(T);
            var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            using (var writer = new StreamWriter(path, false, encoding)) {
                //write content
                foreach (var item in items) {

                    line = "";
                    foreach (var p in properties) {
                        line = string.Format("\"{0}\",\"{1}\"", p.Name, p.GetValue(item, null));
                        writer.WriteLine(line);
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T FromCsvFile_FormatHorizontal(string path, Encoding encoding, string splitString) {
            string line = "";
            T t = new T();

            //get all properties in T
            Type itemType = typeof(T);
            var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            using (var reader = new StreamReader(path, encoding)) {
                //ignore title
                line = reader.ReadLine();

                //read file
                line = reader.ReadLine();
                string[] buffer = line.Split(new string[] { splitString }, StringSplitOptions.None);

                //save content to object
                for (int i = 0; i < properties.Length; i++) {
                    properties[i].SetValue(t, Convert.ChangeType(buffer[i].ToString().Replace("\"", "").Trim(), properties[i].PropertyType));
                }
            }

            return t;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <param name="splitString"></param>
        /// <returns></returns>
        public static T FromCsvFile_FormatVertical(string path, Encoding encoding, string splitString) {
            string line = "@";
            T t = new T();

            //get all properties in T
            Type itemType = typeof(T);
            var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            using (var reader = new StreamReader(path, encoding)) {
                int i = 0;

                while (reader.Peek() != -1) {
                    //read file
                    line = reader.ReadLine();
                    if (line == "") break;

                    string[] buffer = line.Split(new string[] { splitString }, StringSplitOptions.None);
                    properties[i].SetValue(t, Convert.ChangeType(buffer[1].ToString().Replace("\"", "").Trim(), properties[i].PropertyType));
                    i++;
                }
            }

            return t;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static List<T> FromCsvFile(string path, Encoding encoding, string splitString) {
            string line = "";
            List<T> ts = new List<T>();

            //get all properties in T
            Type itemType = typeof(T);
            var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            using (var reader = new StreamReader(path, encoding)) {
                //ignore title
                line = reader.ReadLine();

                //read file
                while (!reader.EndOfStream) {
                    line = reader.ReadLine();
                    string[] buffer = line.Split(new string[] { splitString }, StringSplitOptions.None);

                    //save content to object
                    var t = new T();
                    for (int i = 0; i < properties.Length; i++) {
                        properties[i].SetValue(t, Convert.ChangeType(buffer[i].ToString().Replace("\"", "").Trim(), properties[i].PropertyType));
                    }

                    //save object to list
                    ts.Add(t);
                }
            }

            return ts;
        }


    }
}
