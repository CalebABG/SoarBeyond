using Blazored.Toast.Services;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Components;
using SoarBeyond.Domain.Mediator.JournalEntries;
using SoarBeyond.Domain.Mediator.Journals;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Web.Pages.Journals;

public partial class ViewJournal
{
    [Parameter] public int JournalId { get; set; }

    [Inject] private IMediator Mediator { get; set; }
    [Inject] private IToastService ToastService { get; set; }

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
        await ComponentRunAsync(async () =>
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
        return await Mediator.Send(new GetJournalRequest
        {
            UserId = await GetUserIdAsync(),
            JournalId = JournalId
        });
    }

    private async Task UpdateJournalAsync(string value)
    {
        await ComponentRunAsync(async () =>
        {
            var request = new UpdateJournalRequest
            {
                UserId = await GetUserIdAsync(),
                JournalId = JournalId,
                Journal = _journal
            };

            /* Todo: Handle case where update fails and response is null */
            _journal = await Mediator.Send(request);
        });
    }

    private async Task CreateJournalEntryAsync(JournalEntry journalEntry)
    {
        await ComponentRunAsync(async () =>
        {
            var request = new CreateJournalEntryRequest
            {
                UserId = await GetUserIdAsync(),
                JournalId = JournalId,
                JournalEntry = journalEntry
            };

            var resultJournalEntry = await Mediator.Send(request);
            if (resultJournalEntry is not null)
            {
                CloseForm();
                _journalEntries.AddFirst(resultJournalEntry);
                ToastService.ShowSuccess("Created Journal Entry");
            }
            else
            {
                ToastService.ShowError("Something went wrong creating your Journal Entry, please try again.");
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
            await ComponentRunAsync(async () =>
            {
                var deleteRequest = new DeleteJournalEntryRequest
                {
                    UserId = await GetUserIdAsync(),
                    JournalId = journalEntry.JournalId,
                    JournalEntryId = journalEntry.Id
                };

                /* Todo: Add Modal for delete? Make sure you truly want to delete */
                bool deleted = await Mediator.Send(deleteRequest);
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