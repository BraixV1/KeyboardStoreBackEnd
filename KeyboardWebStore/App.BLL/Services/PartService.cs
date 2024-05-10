using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;
using Part = App.BLL.DTO.Part;

namespace App.BLL.Services;

public class PartService : BaseEntityService<App.DAL.DTO.Part, App.BLL.DTO.Part, IPartRepository>, IPartService
{
    private readonly IMapper _mapper;

    public PartService(IAppUnitOfWork uoW, IPartRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.Part, App.BLL.DTO.Part>(mapper))
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<Part>> GetAllPartsIncludingAsync(Guid userId = default, bool noTracking = true)
    {
        var parts = await Repository.GetAllPartsIncludingAsync();
        return parts.Select(p => _mapper.Map<Part>(p));
    }
}