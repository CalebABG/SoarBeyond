using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Web.Components;

public partial class JournalView
{
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Parameter] public Journal Journal { get; set; }
    [Parameter] public bool ShowBadgeCount { get; set; } = true;
    [Parameter] public EventCallback<Journal> OnDeleteJournal { get; set; }

    public JournalView()
    {
        Class = "card shadow";
    }

    private bool CanShowBadgeCount => ShowBadgeCount && Journal?.JournalEntries.Count > 0;

    private void NavigateToViewPage()
    {
        NavigationManager.NavigateTo($"Journals/{Journal.Id}");
    }

    private async Task DeleteJournalAsync()
    {
        await OnDeleteJournal.InvokeAsync(Journal);
    }
}