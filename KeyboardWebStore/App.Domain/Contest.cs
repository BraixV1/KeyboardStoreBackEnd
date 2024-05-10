using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Contest : BaseEntityId, IDomainAppUser<AppUser>
{
    [MaxLength(2048)]
    [Display(ResourceType = typeof(App.Resources.Domain.Contest), Name = nameof(ContestName))]
    [Column(TypeName = "jsonb")]
    public LangStr ContestName { get; set; } = default!;

    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public DateTime DT { get; set; }

    [Range(minimum: 1, maximum: 2, ErrorMessageResourceType = typeof(Base.Resources.Attribute), ErrorMessageResourceName = "ValueBetween")]
    public decimal DVal { get; set; }
    
}

