using Microsoft.AspNetCore.Identity;
using MotoShop.Constants;
using MotoShop.Data.Migrations;

namespace MotoShop.Data.Seed
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            const string email = "admin@motoshop.com";
            const string password = "Admin@123";

            var admin = await userManager.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = "MotoShop Administrator",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Roles.Admin);
                }
            }
        }
    }
}
