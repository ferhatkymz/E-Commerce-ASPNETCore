using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EF_MVC.Models;

namespace EF_MVC.Controllers;

public class HomeController : Controller
{
    private readonly DataContext _context;
    public HomeController(DataContext context)
    {
        this._context = context;
    }
    public ActionResult Index()
    {
        var products = _context.Products.Where(p => p.isActive && p.isHome).ToList();
        ViewData["Categories"] = _context.Categories.ToList();
        return View(products);
    }
}
