using Base.Contracts.Domain;
using Base.Domain;

namespace App.BLL.DTO;
public class PartCategory : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid? PartId { get; set; }
    public Part? Part { get; set; }

    public Guid? KeyboardId { get; set; }
    public Keyboard? Keyboard { get; set; }

    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
}