using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class OrderItem : BaseEntityId
{
    public Guid? KeyboardId { get; set; }
    public Keyboard? Keyboard { get; set; }

    public Guid? PartId { get; set; }
    public Part? Part { get; set; }

    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    
    public int Quantity { get; set; }
    public double Price { get; set; }

    [DataType(DataType.Date)] public DateTime AddedToBasket { get; set; }
}