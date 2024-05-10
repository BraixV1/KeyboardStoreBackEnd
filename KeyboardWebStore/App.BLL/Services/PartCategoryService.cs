using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class PartCategoryService : BaseEntityService<PartCategory, App.BLL.DTO.PartCategory, IPartCategoryRepository>,
    IPartCategoryService
{
    private readonly IMapper _mapper;

    public PartCategoryService(IAppUnitOfWork uoW, IPartCategoryRepository repository, IMapper mapper) : base(uoW
        , repository, new BllDalMapper<PartCategory, DTO.PartCategory>(mapper))
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<DTO.PartCategory>> GetAllPartInBuildsIncludingAsync(Guid userId = default, bool noTracking = true)
    {
        var result = await Repository.GetAllCategoriesIncludingPartsAsync();
        return result.Select(category => _mapper.Map<DTO.PartCategory>(category));
    }
}