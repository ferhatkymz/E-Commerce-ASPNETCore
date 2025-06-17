using System.ComponentModel.DataAnnotations;

namespace EF_MVC.Models
{
    public class UserEditModel
    {
        [Required]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = null!;

        [Required]
        [Display(Name = "Eposta")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Display(Name = "Parola")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Parola Tekrar")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Parola eşleşmiyor")]
        public string? ConfirmPassword { get; set; }

        public IList<string>? SelectedRoles { get; set; }
    }
}