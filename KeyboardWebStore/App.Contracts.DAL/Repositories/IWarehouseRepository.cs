using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IWarehouseRepository : IEntityRepository<App.DAL.DTO.Warehouse>, IWarehouseRepositoryCustom<App.DAL.DTO.Warehouse>
{
    
    
    
}

public interface IWarehouseRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllWarehousesIncludingAsync(Guid userId = default, bool noTracking = true);
}