using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;

namespace DapperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string connStr = "server=127.0.0.1;uid=root;pwd=123456;database=db_electricity_network;Pooling=true;Max Pool Size=20;";
            var u = new TUserinfo()
            {
                Username = "asd",
                Password = "123",
                Sex = Convert.ToChar("女"),
                Email = "123qq.com",
                Idcard = "123123123412341234",
                Phone = "18811112222"
            };

            for (int i = 0; i < 30; i++)
            {
                using (IDbConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    //int rs = conn.ExecuteAsync("INSERT INTO t_userinfo (idcard,phone,username,password,sex,email) VALUES (@idcard,@phone,@username,@password,@sex,@email);", u).GetAwaiter().GetResult();
                    var user = conn.QueryFirstOrDefault("SELECT * FROM t_userinfo WHERE id =@id", new { id = 1 });
                    //conn.Close();
                }
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 1000; i++)
            {
                using (IDbConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    //int rs = conn.ExecuteAsync("INSERT INTO t_userinfo (idcard,phone,username,password,sex,email) VALUES (@idcard,@phone,@username,@password,@sex,@email);", u).GetAwaiter().GetResult();
                    var user = conn.QueryFirstOrDefault("SELECT * FROM t_userinfo WHERE id =@id", new { id = 1 });
                    //conn.Close();
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"平均耗时!{Convert.ToSingle(stopwatch.ElapsedMilliseconds) / 1000}ms");
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