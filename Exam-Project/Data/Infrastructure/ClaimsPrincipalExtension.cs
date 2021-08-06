namespace Exam_Project.Data.Infrastructure
{

    using System.Security.Claims;

    public static class ClaimsPrincipalExtension
    {
        public static string GetId(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
