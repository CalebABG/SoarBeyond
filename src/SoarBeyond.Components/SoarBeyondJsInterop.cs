using Microsoft.JSInterop;

namespace SoarBeyond.Components;

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