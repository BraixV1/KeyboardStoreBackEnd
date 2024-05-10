using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IPartRatingRepository : IEntityRepository<App.DAL.DTO.PartRating>, IPartRatingRepositoryCustom<App.DAL.DTO.PartRating>
{
    
}


public interface IPartRatingRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllPartRatingsIncludingAsync(Guid userId = default, bool noTracking = true);
}