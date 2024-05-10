using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class WarehouseRepository : BaseEntityRepository<APPDomain.Warehouse, DALDTO.Warehouse, AppDbContext>,  IWarehouseRepository
{

    private readonly IMapper _mapper;
    public WarehouseRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<APPDomain.Warehouse, DALDTO.Warehouse>(mapper))

    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<DALDTO.Warehouse>> GetAllWarehousesIncludingAsync(Guid userId = default, bool noTracking = true)
    {
        var x = await CreateQuery(userId, noTracking)
            .Include(warehouse => warehouse.WarehousePartsCollection)
            .ThenInclude(part => part.Part)
            .Include(w => w.WarehousePartsCollection)
            .ThenInclude(part => part.Keyboard)
            .ToListAsync();
        var result = x.Select(de => Mapper.Map(de));

        return result != null
            ? _mapper.Map<IEnumerable<DALDTO.Warehouse>>(result)
            : Enumerable.Empty<DALDTO.Warehouse>();

    }
}