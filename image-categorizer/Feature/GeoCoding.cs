﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;

namespace image_categorizer
{
    public class GeoCoding : IGeoCoding
    {

        #region ClassMember
        private string DbName { get; set; }
        private string dbversion;
        private SQLiteConnectionStringBuilder connectString;
        private bool _hasDataBase;
        private Logger TaskLogger { get; set; }
        #endregion ClassMember

        #region Constructor
        public GeoCoding(string baseDirectory, ref Logger taskLogger)
        {
            TaskLogger = taskLogger;
            _hasDataBase = false;
            DbName = $"{baseDirectory}\\Data\\allcountries.db";
            dbversion = "3";
            connectString = new();
            connectString.DataSource = DbName;
        }

        public void GeoCodingInit()
        {
            try
            {
                if (!System.IO.File.Exists(DbName))
                {
                    MessageBox.Show("Not Found Geocoding Data");
                    _hasDataBase = false;
                    return;
                }
                _hasDataBase = true;
                return;
            }
            catch (Exception e)
            {
                TaskLogger.WriteLog(e.Message, isError: true, writeNow: true);
                TaskLogger.ShowLogFile();
            }

        }
        #endregion Constructor

        #region Queries
        public string? GetLocation(double? latitude, double? longitude)
        {
            string? location = "None";
            try
            {
                if (_hasDataBase == false) { return "No Geocode Data"; }
                string getAdmin3Query = String.Format($"SELECT country, admin1, admin2 FROM lite ORDER BY ABS(latitude - {latitude})+ABS(longitude - {longitude}) LIMIT 1;");
                using (SQLiteConnection connection = new SQLiteConnection(connectString.ToString()))
                {
                    connection.Open();
                    using SQLiteCommand GetAdmin3Command = new(getAdmin3Query, connection);
                    using SQLiteDataReader Admin3Reader = GetAdmin3Command.ExecuteReader();
                    string admin2code;
                    if (Admin3Reader.Read())
                    {
                        admin2code = String.Format($"{Admin3Reader["country"]}.{Admin3Reader["admin1"]}.{Admin3Reader["admin2"]}");
                    }
                    else { admin2code = "None"; }
                    if (admin2code.Length > 1)
                    {
                        admin2code = admin2code.Substring(0, admin2code.Length - 1);
                    }

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
                }
            }
            catch (Exception e)
            {
                TaskLogger.WriteLog(e.Message, isError: true, writeNow: true);
                TaskLogger.ShowLogFile();
            }
            return location;

        }
        #endregion Queries
    }
}
