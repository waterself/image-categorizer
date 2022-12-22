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
        private static string connectString = String.Format($"Data Source = {dbName};");
        public static void SQLiteinit()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectString))
            {
                if (!System.IO.File.Exists(dbName))
                {
                    SQLiteConnection.CreateFile(dbName);
                }
                connection.Open();
                string createSql = "CREATE TABLE IF NOT EXISTS image_tags(datetime DATETIME, format varchar(5), camera_model varchar(80), modified_date DATETIME";
                SQLiteCommand createCommand = new(createSql, connection);
            }
        }
        public static void InsertQuery(string VALUES)
        {
            string sql = String.Format($"INSERT INTO ic.imagetags VALUES({VALUES})");
            using (SQLiteConnection connection = new SQLiteConnection(connectString))
            { 
                SQLiteCommand command = new(sql, connection);
                int result = command.ExecuteNonQuery();
            }
        }
    }
}
