using App.Contracts.DAL.Repositories;
using App.Domain;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Part = App.DAL.DTO.Part;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class KeyboardRepository : BaseEntityRepository<App.Domain.Keyboard, App.DAL.DTO.Keyboard, AppDbContext>,
    IKeyboardRepository
{
    private readonly IMapper _mapper;
    public KeyboardRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<Keyboard, App.DAL.DTO.Keyboard>(mapper))
    {
        _mapper = mapper;
    }
    

    public async Task<IEnumerable<DTO.Keyboard>> GetAllKeyboardIncludingAsync(Guid userId = default,
        bool noTracking = true)
    {
        var x = (await CreateQuery(userId, noTracking)
            .Include(x => x.KeyboardPartsCollection)
            .ThenInclude(kp => kp.Part)
            .Include(kb => kb.KeyboardRatingsCollection)
            .ThenInclude(rating => rating.User)
            .Include(kb => kb.WarehouseKeyboardsCollection)
            .ThenInclude(part => part.Warehouse)
            .Include(kb => kb.PartCategoriesCollection)
            .ThenInclude(category => category.Category)
            .ToListAsync());
        var result = x.Select(de => Mapper.Map(de));
        return result != null
            ? _mapper.Map<IEnumerable<App.DAL.DTO.Keyboard>>(result)
            : Enumerable.Empty<App.DAL.DTO.Keyboard>();
    }
    
}