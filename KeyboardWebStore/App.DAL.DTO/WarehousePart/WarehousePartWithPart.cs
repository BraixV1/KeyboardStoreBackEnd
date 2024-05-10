namespace App.DAL.DTO;

public class WarehousePartWithPart
{
    public Guid Id { get; set; }
    public int Quantity { get; set; } = default!;

    public Guid? PartId { get; set; }
    public PartWithoutWarehouse? Part { get; set; }

    public Guid? KeyboardId { get; set; }

    public Guid WareHouseId { get; set; }
    public WarehouseWithoutCollection Warehouse { get; set; } = default!;
}