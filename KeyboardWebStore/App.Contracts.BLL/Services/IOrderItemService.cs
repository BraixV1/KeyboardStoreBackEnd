using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IOrderItemService : IEntityRepository<App.BLL.DTO.OrderItem>, IOrderITemRepositoryCustom<App.BLL.DTO.OrderItem>
{
    
}