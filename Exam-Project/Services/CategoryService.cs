namespace Exam_Project.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Exam_Project.Data;
    using Exam_Project.Services.Models;

    using Microsoft.EntityFrameworkCore;

    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<CategoryDto>> GetAllCategories(CancellationToken cancellationToken)
        => await _dbContext.Categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            ParentId = c.ParentId,
            Name = c.Name
        }).ToListAsync(cancellationToken);
    }
}
