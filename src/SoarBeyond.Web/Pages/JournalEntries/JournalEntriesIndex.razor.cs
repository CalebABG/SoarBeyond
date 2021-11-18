using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SoarBeyond.Domain.MediatR.JournalEntries;
using SoarBeyond.Shared.Dto;
using SoarBeyond.Web.Components;

namespace SoarBeyond.Web.Pages.JournalEntries;

public partial class JournalEntriesIndex
{
    private IMediator _mediator;

    private ConfirmationDialog _confirmationDialog;

    private LinkedList<JournalEntry> _journalEntries;

    protected override void OnInitialized()
    {
        _mediator ??= ScopedServices.GetRequiredService<IMediator>();
    }

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
        return await _mediator.Send(new GetAllJournalEntriesRequest
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

                bool deleted = await _mediator.Send(deleteRequest);
                if (deleted) _journalEntries.Remove(journalEntry);
            });
        }
    }
}