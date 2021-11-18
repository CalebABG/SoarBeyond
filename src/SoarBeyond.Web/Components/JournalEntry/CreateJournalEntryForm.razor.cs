using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Components;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Web.Components;

public partial class CreateJournalEntryForm
{
    [Parameter] public int JournalId { get; set; }
    [Parameter] public EventCallback<JournalEntry> OnValidSubmit { get; set; }

    private bool _addThoughts;

    private BSDialog _bsDialog;

    private JournalEntry _journalEntryModel = new();

    public CreateJournalEntryForm()
    {
        Class = "card shadow";
    }

    private async Task HandleValidSubmitAsync()
    {
        IsBusy = true;
        await OnValidSubmit.InvokeAsync(_journalEntryModel);
        IsBusy = false;
    }

    private void AddThought(Thought thought)
    {
        var contains = _journalEntryModel.Thoughts.Contains(thought);
        if (contains) return;

        _journalEntryModel.Thoughts.Add(thought);
        _bsDialog.CloseDialog();
    }

    private void RemoveThought(Thought thought)
    {
        var contains = _journalEntryModel.Thoughts.Contains(thought);
        if (contains) _journalEntryModel.Thoughts.Remove(thought);
    }

    private void ResetModelFields()
    {
        _journalEntryModel = SetJournalEntryModel();
    }

    private JournalEntry SetJournalEntryModel() => new();

    private void ToggleViewThoughts()
    {
        _addThoughts = !_addThoughts;
        if (!_addThoughts) _journalEntryModel.Thoughts.Clear();
    }
}