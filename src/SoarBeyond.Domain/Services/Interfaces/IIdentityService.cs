using SoarBeyond.Data.Entities;

namespace SoarBeyond.Domain.Services.Interfaces;

public interface IIdentityService
{
    Task<SoarBeyondUserEntity> GetUserByIdAsync(int userId);

    Task<bool> CheckUserEmailExistsAsync(string email);
    Task<string> GetUserNameAsync(int userId);

    // Todo: Add CreateUserAsync method

    Task<bool> DeleteUserAsync(int userId);
}