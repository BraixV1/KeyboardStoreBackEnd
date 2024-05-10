using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.BLL.Services;

public class WarehouseService : BaseEntityService<App.DAL.DTO.Warehouse, App.BLL.DTO.Warehouse, IWarehouseRepository>,
    IWarehouseService
{
    private readonly IMapper _mapper;

    public WarehouseService(IAppUnitOfWork uoW, IWarehouseRepository repository, IMapper mapper) : base(uoW, repository,
        new BllDalMapper<Warehouse, DTO.Warehouse>(mapper))
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<DTO.Warehouse>> GetAllWarehousesIncludingAsync(Guid userId = default, bool noTracking = true)
    {
        var result = await Repository.GetAllWarehousesIncludingAsync();
        return result.Select(warehouse => _mapper.Map<DTO.Warehouse>(warehouse));
    }
}