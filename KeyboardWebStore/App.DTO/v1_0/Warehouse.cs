using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0;

public class Warehouse
{
    public Guid Id { get; set; }
    [MaxLength(128)] public string WarehouseName { get; set; } = default!;

    public ICollection<WareHousePartForWarehouse>? WarehousePartsCollection { get; set; }
}