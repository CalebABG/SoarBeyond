using Microsoft.AspNetCore.Components;
using SoarBeyond.Domain.Dto;

namespace SoarBeyond.Web.Components;

public partial class CreateCategoryForm
{
    [Parameter] public bool ShowTitle { get; set; } = true;
    [Parameter] public EventCallback<Category> OnValidSubmit { get; set; }

    private Category _category = new();

    public CreateCategoryForm()
    {
        Class = "card shadow";
    }

    private async Task HandleValidSubmitAsync()
    {
        IsBusy = true;
        await OnValidSubmit.InvokeAsync(_category);
        IsBusy = false;
    }

    private void ResetModel()
    {
        _category = new();
    }

    public void ResetForm()
    {
        ResetModel();
    }
}