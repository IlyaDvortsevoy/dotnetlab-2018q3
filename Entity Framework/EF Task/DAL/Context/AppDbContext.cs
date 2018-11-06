using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Reflection;
using DAL.Entities;

namespace DAL.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("InternationWidgetsContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, Migrations.Configuration>(true));
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //TODO: setup dbcontext configuration based on "InternationWidgets.mdf" file using EF Fluent API. DO NOT modify any files from "Entities" folder.

            modelBuilder.Entity<Customer>()
                .ToTable("customers")
                .HasKey(c => c.CId);

            modelBuilder.Entity<Customer>()
                .Property(c => c.CId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("customer_id");

            modelBuilder.Entity<Customer>()
                .Property(c => c.CAddress)
                .HasColumnType("nvarchar(MAX)")
                .HasColumnName("customer_address")
                .IsOptional();

            modelBuilder.Entity<Customer>()
                .Property(c => c.CCity)
                .HasColumnType("nvarchar(MAX)")
                .HasColumnName("customer_city")
                .IsOptional();

            modelBuilder.Entity<Customer>()
                .Property(c => c.CName)
                .HasColumnType("nvarchar(MAX)")
                .HasColumnName("customer_name")
                .IsOptional();

            modelBuilder.Entity<Customer>()
                .Property(c => c.CState)
                .HasColumnType("nvarchar(MAX)")
                .HasColumnName("customer_state")
                .IsOptional();



            modelBuilder.Entity<Order>()
                .ToTable("orders")
                .HasKey(o => o.OId)
                .HasRequired<Customer>(o => o.OCustomer)
                .WithMany(c => c.COrders)
                .Map(m => m.MapKey("fk"));

            modelBuilder.Entity<Order>()
                .Property(o => o.OId)
                .HasColumnName("order_id");

            modelBuilder.Entity<Order>()
                .Property(o => o.ODate)
                .HasColumnType("datetime")
                .HasColumnName("order_date")
                .IsRequired();

            modelBuilder.Entity<Order>()
                .Property(o => o.OCustomer)
                .HasColumnType("int")
                .HasColumnName("customer_id");

            modelBuilder.Entity<Item>()
                .ToTable("items")
                .HasKey(i => i.IId);

            modelBuilder.Entity<Item>()
                .Property(i => i.IId)
                .HasColumnName("item_id");

            modelBuilder.Entity<Item>()
                .Property(i => i.IDescription)
                .HasColumnType("nvarchar(MAX)")
                .HasColumnName("item_description")
                .IsOptional();

            modelBuilder.Entity<Item>()
                .Property(i => i.IPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("item_price")
                .IsRequired();

            modelBuilder.Entity<OrderItem>()
                .HasRequired<Order>(oi => oi.OIOrder)
                .WithMany(o => o.OOrderItems)
                .HasForeignKey<int>(oi => oi.OIOrderId);

            modelBuilder.Entity<OrderItem>()
                .HasRequired<Item>(oi => oi.OIItem)
                .WithMany(i => i.IOrderItems)
                .HasForeignKey<int>(oi => oi.OIItemId);

            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
