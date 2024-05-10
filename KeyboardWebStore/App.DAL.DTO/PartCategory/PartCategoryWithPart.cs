using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class PartCategoryWithPart : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid? PartId { get; set; }
    public Part? Part { get; set; }

    public Guid? KeyboardId { get; set; }

    public Guid CategoryId { get; set; }
    public CategoryWithoutCollection? Category { get; set; }
}