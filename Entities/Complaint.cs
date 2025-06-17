namespace EF_MVC.Models
{
    public class Complaint
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public User user { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}