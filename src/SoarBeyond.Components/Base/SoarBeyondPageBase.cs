using Microsoft.AspNetCore.Components;

namespace SoarBeyond.Components;

public abstract class SoarBeyondPageBase : SoarBeyondComponentBase
{
    [Inject] protected NavigationManager NavigationManager { get; set; }
}