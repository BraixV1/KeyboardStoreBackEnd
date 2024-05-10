using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Part : BaseEntityId
{
    [MaxLength(256)] public string PartName { get; set; } = default!;

    [MaxLength(256)] public string PartCode { get; set; } = default!;

    [DataType(DataType.Currency)] public double Price { get; set; } = default!;

    public int Quantity { get; set; } = default!;

    [MaxLength(10240)] public string? ImagePath { get; set; }

    public ICollection<WarehousePart>? WarehousePartsCollection { get; set; }

    public ICollection<PartRating>? PartCommentsCollection { get; set; }

    public ICollection<PartCategory>? PartCategoriesCollection { get; set; }

    public ICollection<KeyboardPart>? KeyboardPartsCollection { get; set; }
}