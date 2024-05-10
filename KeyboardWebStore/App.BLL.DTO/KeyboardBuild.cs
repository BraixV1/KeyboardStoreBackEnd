using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.BLL.DTO;

public class KeyboardBuild : IDomainEntityId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; } = default!;

    [MaxLength(64)] public string BuildName { get; set; } = default!;

    [MaxLength(10240)] public string Description { get; set; } = default!;
    public Guid Id { get; set; }

    public ICollection<PartInBuild>? BuildParts { get; set; }
}