using MediatR;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Mediator.Moments;

namespace SoarBeyond.Web.Pages.Moments;

public partial class View
{
    [Inject] private IMediator Mediator { get; set; }
    [Parameter] public int MomentId { get; set; }

    private bool _requestFailed;

    private Moment _moment;
    private LinkedList<Note> _notes;

    protected override async Task OnInitializedAsync()
    {
        await ComponentRunAsync(async () =>
        {
            _requestFailed = false;

            var moment = await GetMomentFromDbAsync();
            if (moment is not null)
            {
                _moment = moment;
                _notes = new LinkedList<Note>(_moment.Notes
                    .OrderByDescending(n => n.CreatedDate)
                    .ToList());
            }
            else
            {
                _requestFailed = true;
            }
        });
    }

    private async Task<Moment> GetMomentFromDbAsync()
    {
        return await Mediator.Send(new GetMomentRequest(await GetUserIdAsync(), MomentId));
    }

    private async Task UpdateMomentAsync(string value)
    {
        await ComponentRunAsync(async () =>
        {
            var request = new UpdateMomentRequest(await GetUserIdAsync(), _moment);
            _moment = await Mediator.Send(request);
        });
    }
}