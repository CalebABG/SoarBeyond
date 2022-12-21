using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Components;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Requests.Moments;

namespace SoarBeyond.Web.Pages.Moments;

public partial class Index
{
    [Inject] private IMediator Mediator { get; set; }

    private ConfirmationDialog _confirmationDialog;

    private LinkedList<Moment> _moments;

    protected override async Task OnInitializedAsync()
    {
        await ComponentRunAsync(async () =>
        {
            var moments = await GetDbMomentsAsync();
            _moments = new LinkedList<Moment>(moments
                .OrderByDescending(dto => dto.CreatedDate));
        });
    }

    private async Task<IEnumerable<Moment>> GetDbMomentsAsync()
    {
        return await Mediator.Send(new GetAllMomentsRequest(await GetUserIdAsync()));
    }

    private async Task DeleteMomentAsync(Moment moment)
    {
        var result = await _confirmationDialog.ShowAsync("Delete?", moment.Title.Truncate(50));

        if (result)
        {
            await ComponentRunAsync(async () =>
            {
                var request = new DeleteMomentRequest(await GetUserIdAsync(), moment.Id);

                bool deleted = await Mediator.Send(request);
                if (deleted) _moments.Remove(moment);
            });
        }
    }
}