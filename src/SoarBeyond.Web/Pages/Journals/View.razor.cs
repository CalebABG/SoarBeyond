using Blazored.Toast.Services;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Components;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Mediator.Moments;
using SoarBeyond.Domain.Mediator.Journals;

namespace SoarBeyond.Web.Pages.Journals;

public partial class View
{
    [Parameter] public int JournalId { get; set; }

    [Inject] private IMediator Mediator { get; set; }
    [Inject] private IToastService ToastService { get; set; }

    private bool _showForm;
    private bool _requestFailed;

    private ConfirmationDialog _confirmationDialog;

    private Journal _journal;
    private LinkedList<Moment> _moments;

    private string MomentFormButtonText => _showForm ? "Cancel" : "Add a Moment";
    private string MomentFormButtonColor => _showForm ? "btn-danger" : "btn-primary";
    private string MomentFormButtonClass => $"btn btn-lg {MomentFormButtonColor} shadow text-white";

    protected override async Task OnInitializedAsync()
    {
        await ComponentRunAsync(async () =>
        {
            _requestFailed = false;

            var resultJournal = await GetJournalFromDbAsync();
            if (resultJournal is not null)
            {
                _journal = resultJournal;
                _moments = new LinkedList<Moment>(_journal.Moments
                    .OrderByDescending(dto => dto.CreatedDate));
            }
            else
            {
                _requestFailed = true;
            }
        });
    }

    private async Task<Journal> GetJournalFromDbAsync()
    {
        return await Mediator.Send(new GetJournalRequest(await GetUserIdAsync(), JournalId));
    }

    private async Task UpdateJournalAsync(string value)
    {
        await ComponentRunAsync(async () =>
        {
            var request = new UpdateJournalRequest(await GetUserIdAsync(), _journal);

            /* Todo: Handle case where update fails and response is null */
            _journal = await Mediator.Send(request);
        });
    }

    private async Task CreateMomentAsync(Moment moment)
    {
        await ComponentRunAsync(async () =>
        {
            var request = new CreateMomentRequest(await GetUserIdAsync(), JournalId, moment);

            var result = await Mediator.Send(request);
            if (result is not null)
            {
                CloseForm();
                _moments.AddFirst(result);
                ToastService.ShowSuccess("Created");
            }
            else
            {
                ToastService.ShowError("Something went wrong when creating. Please try again.");
            }
        });
    }

    private async Task DeleteMomentAsync(Moment moment)
    {
        var result = await _confirmationDialog.ShowAsync
        (
            "Confirm Delete",
            $"Are you sure you want to delete `{moment.Title.Truncate(50)}`"
        );

        if (result)
        {
            await ComponentRunAsync(async () =>
            {
                var request = new DeleteMomentRequest(await GetUserIdAsync(), moment.Id);

                /* Todo: Add Modal for delete? Make sure you truly want to delete */
                bool deleted = await Mediator.Send(request);
                if (deleted) _moments.Remove(moment);
            });
        }
    }

    private void OpenForm() => _showForm = true;
    private void CloseForm() => _showForm = false;

    private void ToggleFormVisibility()
    {
        if (_showForm) CloseForm();
        else OpenForm();
    }
}