namespace OnlineShop.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using OnlineShop.Areas.ShopOwner;
    using OnlineShop.Data;
    using OnlineShop.Data.Enums;
    using OnlineShop.Data.Models;
    using OnlineShop.Services.Models;

    public class StoreService : IStoreService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public StoreService(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
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
            await _context.SaveChangesAsync();
            return store.Id;
        }

        public async Task<Status> ApproveStore(int storeId, CancellationToken cancellationToken)
        {
            var store = await _context.Stores.FindAsync(storeId);
            var user = await _userManager.FindByIdAsync(store.UserId);
            var role = await _roleManager.FindByNameAsync(ShopOwnerConstants.RoleName);

            store.Status = Status.Approved;
            await _context.SaveChangesAsync(cancellationToken);

            await _userManager.AddToRoleAsync(user, role.Name);

            return store.Status;
        }
        public async Task<Status> RejectStore(int storeId, CancellationToken cancellationToken)
        {
            var store = await _context.Stores.FindAsync(storeId);
            var user = await _userManager.FindByIdAsync(store.UserId);
            var role = await _roleManager.FindByNameAsync(ShopOwnerConstants.RoleName);

            store.Status = Status.NotApproved;
            await _context.SaveChangesAsync(cancellationToken);

            if(await _userManager.IsInRoleAsync(user, role.Name))
            {
                await _userManager.RemoveFromRoleAsync(user, role.Name);
            }
            return store.Status;
        }

        public async Task<StoreDto> GetStore(int storeId, CancellationToken cancellationToken)
        {
            return await _context.Stores.Where(s => s.Id == storeId)
                                .Select(s => new StoreDto()
                                {
                                    Id = s.Id,
                                    Name = s.Name,
                                    Description = s.Description,
                                    AdditionalDetails = s.AdditionalInfo,
                                    Status = s.Status,
                                    UserId = s.UserId,
                                    AddressInfo = new AddressInfoDto()
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
                                }).FirstOrDefaultAsync(); ;
        }

        public async Task<int?> GetStoreId(string userId, CancellationToken cancellationToken)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

            return store == null ? null : store.Id;
        }

        public async Task<List<StoreDto>> GetStores(CancellationToken cancellationToken)
        {
            return await _context.Stores.Select(s => new StoreDto()
            {
                Id = s.Id,
                Name = s.Name,
                Description= s.Description,
                AdditionalDetails = s.AdditionalInfo,
                Status = s.Status,
                UserId = s.UserId,
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
