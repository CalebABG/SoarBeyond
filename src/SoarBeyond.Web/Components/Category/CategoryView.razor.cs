using Microsoft.AspNetCore.Components;
using SoarBeyond.Domain.Dto;

namespace SoarBeyond.Web.Components;

public partial class CategoryView
{
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Parameter] public Category Category { get; set; }
    [Parameter] public EventCallback<Category> OnDeleteCategory { get; set; }

    public CategoryView()
    {
        Class = "card shadow";
    }

    private async Task DeleteCategoryAsync()
    {
        await OnDeleteCategory.InvokeAsync(Category);
    }
}