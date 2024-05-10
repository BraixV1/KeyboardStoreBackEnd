using App.Contracts.DAL.Repositories;
using App.Domain;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class KeyboardBuildRepository : BaseEntityRepository<KeyboardBuild, App.DAL.DTO.KeyboardBuild, AppDbContext>,  IKeyboardBuildRepository
{
    private readonly IMapper _mapper;

    public KeyboardBuildRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<KeyboardBuild, DALDTO.KeyboardBuild>(mapper))
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<DALDTO.KeyboardBuild>> GetAllKeyboardBuildsIncludingAsync(Guid userId = default, bool noTracking = true)
    {
        var x = await CreateQuery(userId, noTracking)
            .Include(build => build.AppUser)
            .Include(build => build.BuildParts)
            .ThenInclude(build => build.Part)
            .ToListAsync();

        var result = x.Select(de => Mapper.Map(de));

        return result != null
            ? _mapper.Map<IEnumerable<DALDTO.KeyboardBuild>>(result)
            : Enumerable.Empty<DALDTO.KeyboardBuild>();
    }
}