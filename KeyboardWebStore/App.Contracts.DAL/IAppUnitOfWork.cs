using App.Contracts.DAL.Repositories;
using App.Domain.Identity;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    // list your repos here

    IContestRepository Contests { get; }

    ICategoryRepository Categories { get; }

    IKeyboardRepository Keyboards { get; }
    
    IKeyboardRatingRepository KeyboardRatings { get; }

    IKeyboardBuildRepository KeyboardBuilds { get; }

    IKeyboardPartRepository KeyboardParts { get; }

    IOrderRepository Orders { get; }

    IOrderItemRepository OrderItems { get; }

    IPartCategoryRepository PartCategories { get; }

    IPartInBuildRepository PartInBuilds { get; }

    IPartRatingRepository PartRatings { get; }

    IPartRepository Parts { get; }

    IWarehousePartRepository WareHouseParts { get; }

    IWarehouseRepository Warehouses { get; }
    
    IEntityRepository<AppUser> Users { get; }
    
    IRefreshTokenRepository RefreshTokens { get; }
}