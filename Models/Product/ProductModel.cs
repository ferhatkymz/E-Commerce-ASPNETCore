using System.ComponentModel.DataAnnotations;

namespace EF_MVC.Models
{
    public class ProductModel
    {
        [Display(Name = "Ürün Adı")]

        public string? Name { get; set; }
        [Display(Name = "Fiyat")]
        [Required(ErrorMessage = "{0} Alanı boş geçilemez")]
        [Range(0, 1000000, ErrorMessage = "{0} Alanı {1} ile {2} arasında olamlıdır")]
        public int Price { get; set; }


        [Display(Name = "indirim(%)")]
        public int Discount { get; set; }
        [Display(Name = "Ürün Açıklaması")]
        public string? Description { get; set; }
        public IFormFile? ImgFile { get; set; } = null!;
        public IFormFile? ImgFile2 { get; set; } = null!;
        public IFormFile? ImgFile3 { get; set; } = null!;

        [Display(Name = "Aktif")]
        public bool isActive { get; set; }
        [Display(Name = "AnaSayfa")]
        public bool isHome { get; set; }

        [Display(Name = "Kategori")]
        [Required(ErrorMessage = "Lütfen geçerli bir Kategori Seçiniz")]

        public int CategoryId { get; set; }
    }
}