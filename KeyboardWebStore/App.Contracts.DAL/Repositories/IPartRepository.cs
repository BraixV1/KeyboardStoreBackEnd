using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IPartRepository : IEntityRepository<App.DAL.DTO.Part>, IPartRepositoryCustom<App.DAL.DTO.Part>
{
    
}

public interface IPartRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllPartsIncludingAsync(Guid userId = default, bool noTracking = true);
}