using Blazored.Toast.Services;
using MediatR;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Domain.MediatR.Journals;
using SoarBeyond.Shared.Dto;
using SoarBeyond.Web.Components;

namespace SoarBeyond.Web.Pages.Journals;

public partial class CreateJournal
{
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private IToastService ToastService { get; set; }

    private CreateJournalForm _journalForm;

    private async Task CreateNewJournalAsync(Journal journal)
    {
        await BeyondComponentRunAsync(async () =>
        {
            var resultJournal = await Mediator.Send(new CreateJournalRequest
            {
                UserId = await GetUserId(),
                Journal = journal
            });

            if (resultJournal is not null)
            {
                _journalForm.ResetForm();
                ToastService.ShowSuccess("Created Journal");
                NavigationManager.NavigateTo("Journals");
            }
            else
            {
                ToastService.ShowError("Something went wrong creating " +
                                        "your Journal, please try again.");
            }
        });
    }
}