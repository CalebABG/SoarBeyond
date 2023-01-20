using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Components;
using SoarBeyond.Domain.Dto;

namespace SoarBeyond.Web.Components;

public partial class CreateMomentForm
{
    [Parameter] public EventCallback<Moment> OnValidSubmit { get; set; }

    private bool _addNotes;
    private Moment _moment = new();

    public CreateMomentForm()
    {
        Class = "card shadow";
    }

    private async Task HandleValidSubmitAsync()
    {
        IsBusy = true;
        await OnValidSubmit.InvokeAsync(_moment);
        IsBusy = false;
    }

    private void AddNote(Note note)
    {
        var contains = _moment.Notes.Contains(note);
        if (contains) return;

        _moment.Notes.Add(note);

        _addNotes = false;
    }

    private void RemoveNote(Note note)
    {
        var contains = _moment.Notes.Contains(note);
        if (contains) _moment.Notes.Remove(note);
    }

    private void ResetModel()
    {
        _moment = new();
    }

    private void ToggleNoteDialog()
    {
        _addNotes = !_addNotes;
        if (!_addNotes) _moment.Notes.Clear();
    }
}