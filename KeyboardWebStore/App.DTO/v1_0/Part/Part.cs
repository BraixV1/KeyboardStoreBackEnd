using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0.Part;

public class Part
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
    
    public ICollection<KeyboardPartWithKeyboard>? KeyboardPartsCollection { get; set; } = default!;
    
    public ICollection<PartInBuildWithBuild>? BuildPartsCOllection { get; set; }
}