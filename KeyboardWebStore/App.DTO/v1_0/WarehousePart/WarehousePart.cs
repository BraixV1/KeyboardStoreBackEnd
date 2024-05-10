namespace App.DTO.v1_0;

public class WarehousePart
{
    public Guid Id { get; set; }
    public int Quantity { get; set; } = default!;

    public Guid? PartId { get; set; }
    public Part.Part? Part { get; set; }

    public Guid? KeyboardId { get; set; }
    public Keyboard? Keyboard { get; set; }

    public Guid WareHouseId { get; set; }
    public Warehouse? Warehouse { get; set; } = default!;
    
}