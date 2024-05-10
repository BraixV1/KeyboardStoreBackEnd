using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IPartCategoryRepository : IEntityRepository<App.DAL.DTO.PartCategory>, ICategoryRepositoryCustom<App.DAL.DTO.PartCategory>
{
    
}


public interface IPartCategoryRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllCategoriesIncludingAsync(Guid userId = default, bool noTracking = true);
}