using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IKeyboardRatingService : IEntityRepository<App.BLL.DTO.KeyboardRating>, IKeyboardRatingRepositoryCustom<App.BLL.DTO.KeyboardRating>
{
    
}