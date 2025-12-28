using System;
using System.Security.Claims;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
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
    private readonly SignInManager<ApplicationUser> _signInManager;
    public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IEmailSender emailSender
    , SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _configuration = configuration;
        _emailSender = emailSender;
        _signInManager = signInManager;
    }
    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "invalid Email.",

                };
            }
            if (await _userManager.IsLockedOutAsync(user))
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Your account is locked. Please try again later.",

                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, true);
            if (result.IsLockedOut)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Your account is locked due to multiple failed login attempts. Please try again later.",

                };
            }
            else if (result.IsNotAllowed)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Email not confirmed. Please confirm your email before logging in.",

                };
            }
            if (!result.Succeeded)
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

    public async Task<bool> ConfirmEmailAsync(string token, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return false;
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
            return false;

        return true;
    }
    private async Task<string> GenerateAccessToken(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var userClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, string.Join(',',roles))
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecurityKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: userClaims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<ForgetPasswordResponse> RequestPasswordReset(ForgetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return new ForgetPasswordResponse
            {
                Success = false,
                Message = "Email not found."
            };
        }

        var random = new Random();
        var code = random.Next(1000, 9999).ToString();
        user.CodeResetPassword = code;
        user.PasswordResetCodeExpiry = DateTime.UtcNow.AddMinutes(15);
        await _userManager.UpdateAsync(user);
        await _emailSender.SendEmailAsync(user.Email, "Password Reset Code", $"<h1>Your password reset code is: {code}</h1>");
        return new ForgetPasswordResponse
        {
            Success = true,
            Message = "code sent to your email."
        };
    }

    public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return new ResetPasswordResponse
            {
                Success = false,
                Message = "Email not found."
            };
        }
        if (request.Code != user.CodeResetPassword)
        {
            return new ResetPasswordResponse
            {
                Success = false,
                Message = " Invalid code."
            };
        }
        else if (user.PasswordResetCodeExpiry < DateTime.UtcNow)
        {
            return new ResetPasswordResponse
            {
                Success = false,
                Message = " Code expired."
            };
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
        if (!result.Succeeded)
        {
            return new ResetPasswordResponse
            {
                Success = false,
                Message = " Password reset failed.",
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        await _emailSender.SendEmailAsync(user.Email, "Change password", $"<h1>Your password has been changed successfully.</h1>");
        return new ResetPasswordResponse
        {
            Success = true,
            Message = "Password reset successful."
        };
    }

}
