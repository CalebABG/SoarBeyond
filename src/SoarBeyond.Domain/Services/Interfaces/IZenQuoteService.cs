using System.Threading.Tasks;
using SoarBeyond.Shared.Poco;

namespace SoarBeyond.Domain.Services.Interfaces;

public interface IZenQuoteService
{
    Task<ZenQuote> GetQuoteOfTheDayAsync();
    Task<ZenQuote> GetRandomQuoteAsync();
    Task<ZenQuote> GetDefaultQuoteAsync();
}