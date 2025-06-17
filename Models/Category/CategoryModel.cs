using System.ComponentModel.DataAnnotations;

namespace EF_MVC.Models;
public class CategoryModel
{
    [Display(Name = "Kategori Adı")]
    [StringLength(50, ErrorMessage = "{0} minimum {2} karakter maximum {1} karakter uzunluğunda olmalıdır", MinimumLength = 2)]
    [Required(ErrorMessage = "Lütfen {0} Alanını boş geçmeyiniz")]
    public string Name { get; set; } = null!;

    [Display(Name = "Adres")]
    [StringLength(50, ErrorMessage = "{0} minimum {2} karakter maximum {1} karakter uzunluğunda olmalıdır", MinimumLength = 2)]
    [Required(ErrorMessage = "Lütfen {0} Alanını boş geçmeyiniz")]
    public string Url { get; set; } = null!;
}