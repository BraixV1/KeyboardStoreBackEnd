using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Warehouse : IDomainEntityId
{
    public Guid Id { get; set; }
    [MaxLength(128)] public string WarehouseName { get; set; } = default!;

    public ICollection<WarehousePartForWareHouse>? WarehousePartsCollection { get; set; }
}