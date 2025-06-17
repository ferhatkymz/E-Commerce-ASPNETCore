using System.ComponentModel.DataAnnotations;

namespace EF_MVC.Models;

public class AccountCreateModel
{
    [Required(ErrorMessage = "AdSoyad Alanı zorunlu bir alan")]
    [Display(Name = "AdSoyad")]

    public string FullName { get; set; } = null!;
    [Required(ErrorMessage = "EPosta Zorunlu Alandır")]
    [Display(Name = "EPosta")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Parola zorunlu alandır")]
    [Display(Name = "Parola")]
    [DataType(DataType.Password)]
    // [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*(),.?\":{}|<>]).{8,}$", 
    // ErrorMessage = "Şifre en az bir büyük harf, bir küçük harf ve bir özel karakter içermeli ve en az 8 karakter olmalıdır.")]
    public string Password { get; set; } = null!;


    [Required(ErrorMessage = "Parola zorunlu alandır")]
    [DataType(DataType.Password)]
    [Display(Name = "Parolayı Tekrar giriniz")]
    [Compare("Password", ErrorMessage = "Parola Eşleşmiyor")]
    public string ConfirmPassword { get; set; } = null!;
}