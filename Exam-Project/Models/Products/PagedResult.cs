namespace Exam_Project.Models.Products
{
    using System;
    using System.Collections.Generic;

    public class PagedResult<T> where T : class
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PageCount => (int)Math.Ceiling((double)TotalCount / PageSize);
        public List<T> Items { get; set; }

        public PagedResult()
        {
            Items = new List<T>();
        }
    }
}
