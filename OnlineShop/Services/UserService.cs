namespace OnlineShop.Services
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    using OnlineShop.Data;
    using OnlineShop.Data.Models;
    using OnlineShop.Services.Models;

    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<OrderUserDetailsDto> GetUserDetails(string userId, CancellationToken cancellationToken)
        {
           return await _dbContext.Users.Where(x => x.Id == userId).Select(x => new OrderUserDetailsDto()
           {
               Id = x.Id,
               FirstName = x.FirstName,
               LastName = x.LastName,
               AddressInfo = new AddressInfoDto()
               {
                   AddressLine1 = x.Address.AddressLine1,
                   AddressLine2 = x.Address.AddressLine2,
                   PhoneNumber = x.Address.PhoneNumber,
                   PostCode = x.Address.PostCode,
                   Email = x.Address.Email,
                   City = x.Address.City,
                   LocationLat = x.Address.LocationLat,
                   LocationLng = x.Address.LocationLng,
               },
           }).FirstOrDefaultAsync(cancellationToken);
        }

        public void SetUserAddress(AddressInfoDto address, string userId)
        {
            var addressUser = _dbContext.AddressInfo.Where(x => x.User.Id == userId).FirstOrDefault();
            var addressinfo = new AddressInfoEntity()
            {
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                PhoneNumber = address.PhoneNumber,
                City = address.City,
                PostCode = address.PostCode,
                Email = address.Email,
                UserId = userId,
                
            };

            if (address.IsUserAddress)
            {
                if (addressUser == null)
                {
                    addressUser = addressinfo;
                    _dbContext.Add(addressinfo);
                }
                else
                {
                    addressUser.AddressLine1 = address.AddressLine1;
                    addressUser.AddressLine2 = address.AddressLine2;
                    addressUser.PhoneNumber = address.PhoneNumber;
                    addressUser.City = address.City;
                    addressUser.PostCode = address.PostCode;
                    addressUser.Email = address.Email;
                    
                }
                
            }
            
            _dbContext.SaveChanges();
        }

    }
}
