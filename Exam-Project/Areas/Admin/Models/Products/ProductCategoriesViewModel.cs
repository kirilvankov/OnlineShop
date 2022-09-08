namespace OnlineShop.Areas.Admin.Models.Products
{

    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Category;
    public class ProductCategoriesViewModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }
    }
}
