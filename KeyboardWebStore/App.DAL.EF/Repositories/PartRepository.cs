using App.Contracts.DAL.Repositories;
using App.Domain;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class PartRepository : BaseEntityRepository<App.Domain.Part, App.DAL.DTO.Part, AppDbContext>,  IPartRepository
{
    private readonly IMapper _mapper;
    public PartRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<Part, App.DAL.DTO.Part>(mapper))
    {
        _mapper = mapper;
    }


    public async Task<IEnumerable<DTO.Part>> GetAllPartsIncludingAsync(Guid userId = default, bool noTracking = true)
    {
        var x = (await CreateQuery(userId, noTracking)
            .Include(p => p.PartCategoriesCollection)
            .ThenInclude(category => category.Category)
            .Include(p => p.PartCommentsCollection)
            .ThenInclude(rating => rating.User)
            .Include(p => p.WarehousePartsCollection)
            .ThenInclude(part => part.Warehouse)
            .ToListAsync());

        var result = x.Select(de => Mapper.Map(de));
        return result != null
            ? _mapper.Map<IEnumerable<App.DAL.DTO.Part>>(result)
            : Enumerable.Empty<App.DAL.DTO.Part>();
    }   
}