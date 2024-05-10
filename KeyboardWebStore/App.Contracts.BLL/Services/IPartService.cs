using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IPartService : IEntityRepository<App.BLL.DTO.Part>, IPartRepositoryCustom<App.BLL.DTO.Part>
{
    
}