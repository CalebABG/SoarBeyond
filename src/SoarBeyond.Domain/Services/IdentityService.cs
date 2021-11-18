using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SoarBeyond.Data.Entities;
using SoarBeyond.Domain.Services.Interfaces;

namespace SoarBeyond.Domain.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<SoarBeyondUserEntity> _userManager;

    public IdentityService(UserManager<SoarBeyondUserEntity> userManager)
    {
        _userManager = userManager;
    }

    public async Task<SoarBeyondUserEntity> GetUserByIdAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user;
    }

    public async Task<bool> CheckUserEmailExistsAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task<string> GetUserNameAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user?.UserName;
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        IdentityResult result = null;
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user != null) result = await _userManager.DeleteAsync(user);

        return result?.Succeeded ?? false;
    }
}