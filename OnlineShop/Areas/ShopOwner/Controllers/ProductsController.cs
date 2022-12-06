namespace OnlineShop.Areas.ShopOwner.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Threading;
    using Microsoft.AspNetCore.Mvc;
    using OnlineShop.Areas.Admin.Models.Products;

    using OnlineShop.Models.Products;

    using OnlineShop.Services.Models;

    using OnlineShop.Services;
    using System.Linq;
    using OnlineShop.Data.Infrastructure;
    using static OnlineShop.Data.DataConstants;

    public class ProductsController : BaseShopOwnerController
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IStoreService _storeService;

        public ProductsController(IProductService productService, ICategoryService categoryService, IStoreService storeService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _storeService = storeService;
        }

        public async Task<IActionResult> All([FromQuery] LoadProductsRequest request, CancellationToken cancellationToken)
        {
            var query = new AllProductsDto
            {
                SearchTerm = request.SearchTerm,
                Sorting = request.Sorting,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                CategoryId = request.CategoryId,
                TotalItems = request.TotalItems,
            };
            var userId = User.GetId();
            var storeId = await _storeService.GetStoreId(userId, cancellationToken);

            if (storeId == null)
            {
                return NotFound();
            }
            var result = await _productService.GetAllPerStoreAsync(query, storeId, cancellationToken);

            var model = new PagedResult<ProductViewModel>
            {
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                SearchTerm = result.SearchTerm,
                TotalCount = result.TotalItems,
                Sorting = result.Sorting,
                CategoryId = result.CategoryId,
                Products = result.Products.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    OrderingNumber = p.OrderingNumber,
                }).ToList(),
                Categories = result.Categories.Select(c => new ProductCategoriesViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ParentId = c.ParentId
                }).ToList()

            };

            return View(model);
        }
        public async Task<IActionResult> Add(CancellationToken cancellationToken)
        {

            return View(new ProductFormModel
            {
                Categories = await GetCategories(cancellationToken)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductFormModel product, CancellationToken cancellationToken)
        {
            if (product.Image == null && string.IsNullOrEmpty(product.ImageUrl))
            {
                ModelState.AddModelError("Image", "Field image is required when imageUrl is empty!");
            }

            if (product.Image != null && product.Image.Length > 10 * 1024 * 1024)
            {
                ModelState.AddModelError("file", "File is too large!");
            }

            if (!_categoryService.CategoryExist(product.CategoryId))
            {
                ModelState.AddModelError("Category", "Invalid Category");
            }

            if (!ModelState.IsValid)
            {

                product.Categories = await GetCategories(cancellationToken);
                return View(product);
            }

            var userId = User.GetId();
            var storeId = await _storeService.GetStoreId(userId, cancellationToken);

            await _productService.AddAsync(product, storeId, cancellationToken);

            return RedirectToAction(nameof(All));
        }
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdAsync(id, cancellationToken);

            if (product == null)
            {
                return NotFound();
            }

            return View(new ProductFormModel
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                Categories = await GetCategories(cancellationToken)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductFormModel product, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(new ProductFormModel
                {
                    Categories = await GetCategories(cancellationToken)
                });
            }

            var result = await _productService.EditAsync(id, product, cancellationToken);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)// todo: create view
        {
            var result = await _productService.GetByIdAsync(id, cancellationToken);
            var viewRes = new DetailsProductViewModel()
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                Price = result.Price,
                ImageUrl = result.ImageUrl,
                Category = result.Category,
            };
            return View(viewRes);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _productService.DeleteAsync(id, cancellationToken);
            return RedirectToAction(nameof(All));
        }

        private async Task<IEnumerable<ProductCategoriesViewModel>> GetCategories(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllCategories(cancellationToken);
            return categories.Select(c => new ProductCategoriesViewModel
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();
        }
    }
}
