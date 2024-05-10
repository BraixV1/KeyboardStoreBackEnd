using System.ComponentModel.DataAnnotations;
using App.DTO.v1_0.Identity;

namespace App.DTO.v1_0;

public class KeyboardRating
{
    public Guid Id { get; set; }
    public Guid KeyboardId { get; set; }
    public Keyboard? Keyboard { get; set; } = default!;

    public int Rating { get; set; }

    public DateTime postedAtDt { get; set; } = default;

    [MaxLength(256)] public string Comment { get; set; } = default!;

    public Guid UserId { get; set; }
    public AppUserBll? User { get; set; }
}