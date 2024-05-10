using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Contest: IDomainEntityId
{
    public Guid Id { get; set; }
    
    public string ContestName { get; set; } = default!;
}