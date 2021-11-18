using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Web.Components;

public partial class CreateJournalForm
{
    [Parameter] public bool ShowTitle { get; set; } = true;
    [Parameter] public EventCallback<Journal> OnValidSubmit { get; set; }

    private Journal _model = new();

    public CreateJournalForm()
    {
        Class = "card shadow";
    }

    private async Task HandleValidSubmitAsync()
    {
        IsBusy = true;
        await OnValidSubmit.InvokeAsync(_model);
        IsBusy = false;
    }

    private void ResetModel()
    {
        _model = new();
    }

    public void ResetForm()
    {
        ResetModel();
    }
}