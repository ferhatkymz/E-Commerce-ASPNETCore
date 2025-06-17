using System.ComponentModel.DataAnnotations;

namespace EF_MVC.Models
{
    public class RoleEditModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        [Display(Name = "Role Adı")]
        public string RoleName { get; set; } = null!;
    }
}