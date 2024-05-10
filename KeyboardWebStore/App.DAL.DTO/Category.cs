using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Category : IDomainEntityId
{
    public Guid Id { get; set; }
    [MaxLength(128)] public string CategoryName { get; set; } = default!;
    
    public ICollection<PartCategoryForCategory>? PartCategories { get; set; }
}