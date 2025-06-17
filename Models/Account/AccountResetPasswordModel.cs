using System.ComponentModel.DataAnnotations;

namespace EF_MVC.Models
{
    public class AccountResetPasswordModel
    {
        public string token { get; set; } = null!;
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Lütfen Yeni şifrenizi giriniz")]
        [Display(Name = "Yeni Parola")]
        [DataType(DataType.Password)]

        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Lütfen Yeni şifrenizi tekrar giriniz")]
        [Display(Name = "Yeni Parolayı Tekrar")]

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Parola Eşleşmiyor")]
        public string ConfirmPassword { get; set; } = null!;
    }
}