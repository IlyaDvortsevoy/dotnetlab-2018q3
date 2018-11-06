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
                .HasForeignKey(o => o.OCustomer.CId);
            modelBuilder.Entity<Order>()
                .Property(o => o.OId)
                .HasColumnName("order_id");
            modelBuilder.Entity<Order>()
                .Property(o => o.ODate)
                .HasColumnType("datetime")
                .HasColumnName("order_date")
                .IsRequired();
            modelBuilder.Entity<Order>()
                .Property(o => o.OCustomer.CId)
                .HasColumnType("int")
                .HasColumnName("customer_id");


                
                




            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
