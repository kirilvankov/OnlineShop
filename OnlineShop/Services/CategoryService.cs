﻿namespace OnlineShop.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using OnlineShop.Data;
    using OnlineShop.Services.Models;

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