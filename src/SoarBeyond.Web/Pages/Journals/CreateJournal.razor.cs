using System.Threading.Tasks;
using Blazored.Toast.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SoarBeyond.Domain.MediatR.Journals;
using SoarBeyond.Shared.Dto;
using SoarBeyond.Web.Components;

namespace SoarBeyond.Web.Pages.Journals;

public partial class CreateJournal
{
    private IMediator _mediator;
    private IToastService _toastService;

    private CreateJournalForm _journalForm;

    protected override void OnInitialized()
    {
        _mediator ??= ScopedServices.GetRequiredService<IMediator>();
        _toastService ??= ScopedServices.GetRequiredService<IToastService>();
    }

    private async Task CreateNewJournalAsync(Journal journal)
    {
        await BeyondComponentRunAsync(async () =>
        {
            var resultJournal = await _mediator.Send(new CreateJournalRequest
            {
                UserId = await GetUserId(),
                Journal = journal
            });

            if (resultJournal is not null)
            {
                _journalForm.ResetForm();
                _toastService.ShowSuccess("Created Journal");
                NavigationManager.NavigateTo("Journals");
            }
            else
            {
                _toastService.ShowError("Something went wrong creating " +
                                        "your Journal, please try again.");
            }
        });
    }
}