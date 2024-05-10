using System.Linq.Expressions;
using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IKeyboardRatingRepository : IEntityRepository<App.DAL.DTO.KeyboardRating>, IKeyboardRatingRepositoryCustom<App.DAL.DTO.KeyboardRating>
{
    
}

public interface IKeyboardRatingRepositoryCustom<TEntity>{
Task<IEnumerable<TEntity>> GetAllRatingsIncludingAsync(Guid userId = default, bool noTracking = true);
}
