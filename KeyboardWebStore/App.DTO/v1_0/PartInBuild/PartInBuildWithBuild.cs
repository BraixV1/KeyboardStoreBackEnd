namespace App.DTO.v1_0;

public class PartInBuildWithBuild
{
    public Guid Id { get; set; }

    public Guid KeyboardBuildId { get; set; }
    public KeyboardBuild? KeyboardBuild { get; set; }
}