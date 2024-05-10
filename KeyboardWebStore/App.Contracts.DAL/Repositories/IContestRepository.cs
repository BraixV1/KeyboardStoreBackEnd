using System.Linq.Expressions;
using App.Domain;
using Base.Contracts.DAL;
using DALDTO = App.DAL.DTO;

namespace App.Contracts.DAL.Repositories;


public interface IContestRepository : IEntityRepository<DALDTO.Contest>, IContestRepositoryCustom<DALDTO.Contest>
{
    // define your DAL only custom methods here
}

public interface IContestRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllSortedAsync(Guid userId);
}