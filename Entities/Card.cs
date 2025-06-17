namespace EF_MVC.Models
{
    public class Card
    {
        public int CardId { get; set; }
        public string CustomerName { get; set; } = null!;

        public List<CardItem> CardItems { get; set; } = new();//Bir müşteri birden fazla kartı olabilir 

        public double subTotal()
        {
            double value = CardItems.Sum(p => p.product.Price * p.Amount);
            return Math.Round(value, 3);
        }

        public double Total()
        {
            if (subTotal() > 25000)//ücretsiz kargo
            {
                return Math.Round(subTotal() * 1.2, 2);
            }
            else//25000 altı ise 50 lira kargo ücreti var
            {
                return Math.Round((subTotal() * 1.2) + 50, 2);
            }
        }

        public void addItem(Product product, int amount)
        {
            var item = CardItems.Where(p => p.ProductId == product.Id).FirstOrDefault();
            if (item != null)
            {
                item.Amount += amount;
            }
            else
            {
                CardItems.Add(new CardItem { product = product, Amount = amount });
            }

        }
        public void removeItem(Product product, int amount)
        {
            var item = CardItems.Where(p => p.ProductId == product.Id).FirstOrDefault();
            if (item != null)
            {
                item.Amount -= amount;
                if (item.Amount == 0)
                {
                    CardItems.Remove(item);
                }
            }

        }
    }


}