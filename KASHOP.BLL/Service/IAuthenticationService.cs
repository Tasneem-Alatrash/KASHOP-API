using System;

using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;

namespace KASHOP.BLL.Service;

public interface IAuthenticationService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
    Task<bool> ConfirmEmailAsync(string token , string userId);
    Task<ForgetPasswordResponse> RequestPasswordReset(ForgetPasswordRequest request);
    Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request);
}
