using AutoMapper;

namespace App.BLL;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<App.DAL.DTO.Contest, App.BLL.DTO.Contest>().ReverseMap();

        CreateMap<App.DAL.DTO.AppRefreshToken, App.BLL.DTO.AppRefreshToken>().ReverseMap();

        CreateMap<App.DAL.DTO.Keyboard, App.BLL.DTO.Keyboard>().ReverseMap();
        
        CreateMap<App.DAL.DTO.KeyboardWithoutCollection, App.BLL.DTO.Keyboard>().ReverseMap();

        CreateMap<App.DAL.DTO.Part, App.BLL.DTO.Part>().ReverseMap();
        
        CreateMap<App.DAL.DTO.PartWithoutCollection, App.BLL.DTO.Part>().ReverseMap();
        
        CreateMap<App.DAL.DTO.PartWithoutWarehouse, App.BLL.DTO.Part>().ReverseMap();
        
        CreateMap<App.DAL.DTO.KeyboardPart, App.BLL.DTO.KeyboardPart>().ReverseMap();

        CreateMap<App.DAL.DTO.HelperEnums.OrderStatus, App.BLL.DTO.HelperEnums.OrderStatus>();
        
        CreateMap<App.DAL.DTO.KeyboardPartWithPart, App.BLL.DTO.KeyboardPart>().ReverseMap();
        
        CreateMap<App.DAL.DTO.KeyboardPartWithKeyboard, App.BLL.DTO.KeyboardPart>().ReverseMap();
        
        CreateMap<App.DAL.DTO.KeyboardRating, App.BLL.DTO.KeyboardRating>().ReverseMap();
        
        CreateMap<App.DAL.DTO.KeyboardRatingForKeyboard, App.BLL.DTO.KeyboardRating>().ReverseMap();

        CreateMap<App.DAL.DTO.KeyboardBuild, App.BLL.DTO.KeyboardBuild>().ReverseMap();
        
        CreateMap<App.DAL.DTO.Part, App.BLL.DTO.Part>();
        
        CreateMap<App.DAL.DTO.PartRating, App.BLL.DTO.PartRating>().ReverseMap();
        
        CreateMap<App.DAL.DTO.PartRatingForPart, App.BLL.DTO.PartRating>().ReverseMap();
        
        CreateMap<App.DAL.DTO.PartInBuild, App.BLL.DTO.PartInBuild>().ReverseMap();
        
        CreateMap<App.DAL.DTO.PartInBuildWithBuild, App.BLL.DTO.PartInBuild>().ReverseMap();
        
        CreateMap<App.DAL.DTO.PartInBuildWithPart, App.BLL.DTO.PartInBuild>().ReverseMap();
        
        CreateMap<App.DAL.DTO.PartCategory, App.BLL.DTO.PartCategory>().ReverseMap();
        
        CreateMap<App.DAL.DTO.Category, App.BLL.DTO.Category>().ReverseMap();
        
        CreateMap<App.DAL.DTO.CategoryWithoutCollection, App.BLL.DTO.Category>().ReverseMap();
        
        CreateMap<App.DAL.DTO.PartCategoryForCategory, App.BLL.DTO.PartCategory>().ReverseMap();
        
        CreateMap<App.DAL.DTO.PartCategoryWithPart, App.BLL.DTO.PartCategory>().ReverseMap();
        
        CreateMap<App.DAL.DTO.PartCategoryWithKeyboard, App.BLL.DTO.PartCategory>().ReverseMap();
        
        CreateMap<App.DAL.DTO.Order, App.BLL.DTO.Order>().ReverseMap();
        
        CreateMap<App.DAL.DTO.OrderItem, App.BLL.DTO.OrderItem>().ReverseMap();
        
        CreateMap<App.DAL.DTO.OrderItemForOrder, App.BLL.DTO.OrderItem>().ReverseMap();
        
        CreateMap<App.DAL.DTO.Warehouse, App.BLL.DTO.Warehouse>().ReverseMap();
        
        CreateMap<App.DAL.DTO.WarehouseWithoutCollection, App.BLL.DTO.Warehouse>().ReverseMap();
        
        CreateMap<App.DAL.DTO.WarehousePart, App.BLL.DTO.WarehousePart>().ReverseMap();
        
        CreateMap<App.DAL.DTO.WarehousePartWithKeyboard, App.BLL.DTO.WarehousePart>().ReverseMap();
        
        CreateMap<App.DAL.DTO.WarehousePartWithPart, App.BLL.DTO.WarehousePart>().ReverseMap();
        
        CreateMap<App.DAL.DTO.WarehousePartForWareHouse, App.BLL.DTO.WarehousePart>().ReverseMap();
        
        CreateMap<App.DAL.DTO.Identity.AppUser, App.BLL.DTO.Identity.AppUser>().ReverseMap();
        
        
    }
}