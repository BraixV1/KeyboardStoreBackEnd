using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IContestService : IEntityRepository<App.BLL.DTO.Contest>, IContestRepositoryCustom<App.BLL.DTO.Contest>
{
    
}