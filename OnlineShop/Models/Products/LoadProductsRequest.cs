namespace OnlineShop.Models.Products
{
    public class LoadProductsRequest
    {
        public string SearchTerm { get; set; }

        public Sorting Sorting { get; set; }

        public int? CategoryId { get; set; }

        public int TotalItems { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 3;
    }
}
