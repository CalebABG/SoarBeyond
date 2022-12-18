using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Domain.Dto;

namespace SoarBeyond.Web.Components;

public partial class JournalView
{
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Parameter] public Journal Journal { get; set; }
    [Parameter] public bool ShowBadgeCount { get; set; } = true;
    [Parameter] public EventCallback<Journal> OnDeleteJournal { get; set; }
    [Parameter] public EventCallback<Journal> OnToggleFavorite { get; set; }

    public JournalView()
    {
        Class = "card shadow";
    }

    private bool CanShowBadgeCount => ShowBadgeCount && Journal?.Moments.Count > 0;

    private void NavigateToViewPage()
    {
        NavigationManager.NavigateTo($"Journals/{Journal.Id}");
    }

    private async Task DeleteJournalAsync()
    {
        await OnDeleteJournal.InvokeAsync(Journal);
    }

    private async Task ToggleFavoriteAsync()
    {
        await OnToggleFavorite.InvokeAsync(Journal);
    }
}