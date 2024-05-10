using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class KeyboardRating : BaseEntityId
{
    public Guid KeyboardId { get; set; }
    public Keyboard? Keyboard { get; set; } = default!;

    public int Rating { get; set; }

    [MaxLength(256)] public string Comment { get; set; } = default!;

    public DateTime postedAtDt { get; set; } = default;

    public Guid UserId { get; set; }
    public AppUser? User { get; set; } = default!;
}