using Base.Domain;

namespace App.Domain;

public class PartInBuild : BaseEntityId
{
    public Guid PartId { get; set; }
    public Part? Part { get; set; }

    public Guid KeyboardBuildId { get; set; }
    public KeyboardBuild? KeyboardBuild { get; set; }
}