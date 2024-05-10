using System.ComponentModel.DataAnnotations;

namespace App.DAL.DTO;

public class CategoryWithoutCollection
{
    public Guid Id { get; set; }
    [MaxLength(128)] public string CategoryName { get; set; } = default!;
}