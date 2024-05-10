using System.Collections;
using App.Domain;
using System.Linq.Expressions;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class KeyboardRatingRepository : BaseEntityRepository<KeyboardRating, App.DAL.DTO.KeyboardRating, AppDbContext>,  IKeyboardRatingRepository
{
    private readonly IMapper _mapper;
    public KeyboardRatingRepository(AppDbContext dbContext, IMapper mapper) : 
    base(dbContext, new DalDomainMapper<KeyboardRating, DALDTO.KeyboardRating>(mapper))
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<DALDTO.KeyboardRating>> GetAllRatingsIncludingAsync(Guid userId = default, bool noTracking = true)
    {
        var result = await CreateQuery()
            .Include(rating => rating.Keyboard)
            .Include(rating => rating.User)
            .ToListAsync();

        var mapped = result.Select(de => Mapper.Map(de));
        return mapped != null
            ? _mapper.Map<IEnumerable<App.DAL.DTO.KeyboardRating>>(result) : Enumerable.Empty<App.DAL.DTO.KeyboardRating>();
    }
}