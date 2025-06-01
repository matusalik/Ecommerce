namespace Ecommerce.Orders.Api.Entities.DTO;
public class UpdateOrderPatchDTO
{
    public string? CustomerName { get; set; }
    public string? CustomerSurname { get; set; }
    public string? Status { get; set; }
}
