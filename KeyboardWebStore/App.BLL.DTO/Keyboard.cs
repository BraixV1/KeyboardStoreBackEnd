using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class Keyboard : IDomainEntityId
{
    public Guid Id { get; set; }
    
    [MaxLength(128)] public string Name { get; set; } = default!;

    [MaxLength(128)] public string ItemCode { get; set; } = default!;

    [DataType(DataType.Currency)] public double Price { get; set; }

    [MaxLength(10240)] public string Description { get; set; } = default!;

    [MaxLength(10240)] public string? ImagePath { get; set; }

    public ICollection<WarehousePart>? WarehouseKeyboardsCollection { get; set; }

    public ICollection<PartCategory>? PartCategoriesCollection { get; set; }

    public ICollection<KeyboardRating>? KeyboardRatingsCollection { get; set; } = default!;

    public ICollection<KeyboardPart>? KeyboardPartsCollection { get; set; } = default!;
}