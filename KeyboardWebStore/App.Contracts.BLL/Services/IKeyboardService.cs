using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IKeyboardService : IEntityRepository<App.BLL.DTO.Keyboard>, IKeyboardRepositoryCustom<App.BLL.DTO.Keyboard>
{
    
}