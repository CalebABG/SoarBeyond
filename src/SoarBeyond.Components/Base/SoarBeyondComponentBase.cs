using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace SoarBeyond.Components;

public abstract class SoarBeyondComponentBase : ComponentBase
{
    private const int DebugTaskDelayTime = 150;

    [Parameter] public string Class { get; set; } = string.Empty;
    [Parameter] public string Style { get; set; } = string.Empty;
    [Parameter] public string ExtraClass { get; set; } = string.Empty;
    [Parameter] public string ExtraStyle { get; set; } = string.Empty;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> ComponentAttributes { get; set; } = new();

    protected bool IsBusy { get; set; }

    protected virtual CssBuilder ComponentClassesBuilder
        => new CssBuilder()
            .AddClass(Class)
            .AddClass(ExtraClass);

    protected virtual StyleBuilder ComponentStylesBuilder
        => new StyleBuilder()
            .AddStyle(Style)
            .AddStyle(ExtraStyle);

    protected string ComponentClasses => ComponentClassesBuilder.Build();
    protected string ComponentStyles => ComponentStylesBuilder.Build(true);
    
    protected async Task ComponentRunAsync
    (
        Func<Task> func,
        bool callStateHasChanged = true,
        ILogger<SoarBeyondComponentBase> logger = null
    )
    {
        if (IsBusy) return;

        IsBusy = true;

        try
        {
#if DEBUG
            await Task.Delay(DebugTaskDelayTime);
#endif
            await func.Invoke();
        }
        catch (Exception e)
        {
            logger?.LogError(e, "{Message}", e.Message);
        }
        finally
        {
            IsBusy = false;
            if (callStateHasChanged) 
                StateHasChanged();
        }
    }
}

public static class BlazorComponentUtilitiesExtensions
{
    public static string Build(this StyleBuilder styleBuilder, bool removeTrailingSemicolon = false) 
        => removeTrailingSemicolon ? styleBuilder.Build().TrimEnd(';') : styleBuilder.Build();
}