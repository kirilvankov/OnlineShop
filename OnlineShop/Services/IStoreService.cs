namespace OnlineShop.Services
{
    using System.Threading.Tasks;

    using OnlineShop.Services.Models;

    public interface IStoreService
    {
        Task<string> Apply(RegisterStoreDto model);
    }
}
