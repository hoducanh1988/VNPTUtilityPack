using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;

using ADOX;
using ADODB;

using UtilityPack.Converter;

namespace UtilityPack.Protocol {

    public class MSAccessDB {


        //--------------------------------------------//
        //1. Create Access DB and table
        //2. Open connection
        //3. Insert data to table
        //4. Insert list data to table
        //5. Read data from table
        //6. Update data to table
        //7. Delete data
        //8. Select data
        //9. Close connection
        //--------------------------------------------//


        const int adStateClosed = 0; //Indicates that the object is closed.
        const int adStateOpen = 1; //Indicates that the object is open.
        const int adStateConnecting = 2; //Indicates that the object is connecting.
        const int adStateExecuting = 4; //Indicates that the object is executing a command.
        const int adStateFetching = 8; //Indicates that the rows of the object are being retrieved.


        private string provider = "Microsoft.ACE.OLEDB.12.0"; //Access 2003 = Microsoft.Jet.OLEDB.4.0, Access 2007,10,13,16 = Microsoft.ACE.OLEDB.12.0
        private string conString = "";
        private string dbString = "";
        private string msDatabasePath = "";

        ADODB.Connection con; //connection
        ADODB.Command cmd; //command
        ADODB.Recordset rs; //recordset

        public bool IsConnected = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbPath"></param>
        /// <param name="dbName"></param>
        public MSAccessDB(string dbPath, string dbName) {
            this.msDatabasePath = Path.Combine(dbPath, dbName);
            this.dbString = string.Format("Provider={0};Data Source={1};Jet OLEDB:Engine Type=5", provider, this.msDatabasePath);
            this.conString = string.Format("Provider={0};Data Source={1};Persist Security Info=False", provider, this.msDatabasePath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbFullName"></param>
        public MSAccessDB(string dbFullName) {
            this.msDatabasePath = dbFullName;
            this.dbString = string.Format("Provider={0};Data Source={1};Jet OLEDB:Engine Type=5", provider, this.msDatabasePath);
            this.conString = string.Format("Provider={0};Data Source={1};Persist Security Info=False", provider, this.msDatabasePath);
        }


        /// <summary>
        /// Create access database; True = Success, False = Fail
        /// </summary>
        /// <returns></returns>
        public bool CreateDB() {
            ADOX.CatalogClass cat = new ADOX.CatalogClass();
            bool r = true;

            try {
                cat.Create(this.dbString);
                Thread.Sleep(100);

                r = File.Exists(this.msDatabasePath);
            } catch { r = false; }

            cat = null;
            return r;
        }


        /// <summary>
        /// Create access database table; True = Success, False = Fail
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table_name"></param>
        /// <returns></returns>
        public bool CreateTable<T>(string table_name) {
            bool r = true;
            CatalogClass cat = openDatabase();

            //Get properties of T
            Type itemType = typeof(T);
            var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            try {
                //Create the table and it's fields. 
                ADOX.Table table = new ADOX.Table();
                table.Name = table_name;

                //Add column to the table.
                foreach (var p in properties) {
                    table.Columns.Append(tableField(p.Name, cat, myConverter.FromVSTypeToTableAccessDataType(p.PropertyType.Name.ToString())));
                }

                //Add the table to our database
                cat.Tables.Append(table);

                // Close the connection to the database after we are done creating it and adding the table to it.
                con = (ADODB.Connection)cat.ActiveConnection;
                if (con != null && con.State != 0) con.Close();

            } catch { r = false; }
            cat = null;
            return r;
        }


        /// <summary>
        /// Create access data to path; True = Success, False = Fail
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public bool CreateDBAndTable<T>(string table_name) {
            ADOX.CatalogClass cat = new ADOX.CatalogClass();
            bool r = true;

            //Get properties of T
            Type itemType = typeof(T);
            var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            try {
                //Create microsoft database
                cat.Create(this.dbString);
                
                //Create the table and it's fields. 
                ADOX.Table table = new ADOX.Table();
                table.Name = table_name;

                //Add column to the table.
                foreach (var p in properties) {
                    table.Columns.Append(tableField(p.Name, cat, myConverter.FromVSTypeToTableAccessDataType(p.PropertyType.Name.ToString())));
                }

                //Add the table to our database
                cat.Tables.Append(table);

                // Close the connection to the database after we are done creating it and adding the table to it.
                con = (ADODB.Connection)cat.ActiveConnection;
                if (con != null && con.State != 0) con.Close();
                
            } catch { r = false; }
            cat = null;
            return r;
        }


        /// <summary>
        /// Open connection to access database; True = Success, False = Fail
        /// </summary>
        /// <returns></returns>
        public bool OpenConnection() {
            bool r = true;
            if (con != null && con.State == 1) goto END;
            
            con = new ADODB.ConnectionClass();
            con.ConnectionString = this.conString;

            try {
                con.Open();
                r = con.State == 1;
            } catch { r = false; }
            goto END;

        END:
            IsConnected = r;
            return r;
        }


        /// <summary>
        /// Close connection to access database; True = Success, False = Fail
        /// </summary>
        /// <returns></returns>
        public bool Close() {
            bool r = true;
            if (con == null || con.State == 0) goto END;

            try {
                con.Close();
                r = con.State == 0;
            } catch { r = false; }
            goto END;

        END:
            IsConnected = false;
            return r;
        }


        /// <summary>
        /// Insert data to table; True = Success, False = Fail
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="table_name"></param>
        /// <returns></returns>
        public bool InsertDataToTable<T> (T t, string table_name) {
            if (con == null || con.State != 1) this.OpenConnection();
            if (con.State != 1) return false;

            //Get properties of T
            Type itemType = typeof(T);
            var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //get field and value
            string s1 = "", s2 = "";
            foreach (var p in properties) {
                s1 += string.Format("[{0}],", p.Name);
                s2 += string.Format("'{0}',", p.GetValue(t, null));
            }
            s1 = s1.Substring(0, s1.Length - 1);
            s2 = s2.Substring(0, s2.Length - 1);

            //Assign the connection to the command 
            cmd = new CommandClass();
            cmd.ActiveConnection = con;
            cmd.CommandType = CommandTypeEnum.adCmdText;

            cmd.CommandText = string.Format("INSERT INTO {0}({1}) VALUES({2})", table_name, s1, s2);

            //Execute the command 
            object nRecordsAffected = Type.Missing;
            object oParams = Type.Missing;
            cmd.Execute(out nRecordsAffected, ref oParams, (int)ADODB.ExecuteOptionEnum.adExecuteNoRecords);

            cmd = null;
            return true;
        }



        /// <summary>
        /// Insert data to table and ignored column tb_ID; True = Success, False = Fail
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="table_name"></param>
        /// <returns></returns>
        public bool InsertDataToTable<T> (T t, string table_name, string ignore_column_name) {
            if (con == null || con.State != 1) this.OpenConnection();
            if (con.State != 1) return false;

            //Get properties of T
            Type itemType = typeof(T);
            var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //get field and value
            string s1 = "", s2 = "";
            foreach (var p in properties) {
                if (!p.Name.ToLower().Equals(ignore_column_name.ToLower())) {
                    s1 += string.Format("[{0}],", p.Name);
                    s2 += string.Format("'{0}',", p.GetValue(t, null));
                }
            }
            s1 = s1.Substring(0, s1.Length - 1);
            s2 = s2.Substring(0, s2.Length - 1);

            //Assign the connection to the command 
            cmd = new CommandClass();
            cmd.ActiveConnection = con;
            cmd.CommandType = CommandTypeEnum.adCmdText;

            cmd.CommandText = string.Format("INSERT INTO {0}({1}) VALUES({2})", table_name, s1, s2);

            //Execute the command 
            object nRecordsAffected = Type.Missing;
            object oParams = Type.Missing;
            cmd.Execute(out nRecordsAffected, ref oParams, (int)ADODB.ExecuteOptionEnum.adExecuteNoRecords);

            cmd = null;
            return true;
        }





        /// <summary>
        /// Insert data list to table; True = Success, False = Fail
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="table_name"></param>
        /// <returns></returns>
        public bool InsertIEnumerableDataToTable<T> (IEnumerable<T> ts, string table_name) {
            if (con == null || con.State != 1) this.OpenConnection();
            if (con.State != 1) return false;

            //Get properties of T
            Type itemType = typeof(T);
            var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //Assign the connection to the command 
            cmd = new CommandClass();
            cmd.ActiveConnection = con;
            cmd.CommandType = CommandTypeEnum.adCmdText;

            foreach (var t in ts) {
                
                //get field and value
                string s1 = "", s2 = "";
                foreach (var p in properties) {
                    s1 += string.Format("[{0}],", p.Name);
                    s2 += string.Format("'{0}',", p.GetValue(t, null));
                }
                s1 = s1.Substring(0, s1.Length - 1);
                s2 = s2.Substring(0, s2.Length - 1);

                //
                cmd.CommandText = string.Format("INSERT INTO {0} VALUES({1})", table_name, s2);

                //Execute the command 
                object nRecordsAffected = Type.Missing;
                object oParams = Type.Missing;
                cmd.Execute(out nRecordsAffected, ref oParams, (int)ADODB.ExecuteOptionEnum.adExecuteNoRecords);
            }

            cmd = null;
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public T QueryDataReturnObject<T>(string queryString) where T : class, new() {
            if (con == null || con.State != 1) this.OpenConnection();
            if (con.State != 1) return null;

            //Get properties of T
            Type itemType = typeof(T);
            var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //open recordset
            rs = new ADODB.Recordset();
            rs.Open(queryString, con);

            //get data
            T t = new T();

            object[,] dataRows = (object[,])rs.GetRows(1, 0);
            object[] p = Enumerable.Range(0, dataRows.GetLength(0)).Select(x => dataRows[x, 0]).ToArray();

            for (int i = 0; i < properties.Length; i++) {
                properties[i].SetValue(t, Convert.ChangeType(p[i], properties[i].PropertyType));
            }

            //close recordset
            rs.Close();
            rs = null;

            //return data
            return t;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public List<T> QueryDataReturnListObject<T>(string queryString) where T : new() {
            if (con == null || con.State != 1) this.OpenConnection();
            if (con.State != 1) return null;

            //Get properties of T
            Type itemType = typeof(T);
            var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //open recordset
            rs = new ADODB.Recordset();
            rs.Open(queryString, con);

            //get data
            object[,] dataRows = (object[,])rs.GetRows();
            List<T> ts = new List<T>();

            for (int i = 0; i < dataRows.GetLength(1); i++) {
                object[] p = Enumerable.Range(0, dataRows.GetLength(0)).Select(x => dataRows[x, i]).ToArray();

                T t = new T();
                for (int k = 0; k < properties.Length; k++) {
                    properties[k].SetValue(t, Convert.ChangeType(p[k], properties[k].PropertyType));
                }
                ts.Add(t);
            }

            //close recordset
            rs.Close();
            rs = null;

            //return data
            return ts;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public List<double> QueryDataReturnListDouble(string queryString) {
            if (con == null || con.State != 1) this.OpenConnection();
            if (con.State != 1) return null;

            //open recordset
            rs = new ADODB.Recordset();
            rs.Open(queryString, con);

            //get data
            object[,] dataRows = (object[,])rs.GetRows();
            List<double> ts = new List<double>();

            for (int i = 0; i < dataRows.GetLength(1); i++) {
                object[] p = Enumerable.Range(0, dataRows.GetLength(0)).Select(x => dataRows[x, i]).ToArray();
                ts.Add((double)p[0]);
            }

            //close recordset
            rs.Close();
            rs = null;

            //return data
            return ts;
        }


        /// <summary>
        /// Delete Row from access table
        /// </summary>
        /// <returns></returns>
        public bool QueryDeleteOrUpdate(string queryString) {
            if (con == null || con.State != 1) this.OpenConnection();
            if (con.State != 1) return false;

            //Assign the connection to the command 
            cmd = new CommandClass();
            cmd.ActiveConnection = con;
            cmd.CommandType = CommandTypeEnum.adCmdText;

            //delete row
            cmd.CommandText = queryString;

            //Execute the command 
            object nRecordsAffected = Type.Missing;
            object oParams = Type.Missing;
            cmd.Execute(out nRecordsAffected, ref oParams, (int)ADODB.ExecuteOptionEnum.adExecuteNoRecords);

            cmd = null;
            return true;
        }


        #region support function

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        CatalogClass openDatabase() {
            CatalogClass catalog = new CatalogClass();
            con = new Connection();
            try {
                con.Open(this.conString);
                catalog.ActiveConnection = con;
            } catch { catalog.Create(this.conString); }
            return catalog;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="catalog"></param>
        /// <returns></returns>
        ADOX.ColumnClass tableIDField(string colName, ADOX.CatalogClass catalog) {
            ADOX.ColumnClass column = new ADOX.ColumnClass();
            column.Name = colName; // The name of the column
            column.ParentCatalog = catalog;
            column.Type = ADOX.DataTypeEnum.adInteger; //Indicates a four byte signed integer.
            column.Properties["AutoIncrement"].Value = true; //Enable the auto increment property for this column.
            return column;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="catalog"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        ADOX.ColumnClass tableField(string colName, ADOX.CatalogClass catalog, ADOX.DataTypeEnum dataType) {
            ADOX.ColumnClass column = new ADOX.ColumnClass();
            column.Name = colName; // The name of the column
            column.ParentCatalog = catalog;
            column.Type = dataType;
            return column;
        }

        #endregion

    }
}
