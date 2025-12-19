using System;
using System.Security.Claims;
using KASHOP.DAL.DTO;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace KASHOP.BLL.Service;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailSender _emailSender;
    public AuthenticationService(UserManager<ApplicationUser> userManager , IConfiguration configuration , IEmailSender emailSender)
    {
        _userManager = userManager;
        _configuration = configuration;
        _emailSender = emailSender;
    }
    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
         try
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if(user is null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "invalid Email.",
                    
                };
            }
            var result = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!result)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "invalid Password.",
                    
                };
            }
            return new LoginResponse
            {
                Success = true,
                Message = "succes ",
                 AccessToken = await GenerateAccessToken(user)
            };
        }
        catch (Exception ex)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "An unexpected error.",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
    {
        try
        {
            var user = registerRequest.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user, registerRequest.password);
            if (!result.Succeeded)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "error",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = Uri.EscapeDataString(token);
            var EmailURL = $"http://localhost:5296/api/auth/Account/ConfirmEmail?token={token}&userId={user.Id}";
            await _userManager.AddToRoleAsync(user, "User");
            await _emailSender.SendEmailAsync(user.Email, "Welcome to KASHOP", $"<h1>Thank {user.UserName} for registering at KASHOP!</h1> <a href='{EmailURL}'>Confirm email</a>");    
            return new RegisterResponse
            {
                Success = true,
                Message = "succes "
            };
        }
        catch (Exception ex)
        {
            return new RegisterResponse
            {
                Success = false,
                Message = "An unexpected error.",
                Errors = new List<string> { ex.Message }
            };
        }

    }

    public async Task<bool> ConfirmEmailAsync(string token , string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if(user is null)
        return false;
        var result = await _userManager.ConfirmEmailAsync(user , token);
        if(!result.Succeeded)
            return false;

        return true;
    }
    private async Task<string> GenerateAccessToken(ApplicationUser user)
    {
        var userClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecurityKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience:  _configuration["JWT:Audience"],
            claims: userClaims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
