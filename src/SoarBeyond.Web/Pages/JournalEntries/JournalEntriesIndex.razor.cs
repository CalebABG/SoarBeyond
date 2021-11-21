using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Domain.MediatR.JournalEntries;
using SoarBeyond.Shared.Dto;
using SoarBeyond.Web.Components;

namespace SoarBeyond.Web.Pages.JournalEntries;

public partial class JournalEntriesIndex
{
    [Inject] private IMediator Mediator { get; set; }

    private ConfirmationDialog _confirmationDialog;

    private LinkedList<JournalEntry> _journalEntries;

    protected override async Task OnInitializedAsync()
    {
        await BeyondComponentRunAsync(async () =>
        {
            var journalEntries = await GetDbJournalEntriesAsync();
            _journalEntries = new LinkedList<JournalEntry>(journalEntries
                .OrderByDescending(dto => dto.CreationDate));
        });
    }

    private async Task<IEnumerable<JournalEntry>> GetDbJournalEntriesAsync()
    {
        return await Mediator.Send(new GetAllJournalEntriesRequest
        {
            UserId = await GetUserId()
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

                bool deleted = await Mediator.Send(deleteRequest);
                if (deleted) _journalEntries.Remove(journalEntry);
            });
        }
    }
}