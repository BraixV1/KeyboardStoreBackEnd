using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IPartInBuildRepository : IEntityRepository<App.DAL.DTO.PartInBuild>, IPartInBuildRepositoryCustom<App.DAL.DTO.PartInBuild>
{
    
}


public interface IPartInBuildRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllPartInBuildsIncludingAsync(Guid userId = default, bool noTracking = true);
}