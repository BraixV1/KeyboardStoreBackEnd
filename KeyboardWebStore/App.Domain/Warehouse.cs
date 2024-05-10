using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Warehouse : BaseEntityId
{
    [MaxLength(128)] public string WarehouseName { get; set; } = default!;

    public ICollection<WarehousePart>? WarehousePartsCollection { get; set; }
}