using System.Linq.Expressions;
using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IKeyboardPartRepository : IEntityRepository<App.DAL.DTO.KeyboardPart>, IKeyboardPartCustom<App.DAL.DTO.KeyboardPart>
{
}

public interface IKeyboardPartCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllKeyboardPartsIncludingAsync(Guid userId = default, bool noTracking = true);
}