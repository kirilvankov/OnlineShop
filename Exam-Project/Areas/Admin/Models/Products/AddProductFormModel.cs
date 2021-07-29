namespace Exam_Project.Areas.Admin.Models.Products
{

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Product;

    public class AddProductFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [Url]
        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        [Display(Name="Category")]
        public int CategoryId { get; set; }

        public IEnumerable<ProductCategoriesViewModel> Categories { get; set; }
    }
}
