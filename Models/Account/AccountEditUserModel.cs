using System.ComponentModel.DataAnnotations;

namespace EF_MVC.Models
{
    public class AccountEditUserModel
    {
        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = null!;
        [Required(ErrorMessage = "Zorunlu Alan")]
        [Display(Name = "E-Posta")]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}