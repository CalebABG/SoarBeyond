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

            var moment = await GetMomentFromDb();
            if (moment is not null)
            {
                _moment = moment;
                _notes = new LinkedList<Note>(_moment.Notes
                    .OrderByDescending(je => je.CreatedDate).ToList());
            }
            else
            {
                _requestFailed = true;
            }
        });
    }

    private async Task<Moment> GetMomentFromDb()
    {
        return await Mediator.Send(new GetMomentRequest
        {
            UserId = await GetUserIdAsync(),
            MomentId = MomentId,
        });
    }

    private async Task UpdateMomentAsync(string value)
    {
        await ComponentRunAsync(async () =>
        {
            var request = new UpdateMomentRequest
            {
                UserId = await GetUserIdAsync(),
                MomentId = MomentId,
                Moment = _moment,
            };

            _moment = await Mediator.Send(request);
        });
    }
}