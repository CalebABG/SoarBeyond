using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Requests.Categories;

public sealed record CreateCategoryRequest(int UserId, Category Category)
    : IRequest<Category>;

internal sealed class CreateCategoryRequestHandler
    : IRequestHandler<CreateCategoryRequest, Category>
{
    private readonly ICategoryProvider _categoryProvider;

    public CreateCategoryRequestHandler(ICategoryProvider categoryProvider)
    {
        _categoryProvider = categoryProvider;
    }

    public async Task<Category> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        return await _categoryProvider.CreateAsync(request);
    }
}