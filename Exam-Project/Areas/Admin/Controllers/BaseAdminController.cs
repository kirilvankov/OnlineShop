namespace Exam_Project.Areas.Admin.Controllers
{

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area("Admin")]
    [Authorize()] // TODO: Add roleName
    public class BaseAdminController : Controller
    {
  
    }
}
