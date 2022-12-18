using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Domain.Dto;

namespace SoarBeyond.Web.Components;

public partial class MomentView
{
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Parameter] public Moment Moment { get; set; }
    [Parameter] public bool ShowNotes { get; set; } = true;
    [Parameter] public bool ShowNoteCount { get; set; } = true;
    [Parameter] public EventCallback<Moment> OnDeleteMoment { get; set; }

    public MomentView()
    {
        Class = "card shadow";
    }

    private bool CanShowNotes => ShowNotes && Moment?.Notes.Count > 0;
    private bool CanShowNoteCount => ShowNoteCount && Moment?.Notes.Count > 0;

    private void NavigateToViewPage() => NavigationManager.NavigateTo($"Moments/{Moment.Id}");

    private async Task DeleteMomentAsync()
    {
        await OnDeleteMoment.InvokeAsync(Moment);
    }
}