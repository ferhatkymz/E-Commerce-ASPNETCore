using Microsoft.EntityFrameworkCore;
using EF_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using EF_MVC.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IEmailService, SmtpEmailService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddControllersWithViews();
//buraya veritabanı bağlantımı bıraktım ister development modunda isterse yayın aşamasında hangi bağlantı adresini kullanmak istersem onu kullanırım
builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});
builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();//gittim identity kütüphanesini servisime dahil ettim ve DataContexti vermemin sebebi gitsin dataContext içerisindeki connectionda bulunan veritabanına bu tabloları eklesin.
builder.Services.Configure<IdentityOptions>(Options =>
{//link=>https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-9.0
    Options.Password.RequireDigit = true;//Mutlaka 1 rakam içermeli
    Options.Password.RequiredLength = 7;//Mutlaka şifre en az 7 karakter uzunluğunda olmalıdır.
    Options.Password.RequireLowercase = true;//mutlaka küçük harf bulundur.
    Options.Password.RequireNonAlphanumeric = true;//Mutlaka bir sembol bulundur.
    Options.Password.RequireUppercase = true;//Mutlaka bir büyük harf içer

    Options.User.RequireUniqueEmail = true;//girilen email bilgileri benzersiz olmalı
    Options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzüöçşğıÜÖÇŞĞİABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

    Options.Lockout.MaxFailedAccessAttempts = 5;//giriş yaparken 5 defa deneme yap sonrasında kullanıcı giriş yapamazsa hesap kendini kitlesin.
    Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);//kaç dakika veya gün kitlesin
    Options.Lockout.AllowedForNewUsers = true;
}
);

builder.Services.ConfigureApplicationCookie(option =>
{
    option.ExpireTimeSpan = TimeSpan.FromDays(30);
    option.SlidingExpiration = true;
});
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();//tam şuraya eklemen gerekiyor
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute
(
    name: "ProductByCategoryId",
    pattern: "products/{url?}",
    defaults: new { Controller = "Product", Action = "List" }
)
.WithStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

//test verilerinin eklenmesi
await SeedDatabase.Initialize(app);
app.Run();
