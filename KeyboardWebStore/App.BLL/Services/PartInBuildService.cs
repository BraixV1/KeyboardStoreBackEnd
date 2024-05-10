using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.BLL.Services;

public class PartInBuildService : BaseEntityService<PartInBuild, App.BLL.DTO.PartInBuild, IPartInBuildRepository>, IPartInBuildService
{
    private readonly IMapper _mapper;

    public PartInBuildService(IAppUnitOfWork uoW, IPartInBuildRepository repository,
        IMapper mapper) : base(uoW, repository, new BllDalMapper<PartInBuild, DTO.PartInBuild>(mapper))
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<DTO.PartInBuild>> GetAllPartInBuildsIncludingAsync(Guid userId = default, bool noTracking = true)
    {
        var result = await Repository.GetAllPartInBuildsIncludingAsync();
        return result.Select(build => _mapper.Map<DTO.PartInBuild>(build));
    }
}