using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Requests.Journals;
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
    Task<IEnumerable<Journal>> GetFavoritesAsync(GetFavoriteJournalsRequest request);
    Task<bool> UpdateFavoriteStatusAsync(UpdateJournalFavoriteStatusRequest request);
}