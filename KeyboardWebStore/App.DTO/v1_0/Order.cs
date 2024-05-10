using System.ComponentModel.DataAnnotations;
using App.DTO.v1_0.HelperEnums;
using App.DTO.v1_0.Identity;

namespace App.DTO.v1_0;

public class Order
{
    [DataType(DataType.Date)] public DateTime OrderDate { get; set; }

    public int OrderNumber { get; set; } = new();

    public OrderStatus OrderStatus { get; set; } = default!;

    public ICollection<OrderItemForOrder>? OrderItemsCollection { get; set; }

    public Guid? AppUserId { get; set; }
    public AppUserBll? AppUser { get; set; }
    
    public Guid Id { get; set; }
    
    public string firstName { get; set; } = default!;

    public string lastName { get; set; } = default!;

    public string email { get; set; } = default!;

    public int phoneNumber { get; set; } = default!;

    public string addressLine { get; set; } = default!;

    public string city { get; set; } = default!;

    public string state { get; set; } = default!;

    public int zipCode { get; set; } = default!;
}