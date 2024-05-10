using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0.Part;

public class PartWithoutCollections
{
    public Guid Id { get; set; }
    
    [MaxLength(256)] public string PartName { get; set; } = default!;

    [MaxLength(256)] public string PartCode { get; set; } = default!;

    [DataType(DataType.Currency)] public double Price { get; set; } = default!;

    public int Quantity { get; set; } = default!;

    [MaxLength(10240)] public string? ImagePath { get; set; }

}