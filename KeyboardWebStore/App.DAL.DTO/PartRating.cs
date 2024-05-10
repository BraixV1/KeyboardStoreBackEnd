using System.ComponentModel.DataAnnotations;
using App.DAL.DTO.Identity;
using Base.Contracts.Domain;

namespace App.DAL.DTO;
public class PartRating : IDomainEntityId
{
    public Guid Id { get; set; }
    [MaxLength(10240)] public string Comment { get; set; } = default!;

    public Guid PartId { get; set; }
    public Part? Part { get; set; } = default!;

    public int Rating { get; set; }

    [DataType(DataType.Date)] public DateTime PostedAt { get; set; }

    public Guid UserId { get; set; }
    public Identity.AppUser? User { get; set; } = default!;
}