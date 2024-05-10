using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Part : IDomainEntityId
{
    public Guid Id { get; set; }
    
    [MaxLength(256)] public string PartName { get; set; } = default!;

    [MaxLength(256)] public string PartCode { get; set; } = default!;

    [DataType(DataType.Currency)] public double Price { get; set; } = default!;

    public int Quantity { get; set; } = default!;

    [MaxLength(10240)] public string? ImagePath { get; set; }

    public ICollection<WarehousePartWithKeyboard>? WarehousePartsCollection { get; set; }

    public ICollection<PartRatingForPart>? PartCommentsCollection { get; set; }

    public ICollection<PartCategoryWithKeyboard>? PartCategoriesCollection { get; set; }
    
    public ICollection<KeyboardPartWithKeyboard>? KeyboardPartCollection { get; set; }
    
}