namespace Exam_Project.Data
{
    
    using Exam_Project.Data.Models;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ProjectDbContext : IdentityDbContext<User>
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Product>()
                .Property(p => p.Price).HasColumnType("decimal(18,2)");

            builder
               .Entity<OrderItem>()
               .Property(p => p.ProductPrice).HasColumnType("decimal(18,2)");

            builder.Entity<Category>().HasData(
                new Category {Id = 1, Name = "Home" },
                new Category {Id = 2, Name = "Cars" },
                new Category {Id = 3, Name = "Clothes" },
                new Category {Id = 4, Name = "Shoes" },
                new Category {Id = 5, Name = "Toys" },
                new Category {Id = 6, Name = "Health" });

            base.OnModelCreating(builder);
        }
    }
}
