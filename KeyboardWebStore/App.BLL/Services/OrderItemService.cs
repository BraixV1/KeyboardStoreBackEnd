using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class OrderItemService : BaseEntityService<App.DAL.DTO.OrderItem, App.BLL.DTO.OrderItem, IOrderItemRepository>,
    IOrderItemService
{

    private readonly IMapper _mapper;
    
    public OrderItemService(IAppUnitOfWork uoW, IOrderItemRepository repository, IMapper mapper)
        : base(uoW, repository, new BllDalMapper<OrderItem, DTO.OrderItem>(mapper))
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<DTO.OrderItem>> GetAllOrderItemsIncludingAsync(Guid userId = default, bool Notracking = true)
    {
        var found = await Repository.GetAllOrderItemsIncludingAsync();
        return found.Select(Oi => _mapper.Map<App.BLL.DTO.OrderItem>(Oi));
    }
}