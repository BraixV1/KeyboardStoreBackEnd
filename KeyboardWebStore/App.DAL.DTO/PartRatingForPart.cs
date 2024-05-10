using System.ComponentModel.DataAnnotations;
using App.DAL.DTO.Identity;

namespace App.DAL.DTO;

public class PartRatingForPart
{
    public Guid Id { get; set; }
    [MaxLength(10240)] public string Comment { get; set; } = default!;

    public Guid PartId { get; set; }
    
    public int Rating { get; set; }

    [DataType(DataType.Date)] public DateTime PostedAt { get; set; }

    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
}