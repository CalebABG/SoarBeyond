using Blazored.Toast.Services;
using MediatR;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Components;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Requests.Categories;

namespace SoarBeyond.Web.Pages.Categories;

public partial class Index
{
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private IToastService ToastService { get; set; }

    private bool _showDialog;
    private ConfirmationDialog _confirmationDialog;
    private LinkedList<Category> _categories;

    protected override async Task OnInitializedAsync()
    {
        await ComponentRunAsync(async () =>
        {
            var categories = await GetCategoriesAsync();
            _categories = new LinkedList<Category>(categories
                .OrderByDescending(c => c.CreatedDate));
        });
    }

    private async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        return await Mediator.Send(new GetAllCategoriesRequest(await GetUserIdAsync()));
    }

    private async Task CreateCategoryAsync(Category category)
    {
        await ComponentRunAsync(async () =>
        {
            var request = new CreateCategoryRequest(await GetUserIdAsync(), category);

            var result = await Mediator.Send(request);
            if (result is not null)
            {
                CloseDialog();
                _categories.AddFirst(result);
            }
            else
            {
                ToastService.ShowError("Something went wrong when creating. Please try again.");
            }
        });
    }

    private void OpenDialog() => _showDialog = true;
    private void CloseDialog() => _showDialog = false;
}