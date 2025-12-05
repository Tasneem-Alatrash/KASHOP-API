using System;
using KASHOP.DAL.DTO;

namespace KASHOP.BLL.Service;

public interface IAuthenticationService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
}
