using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DapperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string connStr = "server=192.168.0.82;uid=root;pwd=zkzl1-1=mysql;database=db_electricity_network;Pooling=true;Max Pool Size=20;";
            using (IDbConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var u = new TUserinfo()
                {
                    Username = "asd",
                    Password = "123",
                    Sex = Convert.ToChar("女"),
                    Email = "123qq.com",
                    Idcard = "123123123412341234",
                    Phone = "18811112222"
                };
                int rs = conn.ExecuteAsync("INSERT INTO t_userinfo (idcard,phone,username,password,sex,email) VALUES (@idcard,@phone,@username,@password,@sex,@email);", u).GetAwaiter().GetResult();
                //conn.QuerySingle
                //conn.Close();
            }
            Console.WriteLine();
        }
    }

    public partial class TUserinfo
    {
        /// <summary>
        /// 自增主键ID
        /// </summary>
        [Key]
        public int Id { get; set; } // int(32)
        public string Idcard { get; set; } // char(18)
        public string Phone { get; set; } // varchar(13)
        public string Username { get; set; } // varchar(20)
        public string Password { get; set; } // varchar(50)
        public char? Sex { get; set; } // enum('男','女')
        public string Email { get; set; } // varchar(30)
    }
}