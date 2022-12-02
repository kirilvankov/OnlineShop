namespace OnlineShop.Data
{
    
    using OnlineShop.Data.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductEntity> Products { get; set; }

        public DbSet<CategoryEntity> Categories { get; set; }

        public DbSet<OrderEntity> Orders { get; set; }

        public DbSet<OrderProductEntity> OrderProducts { get; set; }

        public DbSet<AddressInfoEntity> AddressInfo { get; set; }

        public DbSet<StoreEntity> Stores { get; set; }

        public DbSet<OrderAddressEntity> OrderAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<ProductEntity>()
                .Property(p => p.Price).HasColumnType("decimal(18,2)");

            builder.Entity<StoreEntity>()
                .HasOne(x => x.AddressInfo)
                .WithOne(x => x.Store)
                .HasForeignKey<AddressInfoEntity>(x => x.StoreId)
                .IsRequired(false);


            builder.Entity<CategoryEntity>().HasData(
                new CategoryEntity {Id = 1, Name = "Home" },
                new CategoryEntity {Id = 2, Name = "Cars" },
                new CategoryEntity {Id = 3, Name = "Clothes" },
                new CategoryEntity {Id = 4, Name = "Shoes" },
                new CategoryEntity {Id = 5, Name = "Toys" },
                new CategoryEntity {Id = 6, Name = "Health" });

            base.OnModelCreating(builder);
        }
    }
}
