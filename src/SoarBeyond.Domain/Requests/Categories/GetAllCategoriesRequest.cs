using MediatR;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;

namespace SoarBeyond.Domain.Requests.Categories;

public sealed record GetAllCategoriesRequest(int UserId)
    : IRequest<IEnumerable<Category>>;

internal sealed class GetAllCategoriesRequestHandler
    : IRequestHandler<GetAllCategoriesRequest, IEnumerable<Category>>
{
    private readonly ICategoryProvider _categoryProvider;

    public GetAllCategoriesRequestHandler(ICategoryProvider categoryProvider)
    {
        _categoryProvider = categoryProvider;
    }

    public async Task<IEnumerable<Category>> Handle(GetAllCategoriesRequest request, CancellationToken cancellationToken)
    {
        return await _categoryProvider.GetAllAsync(request);
    }
}