using System.ComponentModel.DataAnnotations;
using App.DTO.v1_0.Identity;

namespace App.DTO.v1_0;

public class KeyboardRatingForKeyboard
{
    public Guid Id { get; set; }
    public Guid KeyboardId { get; set; }

    public int Rating { get; set; }

    [MaxLength(256)] public string Comment { get; set; } = default!;

    public DateTime postedAtDt { get; set; } = default!;

    public Guid UserId { get; set; }
    public AppUserBll? User { get; set; }
}