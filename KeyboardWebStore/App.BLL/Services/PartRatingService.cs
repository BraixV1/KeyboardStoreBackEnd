using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.BLL.Services;

public class PartRatingService : BaseEntityService<App.DAL.DTO.PartRating, App.BLL.DTO.PartRating, IPartRatingRepository>,
    IPartRatingService
{
    private readonly IMapper _mapper;

    public PartRatingService(IAppUnitOfWork uoW, IPartRatingRepository repository, IMapper mapper) : base(uoW, repository, new BllDalMapper<PartRating, DTO.PartRating>(mapper))
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<DTO.PartRating>> GetAllPartInBuildsIncludingAsync(Guid userId = default, bool noTracking = true)
    {
        var result = await Repository.GetAllPartRatingsIncludingAsync();
        return result.Select(rating => _mapper.Map<DTO.PartRating>(rating));
    }
}