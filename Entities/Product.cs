namespace EF_MVC.Models
{
  public class Product
  {
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Price { get; set; }
    public int Discount { get; set; } = 0;
    public string img { get; set; } = null!;
    public string? Description { get; set; }
    public bool isActive { get; set; }
    public bool isHome { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;//navigation property ve boş olamaz

    public List<ProductDetail> productDetails { get; set; } = new();
  }
}