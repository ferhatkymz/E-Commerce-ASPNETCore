namespace EF_MVC.Models
{
    public class CategoryGetModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Url { get; set; } = null!;
        public int Count { get; set; }
    }
}