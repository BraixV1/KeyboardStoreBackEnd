using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.BLL.Services;

public class WarehousePartService : BaseEntityService<App.DAL.DTO.WarehousePart, App.BLL.DTO.WarehousePart, IWarehousePartRepository>,
    IWarehousePartService
{
    private readonly IMapper _mapper;

    public WarehousePartService(IAppUnitOfWork uoW, IWarehousePartRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<WarehousePart, DTO.WarehousePart>(mapper))
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<DTO.WarehousePart>> GetAllWarehousePartsIncludingPartsAsync(Guid userId = default, bool noTracking = true)
    {
        var result = await Repository.GetAllWarehousePartsIncludingPartsAsync();
        return result.Select(part => _mapper.Map<DTO.WarehousePart>(part));
    }
}