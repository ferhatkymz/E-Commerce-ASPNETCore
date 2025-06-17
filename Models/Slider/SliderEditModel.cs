using System.ComponentModel.DataAnnotations;

namespace EF_MVC.Models
{
    public class SliderEditModel : SliderModel
    {
        public int Id { get; set; }
        [Display(Name = "Resmi Güncelle")]
        public IFormFile? EditFile { get; set; }

    }
}