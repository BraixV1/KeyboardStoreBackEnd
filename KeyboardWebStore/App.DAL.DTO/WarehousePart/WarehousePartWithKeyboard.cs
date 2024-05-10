namespace App.DAL.DTO;

public class WarehousePartWithKeyboard
{
    public Guid Id { get; set; }
    public int Quantity { get; set; } = default!;

    public Guid? PartId { get; set; }

    public Guid? KeyboardId { get; set; }
    public Keyboard? Keyboard { get; set; }

    public Guid WareHouseId { get; set; }
    public WarehouseWithoutCollection Warehouse { get; set; } = default!;
}