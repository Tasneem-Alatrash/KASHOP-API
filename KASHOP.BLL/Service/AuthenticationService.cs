using System;
using KASHOP.DAL.DTO;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace KASHOP.BLL.Service;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;    
    public AuthenticationService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        throw new NotImplementedException();
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
    {
        var user = registerRequest.Adapt<ApplicationUser>();
        var result =await  _userManager.CreateAsync(user ,registerRequest.password);
        if(!result.Succeeded)
        {
            return new RegisterResponse
            {
                Message ="error"
            };
        }
        await  _userManager.AddToRoleAsync(user , "User");
        return new RegisterResponse
            {
                Message ="succes "
            };
    }
}
