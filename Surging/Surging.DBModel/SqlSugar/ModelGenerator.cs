using SqlSugar;
using Surging.EFCore.DBModel.SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.DBModel.SqlSugar
{
    public class ModelGenerator
    {
        static void Main()
        {
            SqlClient.ConnectionString = "server=192.168.0.82;uid=root;pwd=zkzl1-1=mysql;database=db_electricity_network;";
            SqlSugarClient db = SqlClient.GetInstance();
            db.DbFirst.CreateClassFile(@"F:\cache\models", "EN.SqlSugar.DBModel.Models");
        }
    }
}