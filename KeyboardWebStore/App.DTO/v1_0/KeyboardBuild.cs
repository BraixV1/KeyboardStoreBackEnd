using System.ComponentModel.DataAnnotations;
using App.DTO.v1_0.Identity;

namespace App.DTO.v1_0;

public class KeyboardBuild
{
    public Guid AppUserId { get; set; }
    public AppUserBll? AppUser { get; set; }

    [MaxLength(64)] public string BuildName { get; set; } = default!;

    [MaxLength(10240)] public string Description { get; set; } = default!;
    public Guid Id { get; set; }

    public ICollection<PartInBuildWithPart>? BuildParts { get; set; }
}