namespace OnlineShop.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using OnlineShop.Data;
    using OnlineShop.Data.Enums;
    using OnlineShop.Data.Models;
    using OnlineShop.Services.Models;
    using static OnlineShop.Data.DataConstants;

    public class StoreService : IStoreService
    {
        private readonly ApplicationDbContext _context;
        public StoreService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> Apply(RegisterStoreDto model, string userId, CancellationToken cancellationToken)
        {
            var store = new StoreEntity()
            {
                Name = model.Name,
                Description = model.Description,
                AdditionalInfo = model.AdditionalDetails,
                Status = Status.Pending,
                UserId = userId,
                AddressInfo = new AddressInfoEntity()
                {
                    AddressLine1 = model.AddressInfo.AddressLine1,
                    AddressLine2 = model.AddressInfo.AddressLine2,
                    City = model.AddressInfo.City,
                    Email = model.AddressInfo.Email,
                    PhoneNumber = model.AddressInfo.PhoneNumber,
                    PostCode = model.AddressInfo.PostCode,
                    LocationLat = model.AddressInfo.LocationLat,
                    LocationLng = model.AddressInfo.LocationLng,
                    UserId = userId,
                }
            };
            await _context.AddAsync(store, cancellationToken);
            var res = await _context.SaveChangesAsync();
            return res;
        }

        public async Task<List<StoreDto>> GetStores(CancellationToken cancellationToken)
        {
            return await _context.Stores.Select(s => new StoreDto ()
            {
                Id = s.Id,
                Name = s.Name,
                Description= s.Description,
                AdditionalDetails = s.AdditionalInfo,
                Status = s.Status,
                AddressInfo = new AddressInfoDto ()
                {
                    AddressLine1 = s.AddressInfo.AddressLine1,
                    AddressLine2 = s.AddressInfo.AddressLine2,
                    City = s.AddressInfo.City,
                    Email = s.AddressInfo.Email,
                    PhoneNumber = s.AddressInfo.PhoneNumber,
                    PostCode = s.AddressInfo.PostCode,
                    LocationLat = s.AddressInfo.LocationLat,
                    LocationLng = s.AddressInfo.LocationLng,
                }
            }).ToListAsync();
        }
    }
}
