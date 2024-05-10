using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.BLL.DTO;

public class Warehouse : IDomainEntityId
{
    public Guid Id { get; set; }
    [MaxLength(128)] public string WarehouseName { get; set; } = default!;

    public ICollection<WarehousePart>? WarehousePartsCollection { get; set; }
}