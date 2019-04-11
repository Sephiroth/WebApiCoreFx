using Microsoft.EntityFrameworkCore;

namespace DBModel.Entity
{
    public partial class db_cdzContext : DbContext
    {
        public db_cdzContext()
        {
        }

        public db_cdzContext(DbContextOptions<db_cdzContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbChargingPile> TbChargingPile { get; set; }
        public virtual DbSet<TbChargingPrice> TbChargingPrice { get; set; }
        public virtual DbSet<TbChargingStation> TbChargingStation { get; set; }
        public virtual DbSet<TbCollection> TbCollection { get; set; }
        public virtual DbSet<TbEvaluation> TbEvaluation { get; set; }
        public virtual DbSet<TbFeedback> TbFeedback { get; set; }
        public virtual DbSet<TbInvoice> TbInvoice { get; set; }
        public virtual DbSet<TbOplist> TbOplist { get; set; }
        public virtual DbSet<TbOrder> TbOrder { get; set; }
        public virtual DbSet<TbOrg> TbOrg { get; set; }
        public virtual DbSet<TbPileBill> TbPileBill { get; set; }
        public virtual DbSet<TbPileSession> TbPileSession { get; set; }
        public virtual DbSet<TbPriceModel> TbPriceModel { get; set; }
        public virtual DbSet<TbRecharge> TbRecharge { get; set; }
        public virtual DbSet<TbRefund> TbRefund { get; set; }
        public virtual DbSet<TbRepair> TbRepair { get; set; }
        public virtual DbSet<TbRepairType> TbRepairType { get; set; }
        public virtual DbSet<TbRole> TbRole { get; set; }
        public virtual DbSet<TbRoleOp> TbRoleOp { get; set; }
        public virtual DbSet<TbSynusers> TbSynusers { get; set; }
        public virtual DbSet<TbTimeModel> TbTimeModel { get; set; }
        public virtual DbSet<TbUser> TbUser { get; set; }
        public virtual DbSet<TbUserOrg> TbUserOrg { get; set; }
        public virtual DbSet<TbUserRole> TbUserRole { get; set; }
        public virtual DbSet<TbWay> TbWay { get; set; }
        public virtual DbSet<TbWithdraw> TbWithdraw { get; set; }
        public virtual DbSet<Test> Test { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=192.168.0.84;uid=root;pwd=zkzl1-1=mysql;database=db_cdz");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbChargingPile>(entity =>
            {
                entity.ToTable("tb_charging_pile");

                entity.HasIndex(e => e.State)
                    .HasName("state_id");

                entity.HasIndex(e => e.StationId)
                    .HasName("station_id");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.ChargeInterface)
                    .HasColumnName("charge_interface")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.OrderType)
                    .HasColumnName("order_type")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Sn)
                    .IsRequired()
                    .HasColumnName("sn")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("int(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.StationId)
                    .IsRequired()
                    .HasColumnName("station_id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.TerminalType)
                    .HasColumnName("terminal_type")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("tinyint(4)");
            });

            modelBuilder.Entity<TbChargingPrice>(entity =>
            {
                entity.ToTable("tb_charging_price");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.ElectricPrice)
                    .HasColumnName("electric_price")
                    .HasColumnType("float(64,4)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ServicePrice)
                    .HasColumnName("service_price")
                    .HasColumnType("float(64,4)");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("int(4)");
            });

            modelBuilder.Entity<TbChargingStation>(entity =>
            {
                entity.ToTable("tb_charging_station");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Desc)
                    .HasColumnName("desc")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Flag)
                    .HasColumnName("flag")
                    .HasColumnType("int(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("double(32,10)");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("double(32,10)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasColumnName("owner")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.ParkingFee)
                    .HasColumnName("parking_fee")
                    .HasColumnType("double(10,0)");

                entity.Property(e => e.Photo)
                    .HasColumnName("photo")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.PowerUnit)
                    .HasColumnName("power_unit")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.PriceTypeId).HasColumnName("price_type_id");

                entity.Property(e => e.PriceUnit)
                    .IsRequired()
                    .HasColumnName("price_unit")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("int(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Worktime)
                    .IsRequired()
                    .HasColumnName("worktime")
                    .HasColumnType("varchar(64)");
            });

            modelBuilder.Entity<TbCollection>(entity =>
            {
                entity.ToTable("tb_collection");

                entity.HasIndex(e => e.StationId)
                    .HasName("station");

                entity.HasIndex(e => e.UserId)
                    .HasName("user2");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Logo)
                    .HasColumnName("logo")
                    .HasColumnType("blob");

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.StationId)
                    .IsRequired()
                    .HasColumnName("station_id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<TbEvaluation>(entity =>
            {
                entity.ToTable("tb_evaluation");

                entity.HasIndex(e => e.OrderId)
                    .HasName("ordereva");

                entity.HasIndex(e => e.StationId)
                    .HasName("stationeva");

                entity.HasIndex(e => e.UserId)
                    .HasName("usereva");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.EvaluateContent)
                    .HasColumnName("evaluate_content")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.EvaluateLever)
                    .HasColumnName("evaluate_lever")
                    .HasColumnType("int(4)");

                entity.Property(e => e.OrderId)
                    .IsRequired()
                    .HasColumnName("order_id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.StationId)
                    .IsRequired()
                    .HasColumnName("station_id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<TbFeedback>(entity =>
            {
                entity.ToTable("tb_feedback");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Opinion)
                    .HasColumnName("opinion")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.OpinionState)
                    .HasColumnName("opinion_state")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<TbInvoice>(entity =>
            {
                entity.ToTable("tb_invoice");

                entity.HasIndex(e => e.UserId)
                    .HasName("inuser");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.InvoiceContent)
                    .HasColumnName("invoice_content")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.InvoiceNum)
                    .HasColumnName("invoice_num")
                    .HasColumnType("int(64)");

                entity.Property(e => e.InvoiceType)
                    .HasColumnName("invoice_type")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ReceiverMail)
                    .HasColumnName("receiver_mail")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ReceiverName)
                    .HasColumnName("receiver_name")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ReceiverPhone)
                    .HasColumnName("receiver_phone")
                    .HasColumnType("int(64)");

                entity.Property(e => e.Remarks)
                    .HasColumnName("remarks")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("int(4)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.TaxNum)
                    .HasColumnName("tax_num")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.TitleType)
                    .HasColumnName("title_type")
                    .HasColumnType("tinyint(2)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<TbOplist>(entity =>
            {
                entity.ToTable("tb_oplist");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Flag)
                    .HasColumnName("flag")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.OpCode)
                    .HasColumnName("op_code")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.OpName)
                    .HasColumnName("op_name")
                    .HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<TbOrder>(entity =>
            {
                entity.ToTable("tb_order");

                entity.HasIndex(e => e.PileId)
                    .HasName("orpile");

                entity.HasIndex(e => e.StationId)
                    .HasName("orstation");

                entity.HasIndex(e => e.UserId)
                    .HasName("oruser");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.ActualElectricCharge)
                    .HasColumnName("actual_electric_charge")
                    .HasColumnType("int(255)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ActualServiceCharge)
                    .HasColumnName("actual_service_charge")
                    .HasColumnType("int(255)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Datail)
                    .HasColumnName("datail")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.DiscountMonetary)
                    .HasColumnName("discount_monetary")
                    .HasColumnType("int(64)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Duration)
                    .HasColumnName("duration")
                    .HasColumnType("double(8,4)");

                entity.Property(e => e.InvoiceState)
                    .HasColumnName("invoice_state")
                    .HasColumnType("int(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.OrderNo)
                    .IsRequired()
                    .HasColumnName("order_no")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.PileId)
                    .IsRequired()
                    .HasColumnName("pile_id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Power)
                    .HasColumnName("power")
                    .HasColumnType("double(64,4)");

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasColumnType("varchar(225)");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.StationId)
                    .IsRequired()
                    .HasColumnName("station_id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.WithdrawalMonetary)
                    .HasColumnName("withdrawal_monetary")
                    .HasColumnType("int(64)");
            });

            modelBuilder.Entity<TbOrg>(entity =>
            {
                entity.ToTable("tb_org");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Flag)
                    .HasColumnName("flag")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.OrgDesc)
                    .HasColumnName("org_desc")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.OrgName)
                    .HasColumnName("org_name")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ParentId)
                    .HasColumnName("parent_id")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("int(4)");
            });

            modelBuilder.Entity<TbPileBill>(entity =>
            {
                entity.ToTable("tb_pile_bill");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.ElectricCharge)
                    .HasColumnName("electric_charge")
                    .HasColumnType("int(64)");

                entity.Property(e => e.Power)
                    .HasColumnName("power")
                    .HasColumnType("int(64)");

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasColumnType("int(4)");

                entity.Property(e => e.ServiceCharge)
                    .HasColumnName("service_charge")
                    .HasColumnType("int(64)");

                entity.Property(e => e.UserCard)
                    .HasColumnName("user_card")
                    .HasColumnType("varchar(64)");
            });

            modelBuilder.Entity<TbPileSession>(entity =>
            {
                entity.ToTable("tb_pile_session");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Gunstate)
                    .HasColumnName("gunstate")
                    .HasColumnType("int(5)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Info)
                    .HasColumnName("info")
                    .HasColumnType("int(4)");

                entity.Property(e => e.Pilestate)
                    .HasColumnName("pilestate")
                    .HasColumnType("int(5)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Power)
                    .HasColumnName("power")
                    .HasColumnType("int(255)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Sessionid)
                    .HasColumnName("sessionid")
                    .HasColumnType("varchar(255)");
            });

            modelBuilder.Entity<TbPriceModel>(entity =>
            {
                entity.ToTable("tb_price_model");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.ElectricPrice)
                    .HasColumnName("electric_price")
                    .HasColumnType("int(64)");

                entity.Property(e => e.ModelSign)
                    .HasColumnName("model_sign")
                    .HasColumnType("int(1)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.ServicePrice)
                    .HasColumnName("service_price")
                    .HasColumnType("int(64)")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<TbRecharge>(entity =>
            {
                entity.ToTable("tb_recharge");

                entity.HasIndex(e => e.UserId)
                    .HasName("reuser");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("int(64)");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Method)
                    .HasColumnName("method")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<TbRefund>(entity =>
            {
                entity.ToTable("tb_refund");

                entity.HasIndex(e => e.UserId)
                    .HasName("refunduser");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrderId)
                    .IsRequired()
                    .HasColumnName("order_id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Photo)
                    .HasColumnName("photo")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Reason)
                    .HasColumnName("reason")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.RefundAmount)
                    .HasColumnName("refund_amount")
                    .HasColumnType("int(64)");

                entity.Property(e => e.Remarks)
                    .HasColumnName("remarks")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<TbRepair>(entity =>
            {
                entity.ToTable("tb_repair");

                entity.HasIndex(e => e.RepairTypeId)
                    .HasName("reptype");

                entity.HasIndex(e => e.StationId)
                    .HasName("repstation");

                entity.HasIndex(e => e.UserId)
                    .HasName("repuser");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Datail)
                    .HasColumnName("datail")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Remark)
                    .HasColumnName("remark")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.RepairTypeId)
                    .IsRequired()
                    .HasColumnName("repair_type_id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Result)
                    .HasColumnName("result")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Sn)
                    .IsRequired()
                    .HasColumnName("sn")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("int(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.StationId)
                    .HasColumnName("station_id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<TbRepairType>(entity =>
            {
                entity.ToTable("tb_repair_type");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<TbRole>(entity =>
            {
                entity.ToTable("tb_role");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Flag)
                    .HasColumnName("flag")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.RoleDesc)
                    .HasColumnName("role_desc")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.RoleName)
                    .HasColumnName("role_name")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.RoleType)
                    .HasColumnName("role_type")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<TbRoleOp>(entity =>
            {
                entity.ToTable("tb_role_op");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Flag)
                    .HasColumnName("flag")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.OpCode)
                    .HasColumnName("op_code")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.OpId)
                    .HasColumnName("op_id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<TbSynusers>(entity =>
            {
                entity.ToTable("tb_synusers");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Flag)
                    .HasColumnName("flag")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LoginName)
                    .IsRequired()
                    .HasColumnName("login_name")
                    .HasColumnType("varchar(120)");

                entity.Property(e => e.NatureName)
                    .IsRequired()
                    .HasColumnName("nature_name")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar(200)");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasColumnName("role_name")
                    .HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<TbTimeModel>(entity =>
            {
                entity.ToTable("tb_time_model");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Duration)
                    .HasColumnName("duration")
                    .HasColumnType("int(64)");

                entity.Property(e => e.ModelSign)
                    .HasColumnName("model_sign")
                    .HasColumnType("int(4)");

                entity.Property(e => e.PriceModelId)
                    .HasColumnName("price_model_id")
                    .HasColumnType("varchar(32)");
            });

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

            modelBuilder.Entity<TbUserOrg>(entity =>
            {
                entity.ToTable("tb_user_org");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Flag)
                    .HasColumnName("flag")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.OrgId)
                    .HasColumnName("org_id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.OrgName)
                    .HasColumnName("org_name")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<TbUserRole>(entity =>
            {
                entity.ToTable("tb_user_role");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Flag)
                    .HasColumnName("flag")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<TbWay>(entity =>
            {
                entity.ToTable("tb_way");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(32)");
            });

            modelBuilder.Entity<TbWithdraw>(entity =>
            {
                entity.ToTable("tb_withdraw");

                entity.HasIndex(e => e.UserId)
                    .HasName("withuser");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.Remarks)
                    .HasColumnName("remarks")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(32)");

                entity.Property(e => e.WithdrawAmount)
                    .HasColumnName("withdraw_amount")
                    .HasColumnType("int(64)");

                entity.Property(e => e.WithdrawTime)
                    .HasColumnName("withdraw_time")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.ToTable("test");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Birthday)
                    .HasColumnName("birthday")
                    .HasColumnType("datetime");

                entity.Property(e => e.Income).HasColumnName("income");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)");
            });
        }
    }
}
