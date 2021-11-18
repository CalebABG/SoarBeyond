using System.Threading.Tasks;
using SoarBeyond.Domain.MediatR.Thoughts;
using SoarBeyond.Shared.Dto;

namespace SoarBeyond.Domain.Providers.Interfaces
{
    public interface IThoughtProvider
    {
        Task<Thought> CreateAsync(CreateThoughtRequest request);
    }
}