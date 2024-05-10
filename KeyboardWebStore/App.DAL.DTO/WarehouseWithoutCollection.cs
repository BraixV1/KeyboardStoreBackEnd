using System.ComponentModel.DataAnnotations;

namespace App.DAL.DTO;

public class WarehouseWithoutCollection
{
    public Guid Id { get; set; }
    [MaxLength(128)] public string WarehouseName { get; set; } = default!;
}