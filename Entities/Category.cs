namespace EF_MVC.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string URL { get; set; } = null!;
        public List<Product> Products { get; set; } = new();//bir kategori birden fazla üründe olabilir ve tabiki bellekte referansını türettim.
    }
}