using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace DBModel.Entity
{
    public partial class db_cdzContext : DbContext
    {
        /// <summary>
        /// dbContext连接字符串
        /// </summary>
        public static string DbConnStr;
        static db_cdzContext()
        {
        }

        public db_cdzContext()
        {
        }

        public db_cdzContext(DbContextOptions<db_cdzContext> options)
            : base(options)
        {
        }
        
        public virtual DbSet<TbUser> TbUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql(DbConnStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbUser>(entity =>
            {
                entity.ToTable("tb_user");

                entity.HasIndex(e => e.Phone)
                    .HasName("phone")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.CarNum)
                    .HasColumnName("car_num")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.DiscountBalance)
                    .HasColumnName("discount_balance")
                    .HasColumnType("int(64)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Nickname)
                    .IsRequired()
                    .HasColumnName("nickname")
                    .HasColumnType("varchar(10)");

                entity.Property(e => e.Openid)
                    .HasColumnName("openid")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Photo)
                    .HasColumnName("photo")
                    .HasColumnType("varchar(256)");

                entity.Property(e => e.Pwd)
                    .HasColumnName("pwd")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.WechatId)
                    .HasColumnName("wechat_id")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.WithdrawalBalance)
                    .HasColumnName("withdrawal_balance")
                    .HasColumnType("int(64)")
                    .HasDefaultValueSql("'0'");
            });
        }
    }
}
