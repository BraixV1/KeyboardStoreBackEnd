using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;
using Category = App.BLL.DTO.Category;

namespace App.BLL.Services;

public class CategoryService : BaseEntityService<App.DAL.DTO.Category, App.BLL.DTO.Category, ICategoryRepository>
,ICategoryService
{
    private readonly IMapper _mapper;

    public CategoryService(IAppUnitOfWork uoW, ICategoryRepository repository, IMapper mapper) : 
        base(uoW, repository, new BllDalMapper<App.DAL.DTO.Category, App.BLL.DTO.Category>(mapper))
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesIncludingPartsAsync(Guid userId = default, bool noTracking = true)
    {
        var result = await Repository.GetAllCategoriesIncludingPartsAsync();
        return result.Select(category => _mapper.Map<DTO.Category>(category));
    }
}