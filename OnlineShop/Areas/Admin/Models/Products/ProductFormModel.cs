namespace OnlineShop.Areas.Admin.Models.Products
{

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using static Data.DataConstants.Product;

    public class ProductFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }


        [Display(Name = "Image")]
        public IFormFile Image { get; set; }

        public string ImageName { get; set; }

        [Display(Name="Category")]
        [Range(1, int.MaxValue, ErrorMessage = "You should select valid category from list.")]
        public int CategoryId { get; set; }

        public IEnumerable<ProductCategoriesViewModel> Categories { get; set; }
    }
}
