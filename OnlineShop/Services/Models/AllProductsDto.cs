namespace OnlineShop.Services.Models
{
    using System.Collections.Generic;

    using OnlineShop.Models.Products;

    public class AllProductsDto
    {
        public string SearchTerm { get; set; }

        public Sorting Sorting { get; set; }

        public int? CategoryId { get; set; }

        public int TotalItems { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public List<ProductDto> Products { get; set; }
        public List<CategoryDto> Categories { get; set; }
    }
}
