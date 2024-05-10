using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IKeyboardBuildRepository : IEntityRepository<App.DAL.DTO.KeyboardBuild>, IKeyboardBuildRepositoryCustom<App.DAL.DTO.KeyboardBuild>
{
    
}

public interface IKeyboardBuildRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllKeyboardBuildsIncludingAsync(Guid userId = default, bool noTracking = true);
}