using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class KeyboardService : BaseEntityService<App.DAL.DTO.Keyboard, App.BLL.DTO.Keyboard, IKeyboardRepository>,
    IKeyboardService
{
    
    private readonly IMapper _mapper;


    public KeyboardService(IAppUnitOfWork uoW, IKeyboardRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.Keyboard, App.BLL.DTO.Keyboard>(mapper))
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    

    public async Task<IEnumerable<Keyboard>> GetAllKeyboardIncludingAsync(Guid userId = default, bool noTracking = true)
    {
        var keyboards = await Repository.GetAllKeyboardIncludingAsync();
        return keyboards.Select(kb => _mapper.Map<Keyboard>(kb));
    } 
}