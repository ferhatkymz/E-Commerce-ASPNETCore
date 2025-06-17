using EF_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EF_MVC.Controllers
{
    public class ComplaintController : Controller
    {
        private readonly DataContext _context;
        public ComplaintController(DataContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpPost]
        public ActionResult Submit(string description)
        {
            if (!string.IsNullOrEmpty(description))
            {
                var userName = User.Identity!.Name;
                var user = _context.Users.Select(u => new { u.Id, u.UserName }).FirstOrDefault(u => u.UserName == userName);
                _context.Complaints.Add(new Complaint { UserId = user!.Id, Description = description });
                _context.SaveChanges();
                TempData["Message"] = "Şikayetinizi Değerlendirmek Üzere Aldık ";
            }
            return RedirectToAction("Index", "Home");
        }
    }
}