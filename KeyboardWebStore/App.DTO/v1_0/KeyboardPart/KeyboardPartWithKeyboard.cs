namespace App.DTO.v1_0;

public class KeyboardPartWithKeyboard
{
    public Guid Id { get; set; }
    public Guid KeyboardId { get; set; }
    public KeyboardWithoutCollections? Keyboard { get; set; }

    public Guid PartId { get; set; }
}