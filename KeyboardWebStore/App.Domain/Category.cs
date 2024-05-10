using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Category : BaseEntityId
{
    [MaxLength(128)] public string CategoryName { get; set; } = default!;


    public ICollection<PartCategory>? PartCategories { get; set; }
}