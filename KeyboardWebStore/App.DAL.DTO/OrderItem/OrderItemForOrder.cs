using System.ComponentModel.DataAnnotations;

namespace App.DAL.DTO;

public class OrderItemForOrder
{
    public Guid Id { get; set; }
    public Guid? KeyboardId { get; set; }
    public Keyboard? Keyboard { get; set; }

    public Guid? PartId { get; set; }
    public Part? Part { get; set; }

    public Guid OrderId { get; set; }
    
    public int Quantity { get; set; }
    [DataType(DataType.Currency)]public double Price { get; set; }

    [DataType(DataType.Date)] public DateTime AddedToBasket { get; set; }
}