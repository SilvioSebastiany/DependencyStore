namespace DependencyStore.Models;

public class Order
{
    public Order(decimal deliveryFee, decimal discount, List<Product> products)
    {
        Code = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        Date = DateTime.Now;
        DeliveryFee = deliveryFee;
        Discount = discount;
    }

    public string Code { get; set; }
    public DateTime Date { get; set; }
    public decimal DeliveryFee { get; set; }
    public decimal Discount { get; set; }
    public List<Product> Products { get; set; }
    public decimal SubTotal => Products.Sum(p => p.Price);
    public decimal Total => SubTotal + DeliveryFee - Discount;
}