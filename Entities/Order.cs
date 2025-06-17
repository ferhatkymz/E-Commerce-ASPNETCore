using System.ComponentModel.DataAnnotations;

namespace EF_MVC.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Parse(DateTime.Now.ToString("dd.MM.yyyy"));
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        [MaxLength(85)]
        public string FullName { get; set; } = null!;
        [MaxLength(11)]
        public string Phone { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        [MaxLength(100)]
        public string City { get; set; } = null!;
        [MaxLength(200)]
        public string Adress { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        [MaxLength(300)]
        public string? OrderNotes { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new();

        public double subTotal()
        {
            return Math.Round(OrderDetails.Sum(t => t.Price * t.Amount), 2);
        }
        public double Total()
        {
            if (subTotal() > 25000)//ücretsiz kargo
            {
                return Math.Round(OrderDetails.Sum(t => t.Price * t.Amount) * 1.20, 2);
            }
            else
            {
                return Math.Round((OrderDetails.Sum(t => t.Price * t.Amount) * 1.20) + 50, 2);
            }
        }

        public string cargoStatus()
        {
            if (subTotal() > 25000)//ücretsiz kargo
            {
                return "Ücretsiz";
            }
            else
            {
                return "50";
            }
        }
    }
}