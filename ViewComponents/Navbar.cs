using EF_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace EF_MVC.ViewComponents
{
    public class Navbar : ViewComponent
    {
        private readonly DataContext _context;
        public Navbar(DataContext context)
        {
            this._context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(_context.Categories.ToList());
        }
    }
}