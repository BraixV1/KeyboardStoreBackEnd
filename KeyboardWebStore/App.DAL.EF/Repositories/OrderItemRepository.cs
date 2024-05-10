using System.Collections;
using App.Contracts.DAL.Repositories;
using App.Domain;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class OrderItemRepository : BaseEntityRepository<App.Domain.OrderItem, App.DAL.DTO.OrderItem, AppDbContext>,  IOrderItemRepository
{
    private readonly IMapper _mapper;
    public OrderItemRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<OrderItem, DALDTO.OrderItem>(mapper))
    {
        _mapper = mapper;
    }



    public async Task<IEnumerable<DALDTO.OrderItem>> GetAllOrderItemsIncludingAsync(Guid userId = default, bool Notracking = true)
    {
        var found = (await CreateQuery(userId, Notracking)
            .Include(item => item.Keyboard)
            .Include(item => item.Part)
            .Include(item => item.Order).ThenInclude(order => order!.AppUser)
            .ToListAsync());
        var result = found.Select(de => Mapper.Map(de));
        return result != null
            ? _mapper.Map<IEnumerable<App.DAL.DTO.OrderItem>>(result)
            : Enumerable.Empty<App.DAL.DTO.OrderItem>();
    }
}