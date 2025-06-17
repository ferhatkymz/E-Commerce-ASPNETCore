using EF_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace EF_MVC.Services
{
    public interface ICartService
    {
        string getCustomerName();
        Card getCart(string customerName);
        public void AddToCart(int productId, int amount = 1);
        void RemoveItem(int productId, int amount);
        void TransferToCart(string userName);
    }

    public class CartService : ICartService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartService(DataContext context, IHttpContextAccessor httpContextAccessor)//inject işlemi yaptık ama bu servisten bir örnek oluşturması için program.cs bu servisi eklemek gerek
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddToCart(int productId, int amount = 1)
        {
            var card = getCart(getCustomerName());
            var product = _context.Products.Where(p => p.Id == productId).FirstOrDefault();
            if (product != null)
            {
                card.addItem(product, amount);
                _context.SaveChanges();
            }
        }

        public Card getCart(string customerName)
        {
            var card = _context.Cards.Include(c => c.CardItems).ThenInclude(c => c.product).Where(c => c.CustomerName == customerName).FirstOrDefault();
            if (card == null)
            {
                if (string.IsNullOrEmpty(customerName))
                {
                    customerName = Guid.NewGuid().ToString();//rastgele bir kullanıcı adı oluştur 766e1ae2-1305-4f35-b1b1-71aa1af02e8d.
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddMonths(1),
                        IsEssential = true

                    };
                    _httpContextAccessor.HttpContext?.Response.Cookies.Append("customerName", customerName, cookieOptions);
                }
                card = new Card { CustomerName = customerName };
                _context.Cards.Add(card);
            }
            return card;
        }

        public string getCustomerName()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            return httpContext?.User.Identity?.Name ?? httpContext?.Request.Cookies["customerName"]!;
        }

        public void RemoveItem(int productId, int amount)
        {
            var card = getCart(getCustomerName());
            var product = _context.Products.Where(p => p.Id == productId).FirstOrDefault();
            if (product != null)
            {
                card.removeItem(product, amount);
                _context.SaveChanges();
            }
        }

        public void TransferToCart(string userName)
        {
            var CookieName = _httpContextAccessor.HttpContext?.Request.Cookies["customerName"]!;
            var CookieCart = _context.Cards.Include(c => c.CardItems).ThenInclude(c => c.product).Where(c => c.CustomerName == CookieName).FirstOrDefault();
            if (CookieCart != null)
            {
                var userCart = getCart(userName);

                foreach (var item in CookieCart.CardItems)
                {
                    //kullanıcı daha önce böyle bir sipariş vermiş mi?
                    var userCartItem = userCart?.CardItems.Where(c => c.ProductId == item.ProductId).FirstOrDefault();
                    if (userCartItem != null)
                    {
                        userCartItem.Amount += 1;
                    }
                    else
                    {
                        userCart?.CardItems.Add(new CardItem { ProductId = item.ProductId, Amount = item.Amount });
                    }
                }
                _context.Cards.Remove(CookieCart);
                _context.SaveChanges();
            }
        }
    }
}