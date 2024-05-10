using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace App.BLL.Services;

public class KeyboardRatingService : BaseEntityService<App.DAL.DTO.KeyboardRating, App.BLL.DTO.KeyboardRating, IKeyboardRatingRepository>,
    IKeyboardRatingService
{
    private readonly IMapper _mapper;
    public KeyboardRatingService(IAppUnitOfWork uoW, IKeyboardRatingRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<KeyboardRating, DTO.KeyboardRating>(mapper))
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<DTO.KeyboardRating>> GetAllRatingsIncludingAsync(Guid userId = default, bool noTracking = true)
    {
        var result = await Repository.GetAllRatingsIncludingAsync();
        return result.Select(rating => _mapper.Map<DTO.KeyboardRating>(rating));
    }
}