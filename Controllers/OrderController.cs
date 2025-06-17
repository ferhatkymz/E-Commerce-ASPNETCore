using System.Threading.Tasks;
using AspNetCoreGeneratedDocument;
using EF_MVC.Models;
using EF_MVC.Services;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace EF_MVC.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        // inject
        private readonly ICartService _cartService;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        public OrderController(ICartService cartService, DataContext dataContext, IConfiguration configuration)
        {
            _cartService = cartService;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<Order> orders = _dataContext.Orders.ToList();
            return View(orders);
        }
        public ActionResult Checkout()
        {
            ViewBag.cart = _cartService.getCart(_cartService.getCustomerName());
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Checkout(OrderCreateModel model)
        {
            var cart = _cartService.getCart(_cartService.getCustomerName());

            var user = _dataContext.Users.Select(u => new { u.Id, u.UserName }).Where(u => u.UserName == _cartService.getCustomerName()).FirstOrDefault();
            var id = user!.Id;
            if (ModelState.IsValid)
            {

                var order = new Order()
                {
                    UserId = id,
                    FullName = model.FullName,
                    Phone = model.Phone,
                    PostalCode = model.PostalCode,
                    City = model.City,
                    Adress = model.Adress,
                    TotalPrice = (decimal)cart.Total(),
                    OrderNotes = model.OrderNotes,
                    OrderDetails = cart.CardItems.Select(c => new OrderDetail
                    {
                        ProductId = c.ProductId,
                        Price = c.product.Price,
                        Amount = c.Amount
                    }).ToList()
                };

                var result = await ProcessPayment(model, cart);
                if (result.Status == "success")
                {

                    _dataContext.Orders.Add(order);
                    _dataContext.Cards.Remove(cart);
                    _dataContext.SaveChanges();
                    return RedirectToAction("Completed", "Order", new { orderId = order.Id });
                }
                else//failed
                {
                    ModelState.AddModelError("", result.ErrorMessage);
                }
            }
            ViewBag.cart = cart;
            return View(model);
        }

        public ActionResult Completed(string orderId)
        {
            return View("Completed", orderId);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            var order = _dataContext.Orders.Include(od => od.OrderDetails).ThenInclude(od => od.Product).FirstOrDefault(od => od.Id == id);
            return View(order);
        }


        public ActionResult OrderList()
        {
            var user = _dataContext.Users.Select(u => new { u.Id, u.UserName }).FirstOrDefault(u => u.UserName == _cartService.getCustomerName());
            var orders = _dataContext.Orders.Include(o => o.OrderDetails).ThenInclude(o => o.Product).Where(o => o.UserId == user!.Id).ToList();
            return View(orders);
        }

        private async Task<Payment> ProcessPayment(OrderCreateModel model, Models.Card cart)
        {
            Options options = new Options();
            options.ApiKey = _configuration["Payment:ApiKey"];
            options.SecretKey = _configuration["Payment:SecretKey"];
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = Guid.NewGuid().ToString();
            request.Price = cart.subTotal().ToString();
            request.PaidPrice = cart.subTotal().ToString();
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = model.CartName;
            paymentCard.CardNumber = model.CartNumber;
            paymentCard.ExpireMonth = model.CartExpirationMonth;
            paymentCard.ExpireYear = model.CartExpirationYear;
            paymentCard.Cvc = model.CartCVV;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = model.FullName;
            buyer.Surname = "Doe";
            buyer.GsmNumber = model.Phone;
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = model.City;
            buyer.Country = "Turkey";
            buyer.ZipCode = model.PostalCode;
            request.Buyer = buyer;

            Address address = new Address();
            address.ContactName = model.FullName;
            address.City = model.City;
            address.Country = "Turkey";
            address.Description = model.Adress;
            address.ZipCode = model.PostalCode;
            request.ShippingAddress = address;//ev adresi
            request.BillingAddress = address;//fatura adresi

            // Address billingAddress = new Address();
            // billingAddress.ContactName = "Jane Doe";
            // billingAddress.City = "Istanbul";
            // billingAddress.Country = "Turkey";
            // billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            // billingAddress.ZipCode = "34742";

            List<BasketItem> basketItems = new List<BasketItem>();
            foreach (var item in cart.CardItems)
            {

                BasketItem basketItem = new BasketItem();
                basketItem.Id = item.CardItemId.ToString();
                basketItem.Name = item.product.Name;
                basketItem.Category1 = "Telefon";//bunuda çekmem gerek
                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                basketItem.Price = item.product.Price.ToString();
                basketItems.Add(basketItem);
            }
            request.BasketItems = basketItems;


            return await Payment.Create(request, options);

        }
    }
}

