using App.Contracts.DAL.Repositories;
using App.Domain;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class OrderRepository : BaseEntityRepository<Order, App.DAL.DTO.Order, AppDbContext>,  IOrderRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public OrderRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<Order, DALDTO.Order>(mapper))
    {
        _context = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DTO.Order>> GetAllOrderIncludingEverythingAsync()
    {
        var found = await _context.Orders
            .Include(order => order.AppUser)
            .Include(order => order.OrderItemsCollection)
            .ThenInclude(item => item.Keyboard)
            .Include(order => order.OrderItemsCollection)
            .ThenInclude(item => item.Part)
            .ToListAsync();

        var result = found.Select(de => Mapper.Map(de));
        return result != null
            ? _mapper.Map<IEnumerable<App.DAL.DTO.Order>>(result)
            : Enumerable.Empty<App.DAL.DTO.Order>();
    }
}