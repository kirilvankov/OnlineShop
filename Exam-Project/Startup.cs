namespace Exam_Project
{
    using Exam_Project.Data;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    using Microsoft.AspNetCore.Identity;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Exam_Project.Data.Infrastructure;
    using Exam_Project.Data.Models;
    using System;
    using Exam_Project.Services;
    using Microsoft.AspNetCore.Authentication.Certificate;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddDistributedMemoryCache();

            //services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate(options =>
            //{
            //    options.Events = new CertificateAuthenticationEvents
            //    {
            //        OnCertificateValidated = context =>
            //        {
            //            var claims = new[]
            //            {
            //        new Claim(
            //            ClaimTypes.NameIdentifier,
            //            context.ClientCertificate.Subject,
            //            ClaimValueTypes.String, context.Options.ClaimsIssuer),
            //        new Claim(
            //            ClaimTypes.Name,
            //            context.ClientCertificate.Subject,
            //            ClaimValueTypes.String, context.Options.ClaimsIssuer)
            //    };

            //            context.Principal = new ClaimsPrincipal(
            //                new ClaimsIdentity(claims, context.Scheme.Name));
            //            context.Success();

            //            return Task.CompletedTask;
            //        }
            //    };
            //});

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddHttpContextAccessor();
            services.AddTransient<IShoppingCartStorage, ShoppingCartStorage>();
            services.AddTransient<IShopppingCartService, ShoppingCartService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddControllersWithViews();


        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareAdmin();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
