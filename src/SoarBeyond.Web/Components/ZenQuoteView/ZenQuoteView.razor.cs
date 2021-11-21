using Microsoft.AspNetCore.Components;
using SoarBeyond.Shared.Poco;

namespace SoarBeyond.Web.Components;

public partial class ZenQuoteView
{
    [Parameter] public ZenQuote Quote { get; set; }

    public ZenQuoteView()
    {
        Class = "display-6 text-light font-italic";
    }
}