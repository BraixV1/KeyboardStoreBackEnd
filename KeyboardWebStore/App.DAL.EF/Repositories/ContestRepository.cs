using App.Contracts.DAL.Repositories;
using AutoMapper;
using APPDomain = App.Domain;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ContestRepository : BaseEntityRepository<APPDomain.Contest, DALDTO.Contest, AppDbContext>,
    IContestRepository
{
    public ContestRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<APPDomain.Contest, DALDTO.Contest>(mapper))
    {
    }

    // implement your custom methods here
    public async Task<IEnumerable<DALDTO.Contest>> GetAllSortedAsync(Guid userId)
    {
        var query = CreateQuery(userId);
        query = query.OrderBy(c => c.ContestName);
        var res = await query.ToListAsync();
        return res.Select(e => Mapper.Map(e));
    }
}