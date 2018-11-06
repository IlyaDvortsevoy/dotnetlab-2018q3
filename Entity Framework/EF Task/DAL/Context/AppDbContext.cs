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

            modelBuilder.Entity<Customer>()
                .HasMany<Order>(c => c.COrders)
                .WithRequired(o => o.OCustomer)
                .Map(o => o.MapKey("customer_id"))
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Order>()
                .ToTable("orders")
                .HasKey(o => o.OId);

            modelBuilder.Entity<Order>()
                .Property(o => o.OId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .HasColumnName("order_id");

            modelBuilder.Entity<Order>()
                .Property(o => o.ODate)
                .HasColumnType("datetime")
                .HasColumnName("order_date")
                .IsRequired();

            modelBuilder.Entity<Order>()
                .HasMany<OrderItem>(o => o.OOrderItems)
                .WithRequired(oi => oi.OIOrder)
                .HasForeignKey(oi => oi.OIOrderId)
                .WillCascadeOnDelete(true);

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
                .HasColumnType("decimal")
                .HasColumnName("item_price")
                .IsRequired();

            modelBuilder.Entity<Item>()
                .HasMany<OrderItem>(i => i.IOrderItems)
                .WithRequired(oi => oi.OIItem)
                .HasForeignKey(oi => oi.OIItemId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<OrderItem>()
                .ToTable("order_items")
                .HasKey(oi => new {oi.OIOrderId, oi.OIItemId});

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.OIOrderId)
                .HasColumnName("order_id");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.OIItemId)
                .HasColumnName("item_id");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.OIQuantity)
                .HasColumnType("int")
                .HasColumnName("item_qty");

            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
