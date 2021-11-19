using SoarBeyond.Domain.MediatR.JournalEntries;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Domain.Providers.Interfaces
{
    public interface IJournalEntryProvider
    {
        Task<JournalEntry> CreateAsync(CreateJournalEntryRequest request);
        Task<bool> DeleteAsync(DeleteJournalEntryRequest request);
        Task<JournalEntry> GetAsync(GetJournalEntryRequest request);
        Task<JournalEntry> UpdateAsync(UpdateJournalEntryRequest request);
        Task<IEnumerable<JournalEntry>> GetByJournalIdAsync(GetJournalEntriesByJournalIdRequest request);
        Task<IEnumerable<JournalEntry>> GetAllAsync(GetAllJournalEntriesRequest request);
    }
}