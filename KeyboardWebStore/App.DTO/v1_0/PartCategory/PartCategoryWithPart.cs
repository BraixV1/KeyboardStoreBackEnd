namespace App.DTO.v1_0;

public class PartCategoryWithPart
{
    public Guid Id { get; set; }
    public Guid? PartId { get; set; }
    public Part.Part? Part { get; set; }

    public Guid? KeyboardId { get; set; }

    public Guid CategoryId { get; set; }
    public CategoryWithoutCollections? Category { get; set; }
}