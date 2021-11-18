using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorComponentUtilities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace SoarBeyond.Components;

public class SoarBeyondComponentBase : OwningComponentBase
{
    private const int DebugTaskDelayTime = 150;

    [Parameter] public string Class { get; set; } = string.Empty;
    [Parameter] public string Style { get; set; } = string.Empty;
    [Parameter] public string ExtraClass { get; set; } = string.Empty;
    [Parameter] public string ExtraStyle { get; set; } = string.Empty;

    [Parameter] public object Tag { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> ComponentAttributes { get; set; } = new();

    protected bool IsBusy { get; set; }

    protected virtual string ComponentClasses
        => new CssBuilder()
            .AddClass(Class)
            .AddClass(ExtraClass)
            .Build();

    protected virtual string ComponentStyles
        => new StyleBuilder()
            .AddStyle(Style)
            .AddStyle(ExtraStyle)
            .Build();

    protected async Task BeyondComponentRunAsync(
        Func<Task> func,
        bool callStateHasChanged = true,
        ILogger<SoarBeyondComponentBase> logger = null)
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
                await InvokeAsync(StateHasChanged);
        }
    }
}