using App.Contracts.DAL.Repositories;
using App.Domain;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class PartInRatingRepository : BaseEntityRepository<App.Domain.PartRating, App.DAL.DTO.PartRating, AppDbContext>,  IPartRatingRepository
{
    private readonly IMapper _mapper;
    public PartInRatingRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<PartRating, DALDTO.PartRating>(mapper))
    {
        _mapper = mapper;
    }

    public async Task<IEnumerable<DALDTO.PartRating>> GetAllPartRatingsIncludingAsync(Guid userId = default, bool noTracking = true)
    {
        var x = await CreateQuery(userId, noTracking)
            .Include(rating => rating.User)
            .Include(rating => rating.Part)
            .ToListAsync();

        var result = x.Select(de => Mapper.Map(de));


        return result != null
            ? _mapper.Map<IEnumerable<App.DAL.DTO.PartRating>>(result)
            : Enumerable.Empty<DALDTO.PartRating>();


    }
}