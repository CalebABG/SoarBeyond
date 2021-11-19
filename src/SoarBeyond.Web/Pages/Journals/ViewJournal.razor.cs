using Blazored.Toast.Services;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Domain.MediatR.JournalEntries;
using SoarBeyond.Domain.MediatR.Journals;
using SoarBeyond.Shared.Dto;
using SoarBeyond.Web.Components;

namespace SoarBeyond.Web.Pages.Journals;

public partial class ViewJournal
{
    [Parameter] public int JournalId { get; set; }

    private IMediator _mediator;
    private IToastService _toastService;

    private bool _showForm;
    private bool _requestFailed;

    private ConfirmationDialog _confirmationDialog;

    private Journal _journal;
    private LinkedList<JournalEntry> _journalEntries;

    private string JournalEntryFormButtonText => _showForm ? "Cancel" : "Add an Entry";
    private string JournalEntryFormButtonColor => _showForm ? "btn-danger" : "btn-primary";
    private string JournalEntryFormButtonClass => $"btn btn-lg {JournalEntryFormButtonColor} shadow text-white";

    protected override async Task OnInitializedAsync()
    {
        _mediator ??= ScopedServices.GetRequiredService<IMediator>();
        _toastService ??= ScopedServices.GetRequiredService<IToastService>();

        await BeyondComponentRunAsync(async () =>
        {
            _requestFailed = false;

            var resultJournal = await GetJournalFromDb();
            if (resultJournal is not null)
            {
                _journal = resultJournal;
                _journalEntries = new LinkedList<JournalEntry>(_journal.JournalEntries
                    .OrderByDescending(dto => dto.CreationDate));
            }
            else
            {
                _requestFailed = true;
            }
        });
    }

    private async Task<Journal> GetJournalFromDb()
    {
        return await _mediator.Send(new GetJournalRequest
        {
            UserId = await GetUserId(),
            JournalId = JournalId
        });
    }

    private async Task UpdateJournalAsync(string value)
    {
        await BeyondComponentRunAsync(async () =>
        {
            var request = new UpdateJournalRequest
            {
                UserId = await GetUserId(),
                JournalId = JournalId,
                Journal = _journal
            };

            /* Todo: Handle case where update fails and response is null */
            _journal = await _mediator.Send(request);
        });
    }

    private async Task CreateJournalEntryAsync(JournalEntry journalEntry)
    {
        await BeyondComponentRunAsync(async () =>
        {
            var request = new CreateJournalEntryRequest
            {
                UserId = await GetUserId(),
                JournalId = JournalId,
                JournalEntry = journalEntry
            };

            var resultJournalEntry = await _mediator.Send(request);
            if (resultJournalEntry is not null)
            {
                CloseForm();
                _journalEntries.AddFirst(resultJournalEntry);
                _toastService.ShowSuccess("Created Journal Entry");
            }
            else
            {
                _toastService.ShowError("Something went wrong creating " +
                                        "your Journal Entry, please try again.");
            }
        });
    }

    private async Task DeleteJournalEntryAsync(JournalEntry journalEntry)
    {
        var result = await _confirmationDialog.ShowAsync(
            "Confirm Delete",
            $"Are you sure you want to delete `{journalEntry.Title.Truncate(50)}`"
        );

        if (result)
        {
            await BeyondComponentRunAsync(async () =>
            {
                var deleteRequest = new DeleteJournalEntryRequest
                {
                    UserId = await GetUserId(),
                    JournalId = journalEntry.JournalId,
                    JournalEntryId = journalEntry.Id
                };

                /* Todo: Add Modal for delete? Make sure you truly want to delete */
                bool deleted = await _mediator.Send(deleteRequest);
                if (deleted) _journalEntries.Remove(journalEntry);
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