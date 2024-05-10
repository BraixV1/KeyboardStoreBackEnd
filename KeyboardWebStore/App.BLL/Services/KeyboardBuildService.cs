using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.BLL.Services;

public class KeyboardBuildService : BaseEntityService<App.DAL.DTO.KeyboardBuild, App.BLL.DTO.KeyboardBuild, IKeyboardBuildRepository>,
    IKeyboardBuildService
{
    private readonly IMapper _mapper;

    public KeyboardBuildService(IAppUnitOfWork uoW, IKeyboardBuildRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<KeyboardBuild, DTO.KeyboardBuild>(mapper))
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<DTO.KeyboardBuild>> GetAllKeyboardBuildsIncludingAsync(Guid userId = default, bool noTracking = true)
    {
        var result = await Repository.GetAllKeyboardBuildsIncludingAsync();
        return result.Select(build => _mapper.Map<DTO.KeyboardBuild>(build));
    }
}