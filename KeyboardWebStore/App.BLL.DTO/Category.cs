using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.BLL.DTO;

public class Category : IDomainEntityId
{
    public Guid Id { get; set; }
    [MaxLength(128)] public string CategoryName { get; set; } = default!;
    
    public ICollection<PartCategory>? PartCategories { get; set; }
}