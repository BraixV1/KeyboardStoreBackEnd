using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.EF.Repositories;
using App.Domain.Identity;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : BaseUnitOfWork<AppDbContext>, IAppUnitOfWork
{
    private readonly IMapper _mapper;
    public AppUOW(AppDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    private IKeyboardRatingRepository? _keyboardRatings;
    
    private IContestRepository? _contests;
    
    private ICategoryRepository? _categories;
    
    private IKeyboardBuildRepository? _keyboardBuilds;
    
    private IKeyboardPartRepository? _keyboardParts;
    
    private IKeyboardRepository? _keyboards;
    
    private IOrderItemRepository? _orderItems;
    
    private IOrderRepository? _orders;
    
    private IPartCategoryRepository? _partCategories;
    
    private IPartInBuildRepository? _partInBuilds;
    
    private IPartRatingRepository? _partInRating;
    
    private IPartRepository? _parts;
    
    private IWarehousePartRepository? _warehouseParts;
    
    private IWarehouseRepository? _warehouses;
    
    private IRefreshTokenRepository? _refreshTokens;

    private IEntityRepository<AppUser>? _users;

    
    
    public IContestRepository Contests => _contests ?? new ContestRepository(UowDbContext, _mapper);

    public ICategoryRepository Categories => _categories ?? new CategoryRepository(UowDbContext, _mapper);

    public IKeyboardRepository Keyboards => _keyboards ?? new KeyboardRepository(UowDbContext, _mapper);

    public IKeyboardBuildRepository KeyboardBuilds => _keyboardBuilds ?? new KeyboardBuildRepository(UowDbContext, _mapper);

    public IKeyboardPartRepository KeyboardParts => _keyboardParts ?? new KeyboardPartRepository(UowDbContext, _mapper);

    public IOrderRepository Orders => _orders ?? new OrderRepository(UowDbContext, _mapper);

    public IOrderItemRepository OrderItems => _orderItems ?? new OrderItemRepository(UowDbContext, _mapper);

    public IPartCategoryRepository PartCategories => _partCategories ?? new PartCategoryRepository(UowDbContext, _mapper);

    public IPartInBuildRepository PartInBuilds => _partInBuilds ?? new PartInBuildRepository(UowDbContext, _mapper);

    public IPartRatingRepository PartRatings => _partInRating ?? new PartInRatingRepository(UowDbContext, _mapper);

    public IPartRepository Parts => _parts ?? new PartRepository(UowDbContext, _mapper);

    public IWarehousePartRepository WareHouseParts => _warehouseParts ?? new WarehousePartRepository(UowDbContext, _mapper);

    public IWarehouseRepository Warehouses => _warehouses ?? new WarehouseRepository(UowDbContext, _mapper);

    public IKeyboardRatingRepository KeyboardRatings => _keyboardRatings ?? new KeyboardRatingRepository(UowDbContext, _mapper);

    public IEntityRepository<AppUser> Users => _users ??
                                               new BaseEntityRepository<AppUser, AppUser, AppDbContext>(UowDbContext,
                                                   new DalDomainMapper<AppUser, AppUser>(_mapper));

    public IRefreshTokenRepository RefreshTokens => _refreshTokens ?? new RefreshTokenRepository(UowDbContext, _mapper);
}