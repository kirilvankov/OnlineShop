namespace OnlineShop.Services
{
    using System.Threading.Tasks;

    using OnlineShop.Data;
    using OnlineShop.Services.Models;

    public class StoreService : IStoreService
    {
        private readonly ApplicationDbContext _context;
        public StoreService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<string> Apply(RegisterStoreDto model)
        {
            //todo: implementation
            throw new System.NotImplementedException();
        }
    }
}
