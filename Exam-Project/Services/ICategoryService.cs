namespace Exam_Project.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Exam_Project.Services.Models;

    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategories(CancellationToken cancellationToken);
    }
}
