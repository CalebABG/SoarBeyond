using Blazored.Toast.Services;
using MediatR;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Mediator.Journals;
using SoarBeyond.Web.Components;

namespace SoarBeyond.Web.Pages.Journals;

public partial class Create
{
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private IToastService ToastService { get; set; }

    private CreateJournalForm _journalForm;

    private async Task CreateJournalAsync(Journal journal)
    {
        await ComponentRunAsync(async () =>
        {
            var result = await Mediator.Send(new CreateJournalRequest(await GetUserIdAsync(), journal));
            if (result is not null)
            {
                _journalForm.ResetForm();
                NavigationManager.NavigateTo("Journals");
            }
            else
            {
                ToastService.ShowError("Something went wrong when creating. Please try again.");
            }
        });
    }
}