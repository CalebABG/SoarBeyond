using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using SoarBeyond.Domain.MediatR.JournalEntries;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Web.Pages.JournalEntries;

public partial class ViewJournalEntry
{
    [Parameter] public int JournalEntryId { get; set; }

    private IMediator _mediator;

    private JournalEntry _journalEntry;
    private LinkedList<Thought> _thoughts;

    private bool _requestFailed;

    protected override async Task OnInitializedAsync()
    {
        _mediator ??= ScopedServices.GetRequiredService<IMediator>();

        await BeyondComponentRunAsync(async () =>
        {
            _requestFailed = false;

            var resultJournalEntry = await GetJournalEntryFromDb();
            if (resultJournalEntry is not null)
            {
                _journalEntry = resultJournalEntry;
                _thoughts = new LinkedList<Thought>(_journalEntry.Thoughts
                    .OrderByDescending(je => je.CreationDate).ToList());
            }
            else
            {
                _requestFailed = true;
            }
        });
    }

    private async Task<JournalEntry> GetJournalEntryFromDb()
    {
        return await _mediator.Send(new GetJournalEntryRequest
        {
            UserId = await GetUserId(),
            JournalEntryId = JournalEntryId,
        });
    }

    private async Task UpdateJournalEntryAsync(string value)
    {
        await BeyondComponentRunAsync(async () =>
        {
            var request = new UpdateJournalEntryRequest
            {
                UserId = await GetUserId(),
                JournalEntryId = JournalEntryId,
                JournalEntry = _journalEntry,
            };

            _journalEntry = await _mediator.Send(request);
        });
    }
}