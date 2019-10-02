using SqlSugar;

namespace Surging.EFCore.DBModel.SqlSugar
{
    public class SqlClient
    {
        public static string ConnectionString;

        public SqlClient() { }

        public SqlClient(string connStr)
        {
            ConnectionString = connStr;
        }

        public static SqlSugarClient GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConnectionString,
                DbType = DbType.MySql,
                IsAutoCloseConnection = false,
                InitKeyType = InitKeyType.Attribute
            });
            db.Open();
            return db;
        }
    }
}
