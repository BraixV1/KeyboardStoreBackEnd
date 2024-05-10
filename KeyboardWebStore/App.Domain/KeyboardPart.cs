using Base.Domain;

namespace App.Domain;

public class KeyboardPart : BaseEntityId
{
    public Guid KeyboardId { get; set; }
    public Keyboard? Keyboard { get; set; }
    public Guid PartId { get; set; }
    public Part? Part { get; set; }
}