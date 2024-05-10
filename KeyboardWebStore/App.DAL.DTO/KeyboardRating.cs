using System.ComponentModel.DataAnnotations;
using App.DAL.DTO.Identity;
using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class KeyboardRating : IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid KeyboardId { get; set; }
    public Keyboard? Keyboard { get; set; } = default!;

    public int Rating { get; set; }
    
    public DateTime postedAtDt { get; set; }

    [MaxLength(256)] public string Comment { get; set; } = default!;

    public Guid UserId { get; set; }
    public Identity.AppUser? User { get; set; } = default!;
}