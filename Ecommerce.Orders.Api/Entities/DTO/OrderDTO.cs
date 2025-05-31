namespace Ecommerce.Orders.Api.Entities.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerSurname { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; } = "Pending";

        public List<ProductDTO> Products { get; set; } = new();
    }
}
