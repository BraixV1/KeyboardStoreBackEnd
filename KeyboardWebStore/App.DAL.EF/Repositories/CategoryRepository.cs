using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;



public class CategoryRepository : BaseEntityRepository<APPDomain.Category, DALDTO.Category, AppDbContext>,  ICategoryRepository
{
    private readonly IMapper _mapper;
    public CategoryRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<APPDomain.Category, DALDTO.Category>(mapper))
    {
        _mapper = mapper;
    }



    public async Task<IEnumerable<DALDTO.Category>> GetAllCategoriesIncludingPartsAsync(Guid userId = default, bool noTracking = true)
    {

        var x = await CreateQuery()
            .Include(category => category.PartCategories)
            .ThenInclude(category => category.Part)
            .Include(category => category.PartCategories)
            .ThenInclude(category => category.Keyboard)
            .ToListAsync();


        var result = x.Select(de => Mapper.Map(de));


        return result != null
            ? _mapper.Map<IEnumerable<App.DAL.DTO.Category>>(result)
            : Enumerable.Empty<App.DAL.DTO.Category>();



    }
}