using EF_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace EF_MVC.ViewComponents
{
    public class Slider : ViewComponent
    {
        private readonly DataContext _context;
        public Slider(DataContext context)
        {
            this._context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(_context.Sliders.Where(s => s.isActive == true).ToList());
        }
    }
}