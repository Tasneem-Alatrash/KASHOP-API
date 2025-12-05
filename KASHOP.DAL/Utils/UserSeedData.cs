using System;
using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KASHOP.DAL.Utils;

public class UserSeedData : ISeedData
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UserSeedData(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task DataSeed()
    {
        if (!await _userManager.Users.AnyAsync())
        {
            var User1 = new ApplicationUser
            {
                UserName = "tshreem",
                Email = "t@gmail.com",
                FullName = "Tariq Shreem",
                City = "Nablus",
                Street = "Main Street",
                EmailConfirmed = true
            };

            var User2 = new ApplicationUser
            {
                UserName = "talatrash",
                Email = "talatrash@gmail.com",
                FullName = "Tasneem Alatrash",

                City = "Jenin",
                Street = "Main Street",
                EmailConfirmed = true
            };
            var User3 = new ApplicationUser
            {
                UserName = "soma",
                Email = "s@gmail.com",
                FullName = "Soma Alatrash",
                City = "Jenin",
                Street = "Main Street",
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(User1, "Pass@1122");
            await _userManager.CreateAsync(User2, "Pass@1122");
            await _userManager.CreateAsync(User3, "Pass@1122");

            await _userManager.AddToRoleAsync(User1, "SuperAdmin");
            await _userManager.AddToRoleAsync(User2, "Admin");
            await _userManager.AddToRoleAsync(User1, "User");
        }
    }
}
