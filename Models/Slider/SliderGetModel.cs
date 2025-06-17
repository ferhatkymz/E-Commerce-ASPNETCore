namespace EF_MVC.Models
{
    public class SliderGetModel
    {
        public int Id { get; set; }
        public string Img { get; set; } = null!;
        public string? Title { get; set; }
        public bool isActive { get; set; }
        public int Index { get; set; }
    }
}