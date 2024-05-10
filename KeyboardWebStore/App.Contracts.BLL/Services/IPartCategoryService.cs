using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IPartCategoryService : IEntityRepository<App.BLL.DTO.PartCategory>, IPartInBuildRepositoryCustom<App.BLL.DTO.PartCategory>
{
    
}