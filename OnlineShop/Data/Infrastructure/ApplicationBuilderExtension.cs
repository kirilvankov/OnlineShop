namespace OnlineShop.Data.Infrastructure
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using OnlineShop.Areas.Admin;
    using OnlineShop.Areas.ShopOwner;
    using OnlineShop.Data.Models;

    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedService = app.ApplicationServices.CreateScope();

            var services = scopedService.ServiceProvider;

            MigrateDatabase(services);
            SeedAdministrator(services);
            SeedShopOwnerRole(services);
            SeedProducts(services);

            return app;
        }
        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync(AdminConstants.AdministratorRoleName))
                {
                    return;
                }
                var role = new IdentityRole { Name = AdminConstants.AdministratorRoleName };

                await roleManager.CreateAsync(role);

                var user = new ApplicationUser
                {
                    UserName = AdminConstants.AdministratorEmail,
                    Email = AdminConstants.AdministratorEmail,
                    FirstName = AdminConstants.AdministratorEmail,
                    LastName = AdminConstants.AdministratorEmail,
                };

                await userManager.CreateAsync(user, AdminConstants.AdministratorPassword);

                await userManager.AddToRoleAsync(user, role.Name);


            }).GetAwaiter()
              .GetResult();
        }
        //public static IApplicationBuilder CreateShopOwner(this IApplicationBuilder app)
        //{
        //    using var scopedService = app.ApplicationServices.CreateScope();

        //    var service = scopedService.ServiceProvider;

        //    SeedShopOwnerRole(service);

        //    return app;
        //}

        private static void SeedShopOwnerRole(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync(ShopOwnerConstants.RoleName))
                {
                    return;
                }
                var role = new IdentityRole { Name = ShopOwnerConstants.RoleName };

                await roleManager.CreateAsync(role);

            }).GetAwaiter()
              .GetResult();
        }
        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<ApplicationDbContext>();

            data.Database.Migrate();
        }

        private static void SeedProducts(IServiceProvider services)
        {
            var data = services.GetRequiredService<ApplicationDbContext>();
            if (data.Products.Any())
            {
                return;
            }
            data.Products.AddRange(new[]
            {
                new ProductEntity()
                { 
                    Name = "Shampoo",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas.\r\n              Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut.\r\n              Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 7.00m,
                    ImageUrl = "/media/ab0fb722-a72f-4bbf-a816-2a540c5de096_shampoo.jpg",
                    CategoryId = 6,
                },
                new ProductEntity()
                {
                    Name = "Car",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas.\r\n              Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut.\r\n              Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 1000m,
                    ImageUrl = "/media/c77a70d6-b2c8-42ba-aa24-45d60f99c6e0_usa-car-import-home-900x500_2.png",
                    CategoryId = 2,
                },
                new ProductEntity()
                {
                    Name = "Table",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas.\r\n              Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut.\r\n              Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 50m,
                    ImageUrl = "/media/9f49d103-19ab-4efd-a92b-01a4ed81ddb0_table.jpg",
                    CategoryId = 1,
                },
                new ProductEntity()
                {
                    Name = "T-shirt",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas.\r\n              Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut.\r\n              Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 10m,
                    ImageUrl = "/media/954c8a9b-9745-4866-982f-59d155b60a4c_tshirt_catalog_detail_image.jpg",
                    CategoryId = 3,
                },
                new ProductEntity()
                {
                    Name = "Shoes",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas.\r\n              Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut.\r\n              Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 30.00m,
                    ImageUrl = "/media/f7578757-1c5a-45de-b079-9c67fb9b48ea_5-junior-11cwhtnbl-fightercdgpg-hunter-31cblue-asian-original-imaewnengdkk7hea.jpeg",
                    CategoryId = 4,
                },
                new ProductEntity()
                {
                    Name = "Green T-shirt",
                    Description = "best green T-shirt",
                    Price = 15.50m,
                    ImageUrl = "/media/a60c716c-df05-4654-8334-3646a9adf092_greenT-shirt.jpg",
                    CategoryId = 3,
                },
                new ProductEntity()
                {
                    Name = "White T-shirt",
                    Description = "best white T-shirt",
                    Price = 17.99m,
                    ImageUrl = "/media/39a95a5e-031d-4fc2-ae53-70fb1e9f60f6_white-t-shirt-with-silkscreen-mockup.jpg",
                    CategoryId = 3,
                },
                new ProductEntity()
                {
                    Name = "Sport-Shirt",
                    Description = "Only for football fans",
                    Price = 25.60m,
                    ImageUrl = "/media/0ddae420-1c74-49f6-8b29-8e53a3e586e6_unique-design-t-shirt-500x500.jpg",
                    CategoryId = 3,
                },
                new ProductEntity()
                {
                    Name = "Sport-Shirt",
                    Description = "For sport fans",
                    Price = 18.99m,
                    ImageUrl = "/media/0a175e09-62e5-4671-ae9e-8a303875b1f2_Adidas-T-shirt.jpg",
                    CategoryId = 3,
                },
                new ProductEntity()
                {
                    Name = "Gift bags",
                    Description = "Suitable for all occasions",
                    Price = 5.88m,
                    ImageUrl = "/media/54c74ba7-f2d7-4e4a-a2e1-bb1ecf22989a_Giftbags.jpg",
                    CategoryId = 1,
                },
                new ProductEntity()
                {
                    Name = "Wardrobe",
                    Description = "with glass doors",
                    Price = 680.00m,
                    ImageUrl = "/media/c14b8700-d2ad-4d20-9b53-1992911ed25d_hinged-glass-door-1-750x750_0.jpg",
                    CategoryId = 1,
                },
                new ProductEntity()
                {
                    Name = "Lotion",
                    Description = "A weekly, clinically tested mask that strengthens damaged hair and helps prevent future damage.",
                    Price = 18.25m,
                    ImageUrl = "/media/8398087c-c946-448c-855d-9515d148a381_makasa.jpg",
                    CategoryId = 6,
                },
                new ProductEntity()
                {
                    Name = "Make up",
                    Description = "A weekly, clinically tested mask that strengthens damaged hair and helps prevent future damage!",
                    Price = 25.10m,
                    ImageUrl = "/media/dbd3ecf6-0ff6-42b1-aff5-42ba351205f4_makeUp1.jpg",
                    CategoryId = 6,
                },
                new ProductEntity()
                {
                    Name = "Perfume",
                    Description = "Obsession by Calvin Klein Cologne. Make a stylish statement by using one of the most recognizable scents in the world. Created by Bob Slattery and launched by Calvin Klein in 1986, Obsession for men is a ground-breaking fragrance that stands tall over all others. The timeless best-selling scent features a blend of sweet amber alongside traditionally masculine notes of lavender, spice and wood.",
                    Price = 25.10m,
                    ImageUrl = "/media/3c9cc6e7-4248-4089-909f-1dc8f18eaed4_perfume.jpg",
                    CategoryId = 6,
                },
                new ProductEntity()
                {
                    Name = "Ford",
                    Description = "City car with low consumption. Suitable for center area",
                    Price = 3800.00m,
                    ImageUrl = "/media/d60d362c-9b02-4bc0-b86d-6e4f8a3b79de_ford_fiesta.png",
                    CategoryId = 2,
                },
                new ProductEntity()
                {
                    Name = "Skoda",
                    Description = "City car with low consumption. Suitable for center area",
                    Price = 2500.00m,
                    ImageUrl = "/media/691c79cd-0e1f-4030-9a3c-a5194ed3f96f_skoda-fabia-india-photo.jpg",
                    CategoryId = 2,
                },
                new ProductEntity()
                {
                    Name = "Skoda",
                    Description = "Familycar with high level equipment. Suitable long travel",
                    Price = 7000.00m,
                    ImageUrl = "/media/2b554aca-2716-4612-b2de-b5f97d725b80_Skoda_Superb.jpg",
                    CategoryId = 2,
                },
                new ProductEntity()
                {
                    Name = "Honda",
                    Description = "City car with low consumption. Suitable for center area",
                    Price = 4300.00m,
                    ImageUrl = "/media/a826d9e8-6f91-47a7-ac38-43fcd61d36a3_honda_jazz.jpg",
                    CategoryId = 2,
                },
                new ProductEntity()
                {
                    Name = "Honda",
                    Description = "Sports car. Suitable for driftiing and competition",
                    Price = 6000.00m,
                    ImageUrl = "/media/41cb0d3b-4357-45c0-a983-6875b483641e_Honda-Civic-Type-R.jpg",
                    CategoryId = 2,
                },
                new ProductEntity()
                {
                    Name = "Mercedes",
                    Description = "Luxury car. Only for appriciators",
                    Price = 10000.00m,
                    ImageUrl = "/media/13719c38-554c-49ad-a33f-57d00aa81896_merc.jpg",
                    CategoryId = 2,
                },
                new ProductEntity()
                {
                    Name = "Pants",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 30.00m,
                    ImageUrl = "/media/f09eb308-b12a-4f0e-aef0-cd619d010027_pants.jpg",
                    CategoryId = 3,
                },
                new ProductEntity()
                {
                    Name = "Pants",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 42.00m,
                    ImageUrl = "/media/47eac278-d571-4799-b652-948e0f347939_newPants.jpg",
                    CategoryId = 3,
                },
                new ProductEntity()
                {
                    Name = "Laika",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 12.00m,
                    ImageUrl = "/media/a52c7f4a-efc0-45a6-9cb7-ac44a22767a0_laika.jpg",
                    CategoryId = 6,
                },
                new ProductEntity()
                {
                    Name = "Rose Water",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 11.98m,
                    ImageUrl = "/media/65447703-387e-4d06-9566-8858118e5b66_roseWater.jpg",
                    CategoryId = 6,
                },
                new ProductEntity()
                {
                    Name = "Lotion",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 17.00m,
                    ImageUrl = "/media/ac99394b-31ff-42a9-b5ab-4eccb545ece5_faceCare.jpg",
                    CategoryId = 6,
                },
                new ProductEntity()
                {
                    Name = "Shoes",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 41.00m,
                    ImageUrl = "/media/42e72558-6c00-4a6a-8d52-19100955d1ea_walkingshoes.jpg",
                    CategoryId = 4,
                },
                new ProductEntity()
                {
                    Name = "Sneakers",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 39.00m,
                    ImageUrl = "/media/eff6a8cc-12fe-41f4-98cb-34fe6eeaf7bf_sneakers.jpg",
                    CategoryId = 4,
                },
                new ProductEntity()
                {
                    Name = "Shoes",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 19.99m,
                    ImageUrl = "/media/91f4df9b-1059-4983-9e70-37fa9db3d412_pexels-web-donut.jpg",
                    CategoryId = 4,
                },
                new ProductEntity()
                {
                    Name = "Sneakers",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 45.00m,
                    ImageUrl = "/media/8bd3e8eb-f670-4c6a-aa73-8e6388994c2c_dark-blue-sport-shoes.jpg",
                    CategoryId = 4,
                },
                new ProductEntity()
                {
                    Name = "Heels",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 55.00m,
                    ImageUrl = "/media/96e74374-7b93-483e-816e-e978c1351def_pink_heels.jpg",
                    CategoryId = 4,
                },
                new ProductEntity()
                {
                    Name = "Heels",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 49.00m,
                    ImageUrl = "/media/21093dd3-1da0-41da-9ca9-e8088e860a6f_goodinfonet_high_heels.jpg",
                    CategoryId = 4,
                },
                new ProductEntity()
                {
                    Name = "Plush toy",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 15.00m,
                    ImageUrl = "/media/ebca430d-caa3-4094-8266-2f333f00e64f_sheep.jpg",
                    CategoryId = 5,
                },
                new ProductEntity()
                {
                    Name = "Doll",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 19.00m,
                    ImageUrl = "/media/19ff9bcf-56cd-4e39-bbc6-a931cc3f9e1a_doll toy.jpg",
                    CategoryId = 5,
                },
                new ProductEntity()
                {
                    Name = "Barbie",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 29.00m,
                    ImageUrl = "/media/c43dc15a-d61d-4b6f-a1df-64314f71bb52_Barbie-Gold-Medal-Barbie-Doll_GPC77_04.jpg",
                    CategoryId = 5,
                },
                new ProductEntity()
                {
                    Name = "Barbie",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 32.00m,
                    ImageUrl = "/media/cd763e6a-16dd-4a3c-83c8-081de5dd0743_beauty-1260975__340.jpg",
                    CategoryId = 5,
                },
                new ProductEntity()
                {
                    Name = "Doll",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 16.98m,
                    ImageUrl = "/media/24eec198-fd24-4b01-8212-8e277da1e51a_Kewpie_doll.jpg",
                    CategoryId = 5,
                },
                new ProductEntity()
                {
                    Name = "Train",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 25.00m,
                    ImageUrl = "/media/e3ce2f40-857d-4389-abdb-c8a802e507b3_train_toy.jpg",
                    CategoryId = 5,
                },
                new ProductEntity()
                {
                    Name = "Train",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 35.00m,
                    ImageUrl = "/media/7792bd34-6273-49f6-b998-4d0b26d3de59_Distler-Zug.JPG",
                    CategoryId = 5,
                },
                new ProductEntity()
                {
                    Name = "Tv",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 120.00m,
                    ImageUrl = "/media/6a6b1300-fb69-459f-ae86-46a97dde1620_old_tv.jpg",
                    CategoryId = 1,
                },
                new ProductEntity()
                {
                    Name = "Tv",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 230.00m,
                    ImageUrl = "/media/312c6150-c16d-4463-8d76-d219893a9fcd_a8381d8dc836b6377392d0c44fef9aa-400x400.jpg",
                    CategoryId = 1,
                },
                new ProductEntity()
                {
                    Name = "Wardrobe",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 480m,
                    ImageUrl = "/media/156a1183-22b5-4f56-8298-3c72ca1cd8af_wardrobe_new.jpg",
                    CategoryId = 1,
                },
                new ProductEntity()
                {
                    Name = "Table",
                    Description = "Ut in ea error laudantium quas omnis officia. Sit sed praesentium voluptas. Corrupti inventore consequatur nisi necessitatibus modi consequuntur soluta id. Enim autem est esse natus assumenda. Non sunt dignissimos officiis expedita. Consequatur sint repellendus voluptas. Quidem sit est nulla ullam. Suscipit debitis ullam iusto dolorem animi dolorem numquam. Enim fuga ipsum dolor nulla quia ut. Rerum dolor voluptatem et deleniti libero totam numquam nobis distinctio. Sit sint aut. Consequatur rerum in.",
                    Price = 230.00m,
                    ImageUrl = "/media/9a0b0999-037f-45a0-867a-221822ddd5ff_coffee-side-tables_10705.jpg",
                    CategoryId = 1,
                },
            });

            data.SaveChanges();
        }
    }
}
