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
    public class CardController : Controller
    {
        //incejt işlemi
        private readonly ICartService _CartService;

        public CardController(ICartService CartService)
        {
            _CartService = CartService;
        }

        public ActionResult Index()
        {

            var cart = _CartService.getCart(_CartService.getCustomerName());
            return View(cart);
        }
        [HttpPost]
        public ActionResult AddToCard(int productId, int amount = 1)//sepete ürün ekle
        {
            _CartService.AddToCart(productId, amount);
            var cart = _CartService.getCart(_CartService.getCustomerName());
            TempData["productCount"] = cart.CardItems.Count();
            return RedirectToAction("Details", "Product", new { id = productId });
        }

        [HttpPost]
        public ActionResult AddProduct(int productId, int amount = 1)//ürün arttır
        {
            _CartService.AddToCart(productId, amount);
            var cart = _CartService.getCart(_CartService.getCustomerName());
            TempData["productCount"] = cart.CardItems.Count();
            return RedirectToAction("Index", "Card");
        }


        [HttpPost]
        public ActionResult removeItem(int productId, int amount)//ürünü silme
        {
            _CartService.RemoveItem(productId, amount);
            var cart = _CartService.getCart(_CartService.getCustomerName());
            TempData["productCount"] = cart.CardItems.Count();

            return RedirectToAction("Index", "Card");

        }
    }
}
