namespace EF_MVC.Models
{
    public class CardItem
    {
        public int CardItemId { get; set; }
        public int ProductId { get; set; }
        public Product product { get; set; } = null!;//navigation property
        public int CardId { get; set; }
        public Card card { get; set; } = null!;//navigation property
        public int Amount { get; set; }

    }
}