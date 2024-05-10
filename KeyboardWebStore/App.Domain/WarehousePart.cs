using Base.Domain;

namespace App.Domain;

public class WarehousePart : BaseEntityId
{
    public int Quantity { get; set; } = default!;

    public Guid? PartId { get; set; }
    public Part? Part { get; set; }

    public Guid? KeyboardId { get; set; }
    public Keyboard? Keyboard { get; set; }

    public Guid WareHouseId { get; set; }
    public Warehouse Warehouse { get; set; } = default!;
}