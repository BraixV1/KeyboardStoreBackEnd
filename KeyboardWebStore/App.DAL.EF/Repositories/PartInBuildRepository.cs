using App.Contracts.DAL.Repositories;
using App.Domain;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class PartInBuildRepository : BaseEntityRepository<App.Domain.PartInBuild, DALDTO.PartInBuild, AppDbContext>,  IPartInBuildRepository
{
    private readonly IMapper _mapper;
    public PartInBuildRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<PartInBuild, DALDTO.PartInBuild>(mapper))
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<DALDTO.PartInBuild>> GetAllPartInBuildsIncludingAsync(Guid userId = default, bool noTracking = true)
    {
        var x = await CreateQuery(userId, noTracking)
            .Include(build => build.KeyboardBuild)
            .Include(build => build.Part)
            .ToListAsync();
        var result = x.Select(de => Mapper.Map(de));

        return result != null ? _mapper.Map<IEnumerable<DALDTO.PartInBuild>>(result) : Enumerable.Empty<DALDTO.PartInBuild>();
    }
}