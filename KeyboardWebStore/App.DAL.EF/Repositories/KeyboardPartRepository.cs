using System.Linq.Expressions;
using App.Contracts.DAL.Repositories;
using App.Domain;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class KeyboardPartRepository : BaseEntityRepository<App.Domain.KeyboardPart, App.DAL.DTO.KeyboardPart, AppDbContext>,  IKeyboardPartRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public KeyboardPartRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<KeyboardPart, App.DAL.DTO.KeyboardPart>(mapper))
    {
        _context = dbContext;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<DTO.KeyboardPart>> GetAllKeyboardPartsIncludingAsync(Guid userId = default,
        bool noTracking = true)
    {
        var x = await _context.KeyboardParts
            .Include(part => part.Keyboard)
            .Include(part => part.Part)
            .ToListAsync();
        var result = x.Select(de => Mapper.Map(de));
        return result != null ? _mapper.Map<IEnumerable<App.DAL.DTO.KeyboardPart>>(result) : Enumerable.Empty<App.DAL.DTO.KeyboardPart>();
    }
    
}