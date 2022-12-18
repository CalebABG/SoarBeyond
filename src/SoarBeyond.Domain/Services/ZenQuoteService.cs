using System.Text.Json;
using System.Text.Json.Serialization;
using SoarBeyond.Domain.Services.Interfaces;
using SoarBeyond.Shared.Poco;

namespace SoarBeyond.Domain.Services;

public class ZenQuoteService : IZenQuoteService
{
    private const string QuoteApiBaseUrl = "https://zenquotes.io/api";
    private const string QuoteOfTheDayUrl = $"{QuoteApiBaseUrl}/today";
    private const string RandomQuoteUrl = $"{QuoteApiBaseUrl}/random";

    private const byte ZenQuoteApiMaxRequests = 5;
    private static readonly TimeSpan ApiRateLimitTimeout = TimeSpan.FromSeconds(30);

    private ZenQuote _quoteOfTheDay;
    private DateTimeOffset _nextQuoteOfTheDayFetchTime;

    private byte _requestCount;
    private DateTimeOffset _nextRequestTime;

    private readonly IHttpClientFactory _clientFactory;

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
    };

    public ZenQuoteService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<ZenQuote> GetQuoteOfTheDayAsync()
    {
        // We haven't fetched the quote of the day
        if (_quoteOfTheDay is null)
        {
            await FetchQuoteOfTheDayAsync();
        }
        else
        {
            // Next day, fetch another days quote
            if (_nextQuoteOfTheDayFetchTime < DateTimeOffset.UtcNow)
                await FetchQuoteOfTheDayAsync();
        }

        return _quoteOfTheDay;
    }

    private async Task FetchQuoteOfTheDayAsync()
    {
        _quoteOfTheDay = await GetZenQuoteAsync(QuoteOfTheDayUrl);
        UpdateQuoteOfTheDayFetchTime();
    }

    private void UpdateQuoteOfTheDayFetchTime()
    {
        _nextQuoteOfTheDayFetchTime = DateTimeOffset.UtcNow + TimeSpan.FromDays(1);
    }

    public async Task<ZenQuote> GetRandomQuoteAsync()
    {
        return await SendRateLimitedRequest(async () => await GetZenQuoteAsync(RandomQuoteUrl))
               ?? new DefaultZenQuote();
    }

    public Task<ZenQuote> GetDefaultQuoteAsync()
        => Task.FromResult<ZenQuote>(new DefaultZenQuote());

    private async Task<ZenQuote> GetZenQuoteAsync(string url)
    {
        ZenQuote zenQuote = new DefaultZenQuote();

        var responseMessage = await SendRequestAsync(url);
        if (!responseMessage.IsSuccessStatusCode)
            return zenQuote;

        var content = await responseMessage.Content.ReadAsStringAsync();
        var quotes = Deserialize<ZenQuote[]>(content);
        var quote = quotes[0];

        if (!TooManyRequestsMade(quote))
            zenQuote = quote;

        return zenQuote;
    }

    private static bool TooManyRequestsMade(ZenQuote quoteResponse)
        => quoteResponse.Quote.Contains("too many requests", StringComparison.OrdinalIgnoreCase);

    private async Task<T> SendRateLimitedRequest<T>(Func<Task<T>> rateLimitedRequest)
    {
        T result = default!;

        if (_requestCount is 0 or < ZenQuoteApiMaxRequests)
        {
            _requestCount++;
            result = await rateLimitedRequest.Invoke();
        }
        else
        {
            if (_nextRequestTime == DateTimeOffset.MinValue)
                _nextRequestTime = DateTimeOffset.UtcNow + ApiRateLimitTimeout;

            if (_nextRequestTime >= DateTimeOffset.UtcNow)
                return result;

            _requestCount = 1;
            result = await rateLimitedRequest.Invoke();
            _nextRequestTime = DateTimeOffset.MinValue;
        }

        return result;
    }

    private async Task<HttpResponseMessage> SendRequestAsync(string url)
    {
        using var client = _clientFactory.CreateClient();
        var response = await client.GetAsync(url);
        return response;
    }

    private static T Deserialize<T>(string content)
    {
        return JsonSerializer.Deserialize<T>(content, SerializerOptions);
    }
}