using Microsoft.AspNetCore.Components;

namespace SoarBeyond.Components;

public static class NavigationManagerExtensions
{
    public static void RefreshCurrentPage(this NavigationManager navigationManager)
    {
        navigationManager.NavigateTo(navigationManager.Uri, true);
    }
}