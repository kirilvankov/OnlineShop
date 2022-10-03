namespace OnlineShop.ViewComponents
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using OnlineShop.Models.Products;
    using OnlineShop.Services;

    public class PagerViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public PagerViewComponent(IProductService productService)
        {
            _productService = productService;
        }
        public Task<IViewComponentResult> InvokeAsync(PagedResult<ProductViewModel> result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
