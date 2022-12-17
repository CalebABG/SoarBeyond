using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Web.Components;

public partial class CreateJournalForm
{
    [Parameter] public bool ShowTitle { get; set; } = true;
    [Parameter] public EventCallback<Journal> OnValidSubmit { get; set; }

    private Journal _journal = new();

    public CreateJournalForm()
    {
        Class = "card shadow";
    }

    private async Task HandleValidSubmitAsync()
    {
        IsBusy = true;
        await OnValidSubmit.InvokeAsync(_journal);
        IsBusy = false;
    }

    private void ResetModel()
    {
        _journal = new();
    }

    public void ResetForm()
    {
        ResetModel();
    }
}