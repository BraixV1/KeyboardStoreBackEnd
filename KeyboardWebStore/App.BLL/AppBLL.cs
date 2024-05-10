using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.EF;
using AutoMapper;
using Base.BLL;
using Microsoft.EntityFrameworkCore.Metadata;

namespace App.BLL;

public class AppBll: BaseBll<AppDbContext>, IAppBLL
{
    private readonly IMapper _mapper;
    private readonly IAppUnitOfWork _uow;
    
    public AppBll(IAppUnitOfWork uoW, IMapper mapper) : base(uoW)
    {
        _mapper = mapper;
        _uow = uoW;
    }

    private IContestService? _contests;
    public IContestService Contests => _contests ?? new ContestService(_uow, _uow.Contests, _mapper);

    private IRefreshTokenService? _tokenService;

    public IRefreshTokenService AppRefreshTokens => _tokenService ?? new TokenService(_uow, _uow.RefreshTokens, _mapper);

    private IKeyboardService? _keyboards;

    public IKeyboardService Keyboards => _keyboards ?? new KeyboardService(_uow, _uow.Keyboards, _mapper);

    private IKeyboardRatingService? _keyboardRatings;

    public IKeyboardRatingService KeyboardRatings =>
        _keyboardRatings ?? new KeyboardRatingService(_uow, _uow.KeyboardRatings, _mapper);

    private IKeyboardBuildService? _keyboardBuilds;

    public IKeyboardBuildService KeyboardBuilds =>
        _keyboardBuilds ?? new KeyboardBuildService(_uow, _uow.KeyboardBuilds, _mapper);
    
    private IPartService? _parts;

    public IPartService Parts => _parts ?? new PartService(_uow, _uow.Parts, _mapper);

    private IPartCategoryService? _partCategories;

    public IPartCategoryService PartCategories =>
        _partCategories ?? new PartCategoryService(_uow, _uow.PartCategories, _mapper);

    private IPartRatingService? _partRatings;

    public IPartRatingService PartRatings => _partRatings ?? new PartRatingService(_uow, _uow.PartRatings, _mapper);

    private IPartInBuildService? _partInBuilds;

    public IPartInBuildService PartInBuilds =>
        _partInBuilds ?? new PartInBuildService(_uow, _uow.PartInBuilds, _mapper);

    private IOrderService? _orders;

    public IOrderService Orders => _orders ?? new OrderService(_uow, _uow.Orders, _mapper);

    private IOrderItemService? _orderItems;

    public IOrderItemService OrderItems => _orderItems ?? new OrderItemService(_uow, _uow.OrderItems, _mapper);
    
    private IKeyboardPartService? _keyboardParts;

    public IKeyboardPartService KeyboardParts =>
        _keyboardParts ?? new KeyboardPartService(_uow, _uow.KeyboardParts, _mapper);

    private ICategoryService? _categories;

    public ICategoryService Categories => _categories ?? new CategoryService(_uow, _uow.Categories, _mapper);

    private IWarehouseService? _warehouses;

    public IWarehouseService Warehouses => _warehouses ?? new WarehouseService(_uow, _uow.Warehouses, _mapper);

    private IWarehousePartService? _warehouseParts;

    public IWarehousePartService WarehouseParts =>
        _warehouseParts ?? new WarehousePartService(_uow, _uow.WareHouseParts, _mapper);


}