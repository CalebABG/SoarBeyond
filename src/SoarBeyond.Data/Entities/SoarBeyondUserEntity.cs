using Microsoft.AspNetCore.Identity;

namespace SoarBeyond.Data.Entities;

public class SoarBeyondUserEntity : IdentityUser<int>
{
    [ProtectedPersonalData]
    public DateTime CreationDate { get; set; }
}