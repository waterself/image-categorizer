using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using image_categorizer.MVVM.Model;
using image_categorizer.MVVM.ViewModel;
using System.IO;

namespace image_categorizer
{
    public class IcTagSql
    {
        public IcTagSql(string baseDirectory)
        {
            dbFolder = $"{baseDirectory}\\Data";
            dbName = $"{baseDirectory}\\Data\\ic.db";
            tagTable = "image_tags";
            allAttributes = "file_path TEXT, datetime TEXT, format TEXT, camera_model TEXT, location TEXT , modified_date TEXT";
            //connectString = String.Format($"Data Source={dbName};Password={Properties.Settings.Default.IcTagDBPassword}");
            connectString = new();
            connectString.DataSource = dbName;
            connectString.SyncMode = SynchronizationModes.Off;
            connectString.JournalMode = SQLiteJournalModeEnum.Memory;
            //connectString.TextPassword = Properties.Settings.Default.IcTagDBPassword;
            isInit = false;
        }
        private string dbFolder;
        private string dbName;
        private string tagTable;
        private string allAttributes;
        //private string connectStringString;
        private SQLiteConnectionStringBuilder connectString;
        public bool isInit = false;
        public void SQLiteinit()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectString.ToString()))
            {
                if (!System.IO.File.Exists(dbName))
                {
                    DirectoryInfo di = new(dbFolder);
                    if (di.Exists == false) { 
                        di.Create();
                    }
                    SQLiteConnection.CreateFile(dbName);
                }
                connection.Open();
                //connection.ChangePassword(Properties.Settings.Default.IcTagDBPassword);
                string createSql = String.Format($"CREATE TABLE IF NOT EXISTS {tagTable}({allAttributes});");
                SQLiteCommand createCommand = new(createSql, connection);
                int result = createCommand.ExecuteNonQuery();
            }
            isInit = true;
        }
        //Need Location Data
        public void InsertQuery(InsertQueryModel queryModel)
        {
            int result = -1;
            //need generation
            string sql = String.Format($"INSERT INTO image_tags VALUES(\'{queryModel.fileName}\', \'{queryModel.dateTime}\', \'{queryModel.format}\', \'{queryModel.cameraModel}\', \'{queryModel.location}', \'{queryModel.currentTime}\');");
            using (SQLiteConnection connection = new SQLiteConnection(connectString.ToString()))
            {
                connection.Open();
                using SQLiteCommand command = new(sql, connection);
                result = command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void InsertQuery(List<InsertQueryModel> queryModels)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectString.ToString()))
            {
                connection.Open();
                foreach (InsertQueryModel insertQuery in queryModels)
                {
                    string sql = String.Format($"INSERT INTO image_tags VALUES(\'{insertQuery.fileName}\', \'{insertQuery.dateTime}\', \'{insertQuery.format}\', \'{insertQuery.cameraModel}\', \'{insertQuery.location}', \'{insertQuery.currentTime}\');");
                    using SQLiteCommand command = new(sql, connection);
                    command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }
        /// <summary>
        /// select query with no condition
        /// </summary>
        /// <param name="select">dateTime,format,camera_model,modified_date)</param>
        /// <returns>Dictionary(attribute, Datas)</returns>
        public Dictionary<string, List<string?>>? SelectQuery(string[] select)
        {
            Dictionary<string, List<string?>>? ret = new();
            string attribute = String.Join(",", select);
            string sql = String.Format($"SELECT {attribute} FROM {tagTable};");
            using (SQLiteConnection connection = new(connectString.ToString()))
            {
                connection.Open();
                using SQLiteCommand command = new(sql, connection);
                using SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read()) //for one row
                {
                    Dictionary<string, string?> attributeValue = new();
                    for (int i = 0; i < select.Length; i++) // for attribute
                    {
                        attributeValue.Add(select[i], reader[select[i]] as string);
                        if (!ret.ContainsKey(select[i]))
                        {
                            ret.Add(select[i], new List<string?>());
                        }
                    }
                    foreach (KeyValuePair<string, string?> item in attributeValue)
                    {
                        if (ret.ContainsKey(item.Key))
                        {
                            string? colunmn = "";
                            if (item.Value != null) { colunmn = item.Value; ret[item.Key].Add(colunmn); }
                        }
                    }
                }
                connection.Close();
            }
            return ret;
        }
    }
}
