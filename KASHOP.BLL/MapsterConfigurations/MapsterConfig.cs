using System;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using Mapster;

namespace KASHOP.BLL.MapsterConfigurations;

public static class MapsterConfig
{
    public static void MapsterConfigRegister()
    {
        TypeAdapterConfig<Category , CategoryResponse>.NewConfig().
        Map(dest => dest.CreatedBy , source => source.User.FullName);


        TypeAdapterConfig<Category , CategoryUserResponse>.NewConfig().
        Map(dest => dest.Name , source => source.Trinslations.
        Where(t=>t.Language == MapContext.Current.Parameters["lang"].ToString()).
        Select(t=>t.Name).FirstOrDefault());

        TypeAdapterConfig<Product , ProductResponse>.NewConfig().
        Map(dest => dest.MainImage , source => $"http://localhost:5296/images/{source.MainImage}");
      
    }
}
