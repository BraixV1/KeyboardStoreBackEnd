using AutoMapper;

namespace WebApp.Helpers;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<App.BLL.DTO.Contest, App.DTO.v1_0.Contest>().ReverseMap();

        CreateMap<App.BLL.DTO.AppRefreshToken, App.DTO.v1_0.Identity.AppRefreshToken>().ReverseMap();

        CreateMap<App.BLL.DTO.Keyboard, App.DTO.v1_0.Keyboard>().ReverseMap();
        
        CreateMap<App.BLL.DTO.Keyboard, App.DTO.v1_0.KeyboardWithoutCollections>().ReverseMap();

        CreateMap<App.BLL.DTO.Part, App.DTO.v1_0.Part.Part>().ReverseMap();
        
        CreateMap<App.BLL.DTO.Part, App.DTO.v1_0.Part.PartWithoutCollections>().ReverseMap();
        
        CreateMap<App.BLL.DTO.Part, App.DTO.v1_0.Part.partWithoutWarehouse>().ReverseMap();
        
        CreateMap<App.BLL.DTO.KeyboardPart, App.DTO.v1_0.KeyboardPartWithKeyboard>().ReverseMap();
        
        CreateMap<App.BLL.DTO.KeyboardPart, App.DTO.v1_0.KeyboardPartWithPart>().ReverseMap();

        CreateMap<App.BLL.DTO.KeyboardPart, App.DTO.v1_0.KeyboardPart>().ReverseMap();
        
        CreateMap<App.BLL.DTO.KeyboardRating, App.DTO.v1_0.KeyboardRating>().ReverseMap();
        
        CreateMap<App.BLL.DTO.KeyboardRating, App.DTO.v1_0.KeyboardRatingForKeyboard>().ReverseMap();

        CreateMap<App.BLL.DTO.KeyboardBuild, App.DTO.v1_0.KeyboardBuild>().ReverseMap();
        
        CreateMap<App.BLL.DTO.PartRating, App.DTO.v1_0.PartRating>().ReverseMap();
        
        CreateMap<App.BLL.DTO.PartRating, App.DTO.v1_0.PartRatingForPart>().ReverseMap();
        
        CreateMap<App.BLL.DTO.PartInBuild, App.DTO.v1_0.PartInBuild>().ReverseMap();
        
        CreateMap<App.BLL.DTO.PartCategory, App.DTO.v1_0.PartCategory>().ReverseMap();
        
        CreateMap<App.BLL.DTO.Order, App.DTO.v1_0.Order>().ReverseMap();
        
        CreateMap<App.BLL.DTO.OrderItem, App.DTO.v1_0.OrderItem>().ReverseMap();
        
        CreateMap<App.BLL.DTO.OrderItem, App.DTO.v1_0.OrderItemForOrder>().ReverseMap();
        
        CreateMap<App.BLL.DTO.Warehouse, App.DTO.v1_0.Warehouse>().ReverseMap();
        
        CreateMap<App.BLL.DTO.Warehouse, App.DTO.v1_0.WarehouseWithoutCollection>().ReverseMap();
        
        CreateMap<App.BLL.DTO.WarehousePart,App.DTO.v1_0.WarehousePart>().ReverseMap();
        
        CreateMap<App.BLL.DTO.WarehousePart, App.DTO.v1_0.WareHousePartForWarehouse>().ReverseMap();
        
        CreateMap<App.BLL.DTO.WarehousePart, App.DTO.v1_0.WarehousePartWithPart>().ReverseMap();
        
        CreateMap<App.BLL.DTO.WarehousePart, App.DTO.v1_0.WarehousePartWithKeyboard>().ReverseMap();
        
        CreateMap<App.BLL.DTO.PartCategory, App.DTO.v1_0.PartCategoryWithPart>().ReverseMap();
        
        CreateMap<App.BLL.DTO.Category, App.DTO.v1_0.Category>().ReverseMap();
        
        CreateMap<App.BLL.DTO.Category, App.DTO.v1_0.CategoryWithoutCollections>().ReverseMap();
        
        CreateMap<App.BLL.DTO.PartCategory, App.DTO.v1_0.PartCategoryWithKeyboard>().ReverseMap();
        
        CreateMap<App.BLL.DTO.PartCategory, App.DTO.v1_0.PartCategoryForCategory>().ReverseMap();
        
        CreateMap<App.BLL.DTO.PartCategory, App.DTO.v1_0.PartCategory>().ReverseMap();

        CreateMap<App.BLL.DTO.PartInBuild, App.DTO.v1_0.PartInBuildWithPart>().ReverseMap();
        
        CreateMap<App.BLL.DTO.PartInBuild, App.DTO.v1_0.PartInBuildWithPart>().ReverseMap();
        
        CreateMap<App.BLL.DTO.Identity.AppUser, App.DTO.v1_0.Identity.AppUserBll>().ReverseMap();

        CreateMap<App.BLL.DTO.HelperEnums.OrderStatus, App.DTO.v1_0.HelperEnums.OrderStatus>().ReverseMap();
    }
}