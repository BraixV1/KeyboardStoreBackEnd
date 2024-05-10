using App.Domain;
using Base.Contracts.DAL;
using Keyboard = App.DAL.DTO.Keyboard;

namespace App.Contracts.DAL.Repositories;

public interface IKeyboardRepository : IEntityRepository<App.DAL.DTO.Keyboard>, IKeyboardRepositoryCustom<Keyboard>
{
}

public interface IKeyboardRepositoryCustom<TEntity>
{
    
    Task<IEnumerable<TEntity>> GetAllKeyboardIncludingAsync(Guid userId = default, bool noTracking = true);
}
