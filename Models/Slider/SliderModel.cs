using System.ComponentModel.DataAnnotations;

namespace EF_MVC.Models
{
    public class SliderModel
    {
        [Display(Name = "Başlık")]
        public string? Title { get; set; }
        public string? Img { get; set; }
        [Display(Name = "Açıklama")]
        [StringLength(maximumLength: 250, ErrorMessage = "{0} alanı minimum {2} maxsimum {1} karakterlik bir yazı olmalıdır", MinimumLength = 10)]
        public string? Description { get; set; }
        

        [Required(ErrorMessage = "resmin kaçıncı sırada olmasını istiyorsunuz örneğin 1. resim olsun ")]
        public int Index { get; set; }
        [Display(Name = "Aktif")]
        public bool isActive { get; set; }
    }
}