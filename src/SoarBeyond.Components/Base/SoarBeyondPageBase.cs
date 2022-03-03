using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SoarBeyond.Shared.Extensions;

namespace SoarBeyond.Components;

public abstract class SoarBeyondPageBase : SoarBeyondComponentBase
{
    [Inject] protected NavigationManager NavigationManager { get; set; }
    [CascadingParameter] public Task<AuthenticationState> AuthStateTask { get; set; }

    /// <summary>
    /// Gets set by using the AuthenticationStateProvider
    /// and querying if the user is Authenticated, and if so,
    /// getting the users Id from the ClaimsPrincipal.
    /// <see cref="ClaimsPrincipalExtensions"/>
    /// </summary>
    protected async Task<int> GetUserIdAsync()
    {
        var authState = await AuthStateTask;
        return int.TryParse(authState.User.GetUserId(), out var parsedUserId)
            ? parsedUserId
            : -1;
    }
}