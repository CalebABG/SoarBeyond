using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Mediator.Journals;
using SoarBeyond.Shared.Poco;

namespace SoarBeyond.Domain.Providers.Interfaces;

public interface IJournalProvider
{
    Task<Journal> CreateAsync(CreateJournalRequest request);
    Task<Journal> UpdateAsync(UpdateJournalRequest request);
    Task<bool> DeleteAsync(DeleteJournalRequest request);
    Task<Journal> GetAsync(GetJournalRequest request);
    Task<IEnumerable<JournalName>> GetNamesAsync(GetJournalNamesRequest request);
    Task<IEnumerable<Journal>> GetAllAsync(GetAllJournalsRequest request);
}