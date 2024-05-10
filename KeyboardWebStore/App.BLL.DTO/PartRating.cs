using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.BLL.DTO;
public class PartRating : IDomainEntityId
{
    public Guid Id { get; set; }
    [MaxLength(10240)] public string Comment { get; set; } = default!;

    public Guid PartId { get; set; }
    public Part? Part { get; set; } = default!;

    public int Rating { get; set; }

    [DataType(DataType.Date)] public DateTime PostedAt { get; set; }

    public Guid UserId { get; set; }
    public AppUser? User { get; set; } = default!;
}