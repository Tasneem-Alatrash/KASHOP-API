using System;
using Microsoft.AspNetCore.Http;

namespace KASHOP.BLL.Service;

public interface IFileService
{
    Task<string?> UploadAsync(IFormFile file);
}
