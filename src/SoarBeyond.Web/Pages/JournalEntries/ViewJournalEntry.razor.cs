using MediatR;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Domain.MediatR.JournalEntries;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Web.Pages.JournalEntries;

public partial class ViewJournalEntry
{
    [Parameter] public int JournalEntryId { get; set; }
    [Inject] private IMediator Mediator { get; set; }

    private JournalEntry _journalEntry;
    private LinkedList<Thought> _thoughts;

    private bool _requestFailed;

    protected override async Task OnInitializedAsync()
    {
        await ComponentRunAsync(async () =>
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
        return await Mediator.Send(new GetJournalEntryRequest
        {
            UserId = await GetUserIdAsync(),
            JournalEntryId = JournalEntryId,
        });
    }

    private async Task UpdateJournalEntryAsync(string value)
    {
        await ComponentRunAsync(async () =>
        {
            var request = new UpdateJournalEntryRequest
            {
                UserId = await GetUserIdAsync(),
                JournalEntryId = JournalEntryId,
                JournalEntry = _journalEntry,
            };

            _journalEntry = await Mediator.Send(request);
        });
    }
}