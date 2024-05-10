using App.Contracts.DAL.Repositories;
using App.Domain;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class PartCategoryRepository : BaseEntityRepository<App.Domain.PartCategory, DALDTO.PartCategory, AppDbContext>,  IPartCategoryRepository
{
    private readonly IMapper _mapper;
    public PartCategoryRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<PartCategory, DALDTO.PartCategory>(mapper))
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<DALDTO.PartCategory>> GetAllCategoriesIncludingPartsAsync(Guid userId = default, bool noTracking = true)
    {
        var x = await CreateQuery(userId, noTracking)
            .Include(category => category.Category)
            .Include(category => category.Part)
            .Include(category => category.Keyboard)
            .ToListAsync();

        var result = x.Select(de => Mapper.Map(de));
        return result != null
            ? _mapper.Map<IEnumerable<DALDTO.PartCategory>>(result)
            : Enumerable.Empty<DALDTO.PartCategory>();

    }
}