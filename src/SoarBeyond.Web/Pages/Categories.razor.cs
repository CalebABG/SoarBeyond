using Blazored.Toast.Services;
using MediatR;
using Microsoft.AspNetCore.Components;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Requests.Categories;

namespace SoarBeyond.Web.Pages;

public partial class Categories
{
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private IToastService ToastService { get; set; }

    private LinkedList<Category> _categories;

    protected override async Task OnInitializedAsync()
    {
        await ComponentRunAsync(async () =>
        {
            var categories = await GetCategoriesFromDbAsync();
            _categories = new LinkedList<Category>(categories
                .OrderByDescending(c => c.CreatedDate));
        });
    }

    private async Task<IEnumerable<Category>> GetCategoriesFromDbAsync()
    {
        return await Mediator.Send(new GetAllCategoriesRequest());
    }
}