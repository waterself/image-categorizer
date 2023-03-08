using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;

namespace image_categorizer
{
    public class GeoCoding
    {
        public GeoCoding()
        { 
            dbName = $"D:\\DB\\Data\\allcountries.db";
            dbversion = "3";
            connectString = String.Format($"Data Source={dbName};");
        }

        private string dbName;
        private string dbversion;
        private string connectString;

        public void GeoCodingInit()
        {
            if (!System.IO.File.Exists(dbName))
            {
                MessageBox.Show("Not Found Geocoding Data");
                return;
            }
        }
        public string? GetLocation(double? latitude, double? longitude)
        {
            string getAdmin3Query = String.Format($"SELECT country, admin1, admin2 FROM lite ORDER BY ABS(latitude - {latitude})+ABS(longitude - {longitude}) LIMIT 1;");
            using (SQLiteConnection connection = new SQLiteConnection(connectString)) {
                connection.Open();
                using SQLiteCommand GetAdmin3Command = new(getAdmin3Query, connection);
                using SQLiteDataReader Admin3Reader = GetAdmin3Command.ExecuteReader();
                string admin2code;
                if (Admin3Reader.Read())
                {
                    admin2code = String.Format($"{Admin3Reader["country"]}.{Admin3Reader["admin1"]}.{Admin3Reader["admin2"]}");
                }
                else { admin2code = "None"; }
                if(admin2code.Length > 1) 
                {
                    admin2code = admin2code.Substring(0, admin2code.Length - 1); 
                }

                string? location = "None";
                while (admin2code.Length > 1)
                {
                    string getAdmin2Query = String.Format($"SELECT altname FROM admin2 WHERE admin2code LIKE '{admin2code}%';");
                    using (SQLiteCommand GetAdmin2Command = new(getAdmin2Query, connection))
                    {
                        using (SQLiteDataReader Admin2Reader = GetAdmin2Command.ExecuteReader()) 
                        {
                            if (Admin2Reader.Read()) { location = Admin2Reader["altname"] as string; break; }
                            else { admin2code = admin2code.Substring(0, admin2code.Length - 1); }
                        }
                    }
                }

                /*                Admin3Reader.Close();
                                GetAdmin3Command.Dispose();
                                Admin2Reader.Close();
                                GetAdmin2Command.Dispose();
                                connection.Dispose();*/
                connection.Dispose();
                return location;
            }
        }
    }
}
