using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Requests.Categories;

namespace SoarBeyond.Web.Components;

public partial class CreateJournalForm
{
    [Inject] public IMediator Mediator { get; set; }
    [Parameter] public bool ShowTitle { get; set; } = true;
    [Parameter] public EventCallback<Journal> OnValidSubmit { get; set; }

    private Journal _journal = new();
    private List<Category> _categories;

    public CreateJournalForm()
    {
        Class = "card shadow";
    }

    protected override async Task OnInitializedAsync()
    {
        _categories = (await Mediator.Send(new GetAllCategoriesRequest(await GetUserIdAsync()))).ToList();
    }

    private async Task HandleValidSubmitAsync()
    {
        IsBusy = true;
        await OnValidSubmit.InvokeAsync(_journal);
        IsBusy = false;
    }

    private void ResetModel()
    {
        _journal = new();
    }

    public void ResetForm()
    {
        ResetModel();
    }

    private void HandleCategorySelected(ChangeEventArgs args)
    {
        int value = int.Parse(args.Value!.ToString()!);
        _journal.CategoryId = value < 1 ? null : value;
    }
}