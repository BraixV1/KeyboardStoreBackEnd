using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class KeyboardPartWithPart : IDomainEntityId
{
    public Guid KeyboardId { get; set; }

    public Guid PartId { get; set; }
    public Part? Part { get; set; } = default!;
    public Guid Id { get; set; }
}