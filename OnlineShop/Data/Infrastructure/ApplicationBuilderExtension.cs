namespace OnlineShop.Data.Infrastructure
{
    using System;
    using System.Threading.Tasks;

    using OnlineShop.Areas.Admin;
    using OnlineShop.Data.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;



    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder PrepareAdmin(this IApplicationBuilder app)
        {
            using var scopedService = app.ApplicationServices.CreateScope();

            var service = scopedService.ServiceProvider;

            SeedAdministrator(service);

            return app;
        }
        private static void SeedAdministrator(IServiceProvider service)
        {
            var userManager = service.GetRequiredService<UserManager<User>>();
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync(AdminConstants.AdministratorRoleName))
                {
                    return;
                }
                var role = new IdentityRole { Name = AdminConstants.AdministratorRoleName };

                await roleManager.CreateAsync(role);

                var user = new User
                {
                    UserName = AdminConstants.AdministratorEmail,
                    Email = AdminConstants.AdministratorEmail,
                    FirstName = AdminConstants.AdministratorEmail,
                    LastName = AdminConstants.AdministratorEmail,
                    Address = AdminConstants.AdministratorEmail
                };

                await userManager.CreateAsync(user, AdminConstants.AdministratorPassword);

                await userManager.AddToRoleAsync(user, role.Name);


            }).GetAwaiter()
              .GetResult();


        }
    }
}
