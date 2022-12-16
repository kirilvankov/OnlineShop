namespace OnlineShop.Models.Products
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using OnlineShop.Areas.Admin.Models.Products;

    public class PagedResult<T> where T : class
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PageCount => (int)Math.Ceiling((double)TotalCount / PageSize);

        [Display(Name = "Search")]
        public string SearchTerm { get; set; }

        public Sorting Sorting { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        public List<T> Products { get; set; }
        public List<ProductCategoriesViewModel> Categories { get; set; }

        public PagedResult()
        {
            Products = new List<T>();
            Categories = new List<ProductCategoriesViewModel>();
        }
    }
}
