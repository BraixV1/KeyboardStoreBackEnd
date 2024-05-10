using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class KeyboardPart : IDomainEntityId
{
    public Guid Id { get; set; }
    
    public Guid KeyboardId { get; set; }
    public Keyboard? Keyboard { get; set; }
    public Guid PartId { get; set; }
    public Part? Part { get; set; }
}