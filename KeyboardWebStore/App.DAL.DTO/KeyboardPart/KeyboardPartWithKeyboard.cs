using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class KeyboardPartWithKeyboard : IDomainEntityId
{
    public Guid KeyboardId { get; set; }
    public Keyboard? Keyboard { get; set; }

    public Guid PartId { get; set; }
    public Guid Id { get; set; }
}