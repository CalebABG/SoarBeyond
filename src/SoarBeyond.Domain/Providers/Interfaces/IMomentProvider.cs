using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Mediator.Moments;

namespace SoarBeyond.Domain.Providers.Interfaces;

public interface IMomentProvider
{
    Task<Moment> CreateAsync(CreateMomentRequest request);
    Task<bool> DeleteAsync(DeleteMomentRequest request);
    Task<Moment> GetAsync(GetMomentRequest request);
    Task<Moment> UpdateAsync(UpdateMomentRequest request);
    Task<IEnumerable<Moment>> GetByJournalIdAsync(GetMomentsFromJournalRequest request);
    Task<IEnumerable<Moment>> GetAllAsync(GetAllMomentsRequest request);
}