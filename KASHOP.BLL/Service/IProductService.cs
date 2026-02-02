using System;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;

namespace KASHOP.BLL.Service;

public interface IProductService
{

    Task<List<ProductResponse>> getAllProductsForAdminAsync( );
    Task<ProductResponse> createProductAsync(ProductRequest request);
}
