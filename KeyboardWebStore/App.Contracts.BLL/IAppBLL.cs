using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBLL : Base.Contracts.BLL.IBll
{
    
    IRefreshTokenService  AppRefreshTokens { get; }
    
    IKeyboardService Keyboards { get; }
    
    IKeyboardRatingService KeyboardRatings { get; }
    
    IKeyboardBuildService KeyboardBuilds { get; }
    
    IKeyboardPartService KeyboardParts { get; }
    
    IPartService Parts { get; }
    
    IPartCategoryService PartCategories { get; }
    
    IPartRatingService PartRatings { get; }
    
    ICategoryService Categories { get; }
    
    IPartInBuildService PartInBuilds { get; }
    
    IOrderService Orders { get; }
    
    IOrderItemService OrderItems { get; }
    
    IWarehouseService Warehouses { get; }
    
    IWarehousePartService WarehouseParts { get; }
}