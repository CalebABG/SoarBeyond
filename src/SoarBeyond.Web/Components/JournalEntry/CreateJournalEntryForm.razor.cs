using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Components;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Web.Components;

public partial class CreateJournalEntryForm
{
    [Parameter] public EventCallback<JournalEntry> OnValidSubmit { get; set; }

    private bool _addThoughts;
    private JournalEntry _journalEntry = new();

    public CreateJournalEntryForm()
    {
        Class = "card shadow";
    }

    private async Task HandleValidSubmitAsync()
    {
        IsBusy = true;
        await OnValidSubmit.InvokeAsync(_journalEntry);
        IsBusy = false;
    }

    private void AddThought(Thought thought)
    {
        var contains = _journalEntry.Thoughts.Contains(thought);
        if (contains) return;

        _journalEntry.Thoughts.Add(thought);

        _addThoughts = false;
    }

    private void RemoveThought(Thought thought)
    {
        var contains = _journalEntry.Thoughts.Contains(thought);
        if (contains) _journalEntry.Thoughts.Remove(thought);
    }

    private void ResetModel()
    {
        _journalEntry = new();
    }

    private void ToggleThoughtDialog()
    {
        _addThoughts = !_addThoughts;
        if (!_addThoughts) _journalEntry.Thoughts.Clear();
    }
}