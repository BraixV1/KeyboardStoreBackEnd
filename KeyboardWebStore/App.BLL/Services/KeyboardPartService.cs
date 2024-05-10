using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;
using KeyboardPart = App.BLL.DTO.KeyboardPart;

namespace App.BLL.Services;

public class KeyboardPartService : BaseEntityService<App.DAL.DTO.KeyboardPart, App.BLL.DTO.KeyboardPart, IKeyboardPartRepository>, IKeyboardPartService
{
    private readonly IMapper _mapper;
    public KeyboardPartService(IAppUnitOfWork uoW, IKeyboardPartRepository repository, IMapper mapper)
        : base(uoW, repository, new BllDalMapper<App.DAL.DTO.KeyboardPart, App.BLL.DTO.KeyboardPart>(mapper))
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<KeyboardPart>> GetAllKeyboardPartsIncludingAsync(Guid userId = default, bool noTracking = true)
    {
        var keyboardParts = await Repository.GetAllKeyboardPartsIncludingAsync();
        return keyboardParts.Select(kb => _mapper.Map<KeyboardPart>(kb));
        
    }
}