using System.ComponentModel.DataAnnotations;
using App.Domain.HelperEnums;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Order : BaseEntityId
{
    [DataType(DataType.Date)] public DateTime OrderDate { get; set; }

    public int OrderNumber { get; set; } = new();

    public OrderStatus OrderStatus { get; set; } = default!;

    public ICollection<OrderItem>? OrderItemsCollection { get; set; }

    public Guid? AppUserId { get; set; }
    public AppUser? AppUser { get; set; } = default!;

    public string firstName { get; set; } = default!;

    public string lastName { get; set; } = default!;

    public string email { get; set; } = default!;

    public int phoneNumber { get; set; } = default!;

    public string addressLine { get; set; } = default!;

    public string city { get; set; } = default!;

    public string state { get; set; } = default!;

    public int zipCode { get; set; } = default!;
}