using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ApiTest2.Services
{
    public class ConnectData
    {
        string mySQL = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
        //string cdrcalogconnection = ConfigurationManager.ConnectionStrings["cdrcalogconnection"].ConnectionString;
        //string cdrCGlogconnection = ConfigurationManager.ConnectionStrings["cdrCGlogconnection"].ConnectionString;
        public static MySqlConnection mysqlconnect;
        const string PREFIX_VARIABLE_MYSQL = "";
        const bool IsMysqlDatabase = true;
        const bool IsSqlServer = false;
        public ConnectData(int region)
        {
            switch (region)
            {
                case 1:
                    mysqlconnect = new MySqlConnection(mySQL);
                    break;
            }
        }
    }
}