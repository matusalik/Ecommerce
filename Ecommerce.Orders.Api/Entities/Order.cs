namespace Ecommerce.Orders.Api.Entities;
public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public string CustomerSurname { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pending";
    public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
