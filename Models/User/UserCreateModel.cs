using System.ComponentModel.DataAnnotations;

namespace EF_MVC.Models
{
    public class UserCreateModel
    {
        [Required(ErrorMessage = "İsim soyisim zorunlu alandır.")]
        [Display(Name = "İsim Soyisim")]
        public string FullName { get; set; } = null!;
        [Required(ErrorMessage = "E-Posta zorunlu Alandır.")]
        [Display(Name = "E-Posta")]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}