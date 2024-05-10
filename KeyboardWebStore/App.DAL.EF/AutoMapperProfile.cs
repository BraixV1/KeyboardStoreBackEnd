using AutoMapper;

namespace App.DAL.EF;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<App.Domain.Contest, App.DAL.DTO.Contest>().ReverseMap();

        CreateMap<App.Domain.Identity.AppRefreshToken, App.DAL.DTO.AppRefreshToken>().ReverseMap();

        CreateMap<App.Domain.Keyboard, App.DAL.DTO.Keyboard>().ReverseMap();
        
        CreateMap<App.Domain.Keyboard, App.DAL.DTO.KeyboardWithoutCollection>().ReverseMap();
        
        CreateMap<App.Domain.KeyboardPart, App.DAL.DTO.KeyboardPart>().ReverseMap();
        
        CreateMap<App.Domain.KeyboardPart, App.DAL.DTO.KeyboardPartWithKeyboard>().ReverseMap();
        
        CreateMap<App.Domain.KeyboardPart, App.DAL.DTO.KeyboardPartWithPart>().ReverseMap();
        
        CreateMap<App.Domain.KeyboardRating, App.DAL.DTO.KeyboardRating>().ReverseMap();
        
        CreateMap<App.Domain.KeyboardRating, App.DAL.DTO.KeyboardRatingForKeyboard>().ReverseMap();

        CreateMap<App.Domain.KeyboardBuild, App.DAL.DTO.KeyboardBuild>().ReverseMap();

        CreateMap<App.Domain.Part, App.DAL.DTO.Part>().ReverseMap();
        
        CreateMap<App.Domain.Part, App.DAL.DTO.PartWithoutCollection>().ReverseMap();
        
        CreateMap<App.Domain.Part, App.DAL.DTO.PartWithoutWarehouse>().ReverseMap();
        
        CreateMap<App.Domain.PartRating, App.DAL.DTO.PartRating>().ReverseMap();
        
        CreateMap<App.Domain.PartRating, App.DAL.DTO.PartRatingForPart>().ReverseMap();
        
        CreateMap<App.Domain.HelperEnums.OrderStatus, App.DAL.DTO.HelperEnums.OrderStatus>().ReverseMap();
        
        CreateMap<App.Domain.PartInBuild, App.DAL.DTO.PartInBuild>().ReverseMap();
        
        CreateMap<App.Domain.PartCategory, App.DAL.DTO.PartCategory>().ReverseMap();
        
        CreateMap<App.Domain.Order, App.DAL.DTO.Order>().ReverseMap();
        
        CreateMap<App.Domain.OrderItem, App.DAL.DTO.OrderItem>().ReverseMap();
        
        CreateMap<App.Domain.OrderItem, App.DAL.DTO.OrderItemForOrder>().ReverseMap();
        
        CreateMap<App.Domain.Warehouse, App.DAL.DTO.Warehouse>().ReverseMap();
        
        CreateMap<App.Domain.Warehouse, App.DAL.DTO.WarehouseWithoutCollection>().ReverseMap();
        
        CreateMap<App.Domain.WarehousePart, App.DAL.DTO.WarehousePart>().ReverseMap();
        
        CreateMap<App.Domain.WarehousePart, App.DAL.DTO.WarehousePartForWareHouse>().ReverseMap();
        
        CreateMap<App.Domain.WarehousePart, App.DAL.DTO.WarehousePartWithPart>().ReverseMap();
        
        CreateMap<App.Domain.WarehousePart, App.DAL.DTO.WarehousePartWithKeyboard>().ReverseMap();
        
        CreateMap<App.Domain.PartCategory, App.DAL.DTO.PartCategoryWithPart>().ReverseMap();
        
        CreateMap<App.Domain.Category, App.DAL.DTO.Category>().ReverseMap();
        
        CreateMap<App.Domain.Category, App.DAL.DTO.CategoryWithoutCollection>().ReverseMap();
        
        CreateMap<App.Domain.PartCategory, App.DAL.DTO.PartCategoryWithKeyboard>().ReverseMap();
        
        CreateMap<App.Domain.PartCategory, App.DAL.DTO.PartCategoryForCategory>().ReverseMap();
        
        CreateMap<App.Domain.PartCategory, App.DAL.DTO.PartCategory>().ReverseMap();

        CreateMap<App.Domain.PartInBuild, App.DAL.DTO.PartInBuildWithPart>().ReverseMap();
        
        CreateMap<App.Domain.PartInBuild, App.DAL.DTO.PartInBuildWithBuild>().ReverseMap();
        
        CreateMap<App.Domain.Identity.AppUser, App.DAL.DTO.Identity.AppUser>().ReverseMap();
        
        
        
    }
}