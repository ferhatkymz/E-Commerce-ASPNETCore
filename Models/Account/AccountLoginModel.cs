using System.ComponentModel.DataAnnotations;

namespace EF_MVC.Models;

public class AccountLoginModel
{
    [Required(ErrorMessage = "EPostanızı Giriniz")]
    [Display(Name = "EPosta")]
    [EmailAddress]
    public string Email { get; set; } = null!;


    [Required(ErrorMessage = "Parola zorunlu alandır")]
    [Display(Name = "Parola")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "BeniHatırla")]
    public bool RememberMe { get; set; } = true;
}