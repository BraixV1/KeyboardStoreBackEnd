using System.ComponentModel.DataAnnotations;
using App.DAL.DTO.Identity;
using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class KeyboardBuild : IDomainEntityId, IDomainAppUser<Identity.AppUser>
{
    public Guid AppUserId { get; set; }
    public Identity.AppUser? AppUser { get; set; } = default!;

    [MaxLength(64)] public string BuildName { get; set; } = default!;

    [MaxLength(10240)] public string Description { get; set; } = default!;

    public ICollection<PartInBuildWithPart>? BuildParts { get; set; }
    public Guid Id { get; set; }
}