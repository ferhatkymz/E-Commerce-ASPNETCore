namespace EF_MVC;

public class ProductGetModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Price { get; set; }
    public int Discount { get; set; }
    public string img { get; set; } = null!;
    public bool isActive { get; set; }
    public bool isHome { get; set; }
    public string CategoryName { get; set; } = null!;
}