using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace SoarBeyond.Components;
// This class provides an example of how JavaScript functionality can be wrapped
// in a .NET class for easy consumption. The associated JavaScript module is
// loaded on demand when first needed.
//
// This class can be registered as scoped DI service and then injected into Blazor
// components for use.

/* TODO: *NOTE*: Window 10 EmojiKeyboard = 'WinKey + Period' */
public class SoarBeyondJsInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public SoarBeyondJsInterop(IJSRuntime jsRuntime)
    {
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/SoarBeyond.Components/soarbeyond.js").AsTask());
    }

    public async ValueTask<string> Prompt(string message)
    {
        var module = await _moduleTask.Value;
        return await module.InvokeAsync<string>("soarBeyond.showPrompt", message);
    }

    public async ValueTask OpenInNewTab(string url)
    {
        var module = await _moduleTask.Value;
        await module.InvokeAsync<object>("open", url, "_blank");
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}