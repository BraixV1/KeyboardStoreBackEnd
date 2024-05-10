namespace App.DAL.DTO;

public class PartInBuildWithBuild
{
    public Guid Id { get; set; }
    public Guid PartId { get; set; }

    public Guid KeyboardBuildId { get; set; }
    public KeyboardBuild? KeyboardBuild { get; set; }
}