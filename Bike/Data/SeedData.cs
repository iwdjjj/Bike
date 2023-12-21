using Bike.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Bike.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            if (context.Routes.Any())
            {
                return;
            }

            Height height = new Height
            {
                Terrain_height = 1,
                Complexity = 1,
                Time = 5
            };
            context.Height.Add(height);
            context.SaveChanges();

            MainAddress mainaddress = new MainAddress
            {
                Country = "Россия",
                State = "Чувашия",
                City = "Чебоксары",
                Street = "пр-т. Мира",
                House = "1, корп. 2",
                HeightId = height.HeightId
            };
            context.MainAddress.Add(mainaddress);
            context.SaveChanges();

            MainAddress mainaddress1 = new MainAddress
            {
                Country = "Россия",
                State = "Чувашия",
                City = "Чебоксары",
                Street = "пр-т. Московский",
                House = "1, корп. 2",
                HeightId = height.HeightId
            };
            context.MainAddress.Add(mainaddress1);
            context.SaveChanges();

            Address address = new Address
            {
                MainAddressId = mainaddress.MainAddressId,
                Street = "пр-т. Мира",
                House = "1, корп. 2",
            };
            context.Address.Add(address);
            context.SaveChanges();

            Address address1 = new Address
            {
                MainAddressId = mainaddress1.MainAddressId,
                Street = "пр-т. Московский",
                House = "1, корп. 2",
            };
            context.Address.Add(address1);
            context.SaveChanges();

            BikeType bikeType = new BikeType
            {
                Name = "Гибридный",
                Complexity = 1,
                Speed = 25,
                Time = 5
            };
            context.BikeType.Add(bikeType);
            context.SaveChanges();

            BikeType bikeType1 = new BikeType
            {
                Name = "Шоссейный",
                Complexity = 1,
                Speed = 35,
                Time = 5
            };
            context.BikeType.Add(bikeType1);
            context.SaveChanges();

            Models.Route visit = new Models.Route
            {
                AddressId1 = address.AddressId,
                AddressId2 = address1.AddressId,
                BikeTypeId = bikeType.BikeTypeId,
                Time = 20
            };
            context.Routes.Add(visit);
            context.SaveChanges();

            Doljnost doljnost1 = new Doljnost
            {
                DoljnostName = "Администратор"
            };
            context.Doljnosts.Add(doljnost1);

            Doljnost doljnost2 = new Doljnost
            {
                DoljnostName = "Пользователь"
            };
            context.Doljnosts.Add(doljnost2);

            context.SaveChanges();

            string[] roles = new string[] { "Administrator", "Guest" };
            foreach (string role in roles)
            {
                CreateRole(serviceProvider, role);
            }

            CustomUser customUser1 = new() { Surname = "Alekseev", Name = "Aleksei", Midname = "Alekseevich", UserName = "alekseev@mail.ru", Email = "alekseev@mail.ru", DoljnostId = doljnost1.DoljnostId };

            AddUserToRole(serviceProvider, "Password123!", "Administrator", customUser1);

            CustomUser customUser2 = new() { Surname = "Ivanov", Name = "Ivan", Midname = "Ivanovich", UserName = "ivanov@mail.ru", Email = "ivanov@mail.ru", DoljnostId = doljnost2.DoljnostId };

            AddUserToRole(serviceProvider, "Password123!", "Guest", customUser2);
        }

        private static void CreateRole(IServiceProvider serviceProvider, string roleName)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Task<bool> roleExists = roleManager.RoleExistsAsync(roleName);
            roleExists.Wait();

            if (!roleExists.Result)
            {
                Task<IdentityResult> roleResult = roleManager.CreateAsync(new IdentityRole(roleName));
                roleResult.Wait();
            }

        }

        private static void AddUserToRole(IServiceProvider serviceProvider, string userPwd, string roleName, CustomUser customUser)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<CustomUser>>();

            Task<CustomUser> checkAppUser = userManager.FindByEmailAsync(customUser.Email); ;

            checkAppUser.Wait();

            if (checkAppUser.Result == null)
            {
                Task<IdentityResult> taskCreateAppUser = userManager.CreateAsync(customUser, userPwd);

                taskCreateAppUser.Wait();

                if (taskCreateAppUser.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(customUser, roleName);
                    newUserRole.Wait();
                }
            }
        }
    }
}
