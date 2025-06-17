using System.ComponentModel.DataAnnotations;

namespace EF_MVC.Models
{
    public class ProductDetail
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product product { get; set; } = null!;
        [MaxLength(350)]
        public string Img { get; set; } = null!;
    }
}