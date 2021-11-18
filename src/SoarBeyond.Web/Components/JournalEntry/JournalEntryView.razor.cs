using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Web.Components;

public partial class JournalEntryView
{
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Parameter] public JournalEntry JournalEntry { get; set; }
    [Parameter] public bool ShowThoughts { get; set; } = true;
    [Parameter] public bool ShowBadgeCount { get; set; } = true;
    [Parameter] public EventCallback<JournalEntry> OnDeleteJournalEntry { get; set; }

    public JournalEntryView()
    {
        Class = "card shadow";
    }

    private bool CanShowThoughts => ShowThoughts && JournalEntry?.Thoughts.Count > 0;
    private bool CanShowBadgeCount => ShowBadgeCount && JournalEntry?.Thoughts.Count > 0;

    private void NavigateToViewPage() => NavigationManager.NavigateTo($"JournalEntries/{JournalEntry.Id}");

    private async Task DeleteJournalEntryAsync()
    {
        await OnDeleteJournalEntry.InvokeAsync(JournalEntry);
    }
}