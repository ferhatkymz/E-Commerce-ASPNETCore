namespace EF_MVC.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string img { get; set; }=null!;
        public int Index { get; set; }
        public bool isActive { get; set; }
    }
}