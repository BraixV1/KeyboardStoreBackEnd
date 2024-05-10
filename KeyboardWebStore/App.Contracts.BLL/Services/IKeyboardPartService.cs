using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IKeyboardPartService : IEntityRepository<App.BLL.DTO.KeyboardPart>, IKeyboardPartCustom<App.BLL.DTO.KeyboardPart>
{
    
}