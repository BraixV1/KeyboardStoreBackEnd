using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IOrderItemRepository : IEntityRepository<App.DAL.DTO.OrderItem>, IOrderITemRepositoryCustom<App.DAL.DTO.OrderItem>
{
    
}


public interface IOrderITemRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllOrderItemsIncludingAsync(Guid userId = default, bool Notracking = true);
}