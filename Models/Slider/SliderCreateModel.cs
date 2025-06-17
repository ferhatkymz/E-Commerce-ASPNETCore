using System.ComponentModel.DataAnnotations;

namespace EF_MVC.Models
{
    public class SliderCreateModel : SliderModel
    {
        [Display(Name = "Resim")]
        [Required(ErrorMessage = "Lütfen bir {0} Seçiniz")]
        public IFormFile ImgFile { get; set; } = null!;
    }
}