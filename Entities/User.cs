using Microsoft.AspNetCore.Identity;

namespace EF_MVC.Models
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; } = null!;
        public List<Complaint> Complaints { get; set; } = new();
    }
}