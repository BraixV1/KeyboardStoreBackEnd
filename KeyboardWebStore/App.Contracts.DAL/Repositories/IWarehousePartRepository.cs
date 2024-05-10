using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IWarehousePartRepository : IEntityRepository<App.DAL.DTO.WarehousePart>, IWarehousePartRepositoryCustom<App.DAL.DTO.WarehousePart>
{
    
}

public interface IWarehousePartRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllWarehousePartsIncludingPartsAsync(Guid userId = default, bool noTracking = true);
}