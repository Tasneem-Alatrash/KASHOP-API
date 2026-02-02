using System;
using KASHOP.DAL;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;

namespace KASHOP.BLL.Service;

public class ProductSerivce : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IFileService _fileService;
    public ProductSerivce(IProductRepository productRepository, IFileService fileService)
    {
        _productRepository = productRepository;
        _fileService = fileService;
    }
    public async Task<List<ProductResponse>> getAllProductsForAdminAsync()
    {
       var products = await _productRepository.GetAllAsync();
            return products.Adapt<List<ProductResponse>>();
    }
    public async Task<ProductResponse> createProductAsync(ProductRequest request)
    {
        var product = request.Adapt<Product>(); 
        if (request.MainImage != null)
        {
            var imagePath = await _fileService.UploadAsync(request.MainImage);
            product.MainImage = imagePath;
        }
        await _productRepository.AddAsync(product);
        return product.Adapt<ProductResponse>();
    }
}
