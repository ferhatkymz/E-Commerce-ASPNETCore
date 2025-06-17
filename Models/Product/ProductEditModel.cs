using System.ComponentModel.DataAnnotations;

namespace EF_MVC.Models
{
    public class ProductEditModel : ProductModel
    {
        public int Id { get; set; }
        [Display(Name = "Resim Adı")]
        public string ImgName { get; set; } = null!;
    }
}

