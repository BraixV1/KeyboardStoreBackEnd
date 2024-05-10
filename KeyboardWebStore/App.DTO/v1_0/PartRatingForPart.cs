using System.ComponentModel.DataAnnotations;
using App.DTO.v1_0.Identity;

namespace App.DTO.v1_0;

public class PartRatingForPart
{
    public Guid Id { get; set; }
    [MaxLength(10240)] public string Comment { get; set; } = default!;

    public Guid PartId { get; set; }
    
    public int Rating { get; set; }

    [DataType(DataType.Date)] public DateTime PostedAt { get; set; }

    public Guid UserId { get; set; }
    public AppUserBll? User { get; set; }
}