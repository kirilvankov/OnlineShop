namespace OnlineShop.Tests.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using OnlineShop.Data;
    using OnlineShop.Data.Models;

    public static class Utilities
    {
        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            db.Products.AddRange(GetSeedingProducts());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(ApplicationDbContext db)
        {
            db.Products.RemoveRange(db.Products);
            InitializeDbForTests(db);
        }

        public static List<ProductEntity> GetSeedingProducts()
        {
            return new List<ProductEntity>()
            {
                new ProductEntity(){ Id = 1, CategoryId = 1, Name = "TestProduct1", Description = "Test description1", Price = 10.99m, ImageUrl = "testImageUrl1", StoreId = null },
                new ProductEntity(){ Id = 2, CategoryId = 2, Name = "TestProduct2", Description = "Test description2", Price = 11.99m, ImageUrl = "testImageUrl2", StoreId = null },
                new ProductEntity(){ Id = 3, CategoryId = 3, Name = "TestProduct3", Description = "Test description3", Price = 12.99m, ImageUrl = "testImageUrl3", StoreId = null }
            };
        }
    }
}
