using System.ComponentModel.DataAnnotations;
using App.DAL.DTO.Identity;

namespace App.DAL.DTO;

public class KeyboardRatingForKeyboard
{
    public Guid Id { get; set; }
    public Guid KeyboardId { get; set; }

    public int Rating { get; set; }

    [MaxLength(256)] public string Comment { get; set; } = default!;
    
    public DateTime postedAtDt { get; set; } = default!;

    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
}