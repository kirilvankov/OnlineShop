namespace OnlineShop.ViewComponents
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using OnlineShop.Models.Products;

    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagedResult<ProductViewModel> result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
