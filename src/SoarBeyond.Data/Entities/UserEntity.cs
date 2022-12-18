using Microsoft.AspNetCore.Identity;

namespace SoarBeyond.Data.Entities;

public class UserEntity : IdentityUser<int>
{
    [ProtectedPersonalData]
    public DateTimeOffset CreatedDate { get; set; }

    [ProtectedPersonalData] 
    public string NickName { get; set; } = string.Empty;
}