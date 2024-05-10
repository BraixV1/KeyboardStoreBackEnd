namespace App.DTO.v1_0;

public class KeyboardPartWithPart
{
    public Guid Id { get; set; }
    public Guid KeyboardId { get; set; }

    public Guid PartId { get; set; }
    public Part.PartWithoutCollections? Part { get; set; } = default!;
}