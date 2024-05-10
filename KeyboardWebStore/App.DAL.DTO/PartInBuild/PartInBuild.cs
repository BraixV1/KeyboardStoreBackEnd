using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class PartInBuild : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid PartId { get; set; }
    public Part? Part { get; set; }

    public Guid KeyboardBuildId { get; set; }
    public KeyboardBuild? KeyboardBuild { get; set; }
}