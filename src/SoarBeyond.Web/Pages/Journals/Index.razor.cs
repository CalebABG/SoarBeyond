using Blazored.Toast.Services;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Components;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Mediator.Journals;

namespace SoarBeyond.Web.Pages.Journals;

public partial class Index : SoarBeyondPageBase
{
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private IToastService ToastService { get; set; }

    private bool _showDialog;

    private ConfirmationDialog _confirmationDialog;

    /* Todo: Use Pagination for large sets of data */
    private LinkedList<Journal> _journals;

    protected override async Task OnInitializedAsync()
    {
        await ComponentRunAsync(async () =>
        {
            var journals = await GetJournalsFromDb();
            _journals = new LinkedList<Journal>(journals
                .OrderByDescending(dto => dto.CreatedDate));
        });
    }

    private async Task<IEnumerable<Journal>> GetJournalsFromDb()
    {
        return await Mediator.Send(new GetAllJournalsRequest(await GetUserIdAsync()));
    }

    private void OpenDialog() => _showDialog = true;
    private void CloseDialog() => _showDialog = false;

    private async Task CreateJournalFromDialog(Journal journal)
    {
        await ComponentRunAsync(async () =>
        {
             var createRequest = new CreateJournalRequest(await GetUserIdAsync(), journal);

            var resultJournal = await Mediator.Send(createRequest);
            if (resultJournal is not null)
            {
                CloseDialog();
                _journals.AddFirst(resultJournal);
                ToastService.ShowSuccess("Created");
            }
            else
            {
                ToastService.ShowError("Something went wrong when creating. Please try again.");
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
            await ComponentRunAsync(async () =>
            {
                var request = new DeleteJournalRequest(await GetUserIdAsync(), journal.Id);

                bool deleted = await Mediator.Send(request);
                if (deleted) _journals.Remove(journal);
            });
        }
    }

    private async Task ToggleFavoriteAsync(Journal journal)
    {
        await ComponentRunAsync(async () =>
        {
            var newStatus = !journal.Favored;
            var request = new UpdateJournalFavoriteStatusRequest(await GetUserIdAsync(), journal.Id, newStatus);

            bool updated = await Mediator.Send(request);
            if (updated) journal.Favored = newStatus;
        });
    }
}