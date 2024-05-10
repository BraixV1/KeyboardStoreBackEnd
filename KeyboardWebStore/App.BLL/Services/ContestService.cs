using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using App.DAL.EF;
using App.DAL.EF.Repositories;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;
using Contest = App.BLL.DTO.Contest;

namespace App.BLL.Services;

public class ContestService :
    BaseEntityService<App.DAL.DTO.Contest, App.BLL.DTO.Contest, IContestRepository>,
    IContestService
{
    public ContestService(IAppUnitOfWork uoW, IContestRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.Contest, App.BLL.DTO.Contest>(mapper))
    {
    }

    public async Task<IEnumerable<Contest>> GetAllSortedAsync(Guid userId)
    {
        return (await Repository.GetAllSortedAsync(userId)).Select(e => Mapper.Map(e));
    }
}