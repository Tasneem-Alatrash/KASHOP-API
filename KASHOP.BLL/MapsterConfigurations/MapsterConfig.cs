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
        Map(dest => dest.CreatedBy , source => source.Users.UserName);
    }
}
