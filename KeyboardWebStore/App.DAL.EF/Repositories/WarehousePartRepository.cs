using App.Contracts.DAL.Repositories;
using App.Domain;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class WarehousePartRepository : BaseEntityRepository<App.Domain.WarehousePart, DALDTO.WarehousePart, AppDbContext>,  IWarehousePartRepository
{
    private readonly IMapper _mapper;
    public WarehousePartRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<WarehousePart, DALDTO.WarehousePart>(mapper))
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<DALDTO.WarehousePart>> GetAllWarehousePartsIncludingPartsAsync(Guid userId = default, bool noTracking = true)
    {
        var x = await CreateQuery(userId, noTracking)
            .Include(part => part.Part)
            .Include(part => part.Warehouse)
            .Include(part => part.Keyboard)
            .ToListAsync();

        var result = x.Select(de => Mapper.Map(de));
        return result != null
            ? _mapper.Map<IEnumerable<DALDTO.WarehousePart>>(result)
            : Enumerable.Empty<DALDTO.WarehousePart>();
    }
}