using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IOrderRepository : IEntityRepository<App.DAL.DTO.Order>, IOrderRepositoryCustom<App.DAL.DTO.Order>
{
    
}

public interface IOrderRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllOrderIncludingEverythingAsync();
}