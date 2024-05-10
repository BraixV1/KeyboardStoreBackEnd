using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IKeyboardBuildService : IEntityRepository<App.BLL.DTO.KeyboardBuild>, IKeyboardBuildRepositoryCustom<App.BLL.DTO.KeyboardBuild>
{
    
}