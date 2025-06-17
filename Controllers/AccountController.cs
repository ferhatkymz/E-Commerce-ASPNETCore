using System.Security.Claims;
using System.Threading.Tasks;
using EF_MVC.Models;
using EF_MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EF_MVC.Controllers
{
    public class AccountController : Controller
    {
        //incejt işlemi
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signManager;
        private readonly IEmailService _emailService;
        private readonly DataContext _context;
        private readonly ICartService _cartService;
        public AccountController(UserManager<User> userManager, SignInManager<User> signManager, IEmailService emailService, DataContext context, ICartService cartService)
        {
            _userManager = userManager;
            _signManager = signManager;
            _emailService = emailService;
            _context = context;
            _cartService = cartService;
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(AccountCreateModel model)
        {
            if (ModelState.IsValid)//önce formun doğru şekilde doldurulduğuna bak
            {
                var user = new User { FullName = model.FullName, UserName = model.Email, Email = model.Email };//bir kullanıcı oluştur.
                var result = await _userManager.CreateAsync(user, model.Password);//bunu veritabanına gönder
                if (result.Succeeded)//başarılı bir şekilde kayıt ederse 
                {
                    await _userManager.AddToRoleAsync(user, "Customer");
                    return RedirectToAction("Login", "Account");//ana sayfaya gönder
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);//başarısız olursa hataları yazdır
                }
            }
            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(AccountLoginModel model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                    if (result.Succeeded)
                    {
                        await _userManager.AccessFailedAsync(user);//giriş yaptıktan sonra erişim hakkını kaldır
                        await _userManager.SetLockoutEndDateAsync(user, null);//süreyi kaldır
                        _cartService.TransferToCart(_cartService.getCustomerName());//anonim kullanıcı işlem yaptıysa onu giriş yapan kullanıcıya ata.
                        if (!string.IsNullOrEmpty(returnUrl))
                        {

                            return RedirectToAction(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else if (result.IsLockedOut)
                    {
                        var lockedDate = await _userManager.GetLockoutEndDateAsync(user);
                        var timeLeft = lockedDate.Value - DateTime.UtcNow;
                        ModelState.AddModelError("", $"Hesabınız kitlendi. Lütfen {timeLeft.Minutes + 1} dakika sonra tekrar deneyiniz.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Şifre Hatalı");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Böyle bir E-Posta bulunamadı");
                }
            }
            return View(model);
        }


        [Authorize]
        public ActionResult Logout()
        {
            _signManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public ActionResult Settings()
        {
            return View();
        }
        [Authorize]
        public async Task<ActionResult> EditUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);
            if (user == null)
            {
                RedirectToAction("Login");
            }
            return View(new AccountEditUserModel { FullName = user!.FullName, Email = user.Email! });
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> EditUser(AccountEditUserModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId!);

                if (user != null)
                {
                    user.FullName = model.FullName;
                    user.Email = model.Email;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["message"] = "Kullanıcı Bilgileri Güncellendi";
                    }
                }

            }
            return View(model);
        }
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ChangePassword(AccountChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//tarayıcı üzerindeki giriş yapan kullanıcının bilgilerini al
                var user = await _userManager.FindByIdAsync(userId!);
                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
                    if (result.Succeeded)
                    {
                        TempData["message"] = "Parolanız Başarılı bir şekilde Değiştirildi";
                    }
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return View(model);
        }

        public ActionResult ForgotPassword()//şifremi unuttum
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(string email)//şifremi unuttum
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["message"] = "Eposta adresinizi giriniz";
                return RedirectToAction("ForgotPassword");
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["message"] = "Böyle bir Eposta kayıtlı değil";
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("ResetPassword", "Account", new { UserId = user.Id, token });//bunun çalışması için program.cs AddDefaultTokenProviders() bu providers eklemek gerekiyor.
            var link = $"<a href='http://localhost:5256{url}'>Şifreni Sıfırlamak için tıkla</a>";
            await _emailService.SendEmailAsync(user.Email!, "Şifre Sıfırlama", link);
            TempData["Message"] = "Eposta adresine gönderilen link ile şifreni sıfırlayabilirsin.";
            return RedirectToAction("Login");
        }

        public async Task<ActionResult> ResetPassword(string userId, string token)
        {
            if (userId == null || token == null)
                return RedirectToAction("Login");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return RedirectToAction("Login");

            // await _userManager.ResetPasswordAsync(user,token,)
            return View
            (
                new AccountResetPasswordModel { Email = user.Email!, token = token }
            );
        }
        [HttpPost]
        public async Task<ActionResult> ResetPassword(AccountResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    TempData["message"] = "Böyle bir kullanıcı bulunamadı";
                    return RedirectToAction("Login");
                }
                var result = await _userManager.ResetPasswordAsync(user, model.token, model.Password);
                if (result.Succeeded)
                {
                    TempData["message"] = "Şifreniz Başarılı bir şekilde değiştirildi";
                    return RedirectToAction("Login");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View(model);
        }



        public ActionResult AccessDenied()//Not-Found
        {
            return View();
        }
    }
}