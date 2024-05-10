using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface ICategoryRepository : IEntityRepository<App.DAL.DTO.Category>, ICategoryRepositoryCustom<App.DAL.DTO.Category>
{
    
}

public interface ICategoryRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllCategoriesIncludingPartsAsync(Guid userId = default, bool noTracking = true);
}