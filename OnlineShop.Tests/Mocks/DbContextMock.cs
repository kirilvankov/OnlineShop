namespace OnlineShop.Tests.Mocks
{
    using System;

    using Microsoft.EntityFrameworkCore;

    using OnlineShop.Data;

    public static class DbContextMock
    {
        public static ApplicationDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new ApplicationDbContext(dbContextOptions);
            }
        }
    }
}
