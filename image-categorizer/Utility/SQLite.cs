using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_categorizer
{
    public static class SQLite
    {
        /*private static string dbName = String.Format($"{Environment.CurrentDirectory}\\ic.sqlite");*/
        private static string dbName = "D:\\DB\\ic.db";
        private static string dbversion = "3";
        private static string tagTable = "image_tags";
        private static string allAttributes = "file_path TEXT, datetime TEXT, format TEXT, camera_model TEXT, modified_date TEXT";
        private static string connectString = String.Format($"Data Source = {dbName};");
        public static bool isInit = false;
        public static void SQLiteinit()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectString))
            {
                if (!System.IO.File.Exists(dbName))
                {
                    SQLiteConnection.CreateFile(dbName);
                }
                connection.Open();
                string createSql = String.Format($"CREATE TABLE IF NOT EXISTS {tagTable}({allAttributes});");
                SQLiteCommand createCommand = new(createSql, connection);
                int result =  createCommand.ExecuteNonQuery();
            }
            isInit = true;
        }
        public static int InsertQuery(string? filePath, string? dateTime, string? format, string? camera_model, string? modified_date)
        {
            int result = -1;
            string sql = String.Format($"INSERT INTO image_tags VALUES(\'{filePath}\', \'{dateTime}\', \'{format}\', \'{camera_model}\', \'{modified_date}\');");
            using (SQLiteConnection connection = new SQLiteConnection(connectString))
            {
                connection.Open();
                using SQLiteCommand command = new(sql, connection);
                result = command.ExecuteNonQuery();
            }
            return result;
        }
        /// <summary>
        /// select query with no condition
        /// </summary>
        /// <param name="select">dateTime,format,camera_model,modified_date)</param>
        /// <returns>Dictionary(attribute, Datas)</returns>
        public static Dictionary<string, List<string?>>? SelectQuery(string[] select)
        {
            Dictionary<string, List<string?>>? ret = new();
            string attribute = String.Join(",", select);
            string sql = String.Format($"SELECT {attribute} FROM {tagTable};");
            using (SQLiteConnection connection = new(connectString))
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
                            if (item.Value != null) { colunmn = item.Value;  ret[item.Key].Add(colunmn); }
                        }
                    }
                }
            }
            return ret;
        }
    }
}
