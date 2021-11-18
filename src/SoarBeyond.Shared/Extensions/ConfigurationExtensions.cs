using Microsoft.Extensions.Configuration;
using SoarBeyond.Shared.ConfigurationOptions;

namespace SoarBeyond.Shared.Extensions;

public static class ConfigurationExtensions
{
    /// <summary>
    /// An extension method to get a <see cref="SoarBeyondConfigurationOptions" />
    /// instance from the Configuration by Section Key
    /// </summary>
    /// <param name="configuration">the configuration</param>
    /// <param name="configurationSection">the configuration section to use</param>
    /// <returns>an instance of the defined configuration options class, or null if not found</returns>
    public static SoarBeyondConfigurationOptions GetSoarBeyondConfigurationOptions(
        this IConfiguration configuration,
        IConfigurationSection configurationSection = null)
    {
        var soarBeyondSection = configurationSection ?? GetSoarBeyondConfigurationOptionsSection(configuration);
        return soarBeyondSection.Get<SoarBeyondConfigurationOptions>();
    }

    public static IConfigurationSection GetSoarBeyondConfigurationOptionsSection(this IConfiguration configuration)
    {
        return configuration.GetSection(SoarBeyondConfigurationOptions.SectionKey);
    }
}