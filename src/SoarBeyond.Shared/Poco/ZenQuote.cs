using System.Text.Json.Serialization;

namespace SoarBeyond.Shared.Poco;

public class ZenQuote
{
    [JsonPropertyName("q")]
    public string Quote { get; set; } = string.Empty;

    [JsonPropertyName("a")]
    public string Author { get; set; } = string.Empty;

    [JsonIgnore]
    public bool IsDefault { get; set; }
}

/// <summary>
/// Stand in class as implementation of Null Object pattern.
/// If request to api fails, this type of quote will be provided.
/// <see cref="ZenQuote"/>
/// </summary>
public class DefaultZenQuote : ZenQuote
{
    public DefaultZenQuote()
    {
        IsDefault = true;
        Quote = "Yesterday is history, tomorrow is a mystery, but" +
                " today is a gift. That is why it is called present.";
        Author = "Master Oogway";
    }
}