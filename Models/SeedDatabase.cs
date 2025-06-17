using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EF_MVC.Models
{
    public static class SeedDatabase
    {
        public static async Task Initialize(IApplicationBuilder app)
        {
            var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<Role>>();

            if (!roleManager.Roles.Any())//eğer sistemde hiç role yoksa başlangıç olarak sen role ekle test amaçlı
            {
                var role = new Role { Name = "Admin" };
                await roleManager.CreateAsync(role);
                var role2 = new Role { Name = "Customer" };
                await roleManager.CreateAsync(role2);
            }
            if (!userManager.Users.Any())//eğer sistemde hiç kullanıcı yoksa başlangıç olarak sen kullanıcılar ekle
            {
                var user1 = new User { FullName = "Ferhat Kaymaz", Email = "ferhat@gmail.com", UserName = "FerhatKaymaz" };
                await userManager.CreateAsync(user1, "Fk?123456");
                await userManager.AddToRoleAsync(user1, "Admin");

                var user2 = new User { FullName = "Eylül Kaymaz", Email = "eylül@gmail.com", UserName = "EylülKaymaz" };
                await userManager.CreateAsync(user2, "Ek?123456");
                await userManager.AddToRoleAsync(user2, "Customer");
            }
        }

    }
}