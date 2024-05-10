namespace App.DTO.v1_0;

public class KeyboardPart
{
    public Guid Id { get; set; }
    public Guid KeyboardId { get; set; }
    public Keyboard? Keyboard { get; set; }
    public Guid PartId { get; set; }
    public Part.Part? Part { get; set; }
}