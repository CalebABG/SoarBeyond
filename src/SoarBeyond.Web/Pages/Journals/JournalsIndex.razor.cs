using Blazored.Toast.Services;
using Humanizer;
using MediatR;
using SoarBeyond.Components;
using SoarBeyond.Domain.MediatR.Journals;
using SoarBeyond.Shared.Dto;
using SoarBeyond.Web.Components;

namespace SoarBeyond.Web.Pages.Journals;

public partial class JournalsIndex : SoarBeyondPageBase
{
    private bool _showDialog;

    private IMediator _mediator;
    private IToastService _toastService;

    private BSDialog _bsDialog;
    private ConfirmationDialog _confirmationDialog;

    /* Todo: Use Pagination for large sets of data */
    /* Todo: LinkedList efficient add/insert O(1) - delete/search O(N) */
    private LinkedList<Journal> _journals;

    protected override void OnInitialized()
    {
        _mediator ??= ScopedServices.GetRequiredService<IMediator>();
        _toastService ??= ScopedServices.GetRequiredService<IToastService>();
    }

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
        return await _mediator.Send(new GetAllJournalsRequest
        {
            UserId = await GetUserId()
        });
    }

    private async Task CreateJournalFromDialog(Journal journal)
    {
        await BeyondComponentRunAsync(async () =>
        {
            CreateJournalRequest createRequest = new()
            {
                UserId = await GetUserId(),
                Journal = journal
            };

            var resultJournal = await _mediator.Send(createRequest);
            if (resultJournal is not null)
            {
                _bsDialog.CloseDialog();
                _journals.AddFirst(resultJournal);
                _toastService.ShowSuccess("Created Journal");
            }
            else
            {
                _toastService.ShowError("Something went wrong creating" +
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
                    UserId = await GetUserId(),
                    JournalId = journal.Id
                };

                bool deleted = await _mediator.Send(request);
                if (deleted) _journals.Remove(journal);
            });
        }
    }
}