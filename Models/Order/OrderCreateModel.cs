using System.ComponentModel.DataAnnotations;

namespace EF_MVC.Models
{
    public class OrderCreateModel
    {
        [Display(Name = "İsim Soyisim")]
        [MaxLength(85)]
        [Required(ErrorMessage = " İsim ve soyisminizi boş geçmeyiniz")]
        public string FullName { get; set; } = null!;

        [Display(Name = "Telefon")]
        [MaxLength(11)]
        [Required(ErrorMessage = "Telefon numaranızı giriniz")]
        [Phone(ErrorMessage = "Geçerli bir Telefon numarası giriniz")]
        [RegularExpression(@"^05\d{9}$", ErrorMessage = "Telefon numarası 05 ile başlamalı ve 11 haneli olmalıdır.")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Posta kodu giriniz")]
        [Display(Name = "Posta Kodu")]
        public string PostalCode { get; set; } = null!;
        [Display(Name = "Şehir")]
        [Required(ErrorMessage = "Şehir giriniz")]
        [MaxLength(100)]
        public string City { get; set; } = null!;
        [Display(Name = "Adres")]
        [MaxLength(200)]
        [Required(ErrorMessage = "Açık adres giriniz")]

        public string Adress { get; set; } = null!;
        [Display(Name = "Sipariş Notu")]

        public string? OrderNotes { get; set; }

        [Display(Name = "Kart Sahibinin Adı")]
        [Required(ErrorMessage = "Kart Sahibinin adı soyadı")]
        public string CartName { get; set; } = null!;
        [Display(Name = "Kart Numarası")]
        [Required(ErrorMessage = "Kart numarası zorunlu")]
        public string CartNumber { get; set; } = null!;
        [Display(Name = "Yıl")]
        [Required(ErrorMessage = "Kartın son kullanılacak yılını giriniz")]

        public string CartExpirationYear { get; set; } = null!;
        [Display(Name = "Ay")]
        [Required(ErrorMessage = "Kartın son kullanılacak Ayı giriniz")]

        public string CartExpirationMonth { get; set; } = null!;
        [Display(Name = "CVV")]
        [Required(ErrorMessage = "Kartın CVV bilgisini giriniz")]
        public string CartCVV { get; set; } = null!;
    }
}