using Blazored.Toast.Services;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Components;
using SoarBeyond.Domain.MediatR.Journals;
using SoarBeyond.Shared.Dto;
using SoarBeyond.Web.Components;

namespace SoarBeyond.Web.Pages.Journals;

public partial class JournalsIndex : SoarBeyondPageBase
{
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private IToastService ToastService { get; set; }

    private bool _showDialog;

    private ConfirmationDialog _confirmationDialog;

    /* Todo: Use Pagination for large sets of data */
    private LinkedList<Journal> _journals;

    protected override async Task OnInitializedAsync()
    {
        await BeyondComponentRunAsync(async () =>
        {
            var journals = await GetJournalsFromDb();
            _journals = new LinkedList<Journal>(journals
                .OrderByDescending(dto => dto.CreationDate));
        });
    }

    private async Task<IEnumerable<Journal>> GetJournalsFromDb()
    {
        return await Mediator.Send(new GetAllJournalsRequest
        {
            UserId = await GetUserIdAsync()
        });
    }

    private void OpenDialog() => _showDialog = true;
    private void CloseDialog() => _showDialog = false;

    private async Task CreateJournalFromDialog(Journal journal)
    {
        await BeyondComponentRunAsync(async () =>
        {
            CreateJournalRequest createRequest = new()
            {
                UserId = await GetUserIdAsync(),
                Journal = journal
            };

            var resultJournal = await Mediator.Send(createRequest);
            if (resultJournal is not null)
            {
                CloseDialog();
                _journals.AddFirst(resultJournal);
                ToastService.ShowSuccess("Created Journal");
            }
            else
            {
                ToastService.ShowError("Something went wrong creating" +
                                        " your Journal, please try again.");
            }
        });
    }

    private async Task DeleteJournalAsync(Journal journal)
    {
        var result = await _confirmationDialog.ShowAsync(
            "Confirm Delete",
            $"Are you sure you want to delete `{journal.Name.Truncate(50)}`"
        );

        if (result)
        {
            await BeyondComponentRunAsync(async () =>
            {
                var request = new DeleteJournalRequest
                {
                    UserId = await GetUserIdAsync(),
                    JournalId = journal.Id
                };

                bool deleted = await Mediator.Send(request);
                if (deleted) _journals.Remove(journal);
            });
        }
    }
}