using Microsoft.EntityFrameworkCore;
using ZepterTest.Common.Models;

namespace ZepterTest.Common
{
    public class ZepterTestContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<ClientInfo> Clients { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Shop> Shops { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }

        public ZepterTestContext(DbContextOptions<ZepterTestContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,14330;Database=ZepterTest;User Id=sa;Password=zeptertest2137!;TrustServerCertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.ClientInfo)
                    .WithMany(e => e.Orders)
                    .HasForeignKey(e => e.ClientInfoId);
                entity.HasOne(e => e.Shop)
                    .WithMany(e => e.Orders)
                    .HasForeignKey(e => e.ShopId);
                entity.HasMany(e => e.Products)
                    .WithMany(e => e.Orders)
                    .UsingEntity<OrderProduct>(
                        l => l.HasOne<Product>()
                            .WithMany()
                            .HasForeignKey(e => e.ProductCode),
                        r => r.HasOne<Order>()
                            .WithMany()
                            .HasForeignKey(e => e.OrderId)
                    );
            });

            modelBuilder.Entity<ClientInfo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Street).IsRequired();
                entity.Property(e => e.City).IsRequired();
                entity.Property(e => e.PostCode).IsRequired();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductCode);
                entity.Property(e => e.Price).IsRequired();
            });

            modelBuilder.Entity<Shop>().HasKey(e => e.Id);
        }
    }
}
