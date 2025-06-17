using System.ComponentModel.DataAnnotations;

namespace EF_MVC.Models
{
    public class CreateRoleModel
    {
        [Required]
        [StringLength(30)]
        [Display(Name = "Role Adı")]
        public string RoleName { get; set; } = null!;
    }
}