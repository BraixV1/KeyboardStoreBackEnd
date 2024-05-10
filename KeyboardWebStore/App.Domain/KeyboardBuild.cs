using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class KeyboardBuild : BaseEntityId, IDomainAppUser<AppUser>
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; } = default!;

    [MaxLength(64)] public string BuildName { get; set; } = default!;

    [MaxLength(10240)] public string Description { get; set; } = default!;
    
    public ICollection<PartInBuild>? BuildParts { get; set; }
}