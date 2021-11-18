using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SoarBeyond.Domain.Services.Interfaces;
using SoarBeyond.Shared.Extensions;
using SoarBeyond.Shared.Poco;

namespace SoarBeyond.Web.Pages;

public partial class Index
{
    [Inject] public IZenQuoteService QuoteService { get; set; }
    [CascadingParameter] public Task<AuthenticationState> AuthStateTask { get; set; }

    private ZenQuote _quote = new DefaultZenQuote();

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateTask;
        var authenticated = authState.User.IsAuthenticated();
        if (authenticated) _quote = await QuoteService.GetQuoteOfTheDayAsync();
    }
}