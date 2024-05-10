using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0;

public class KeyboardWithoutCollections
{
    public Guid Id { get; set; }
    
    [MaxLength(128)] public string Name { get; set; } = default!;

    [MaxLength(128)] public string ItemCode { get; set; } = default!;

    [DataType(DataType.Currency)] public double Price { get; set; }

    [MaxLength(10240)] public string Description { get; set; } = default!;

    [MaxLength(10240)] public string? ImagePath { get; set; }
    
}