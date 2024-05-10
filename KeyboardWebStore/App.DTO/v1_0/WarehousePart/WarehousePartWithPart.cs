namespace App.DTO.v1_0;

public class WarehousePartWithPart
{
    public Guid Id { get; set; }
    public int Quantity { get; set; } = default!;

    public Guid? PartId { get; set; }
    public Part.partWithoutWarehouse? Part { get; set; }

    public Guid? KeyboardId { get; set; }

    public Guid WareHouseId { get; set; }
    public WarehouseWithoutCollection Warehouse { get; set; } = default!;
}