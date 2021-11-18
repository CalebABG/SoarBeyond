using System.Collections.Generic;
using System.Threading.Tasks;
using SoarBeyond.Domain.MediatR.Journals;
using SoarBeyond.Shared.Dto;
using SoarBeyond.Shared.Poco;

namespace SoarBeyond.Domain.Providers.Interfaces
{
    public interface IJournalProvider
    {
        Task<Journal> CreateAsync(CreateJournalRequest request);

        Task<Journal> UpdateAsync(UpdateJournalRequest request);

        Task<bool> DeleteAsync(DeleteJournalRequest request);

        Task<Journal> GetAsync(GetJournalRequest request);

        Task<HashSet<JournalNameId>> GetNameIdsAsync(GetJournalNameIdsRequest request);

        Task<IEnumerable<Journal>> GetAllAsync(GetAllJournalsRequest request);
    }
}