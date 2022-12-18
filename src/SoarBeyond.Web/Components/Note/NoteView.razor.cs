using System.Threading.Tasks;
using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Domain.Dto;

namespace SoarBeyond.Web.Components;

public partial class NoteView
{
    [Parameter] public Note Note { get; set; }
    [Parameter] public bool ShowActionButtons { get; set; }
    [Parameter] public bool ShowHeader { get; set; } = true;
    [Parameter] public EventCallback<Note> OnDeleteNote { get; set; }

    public NoteView()
    {
        Class = "card shadow-sm";
    }

    private async Task HandleDeleteNote()
    {
        await OnDeleteNote.InvokeAsync(Note);
    }
}