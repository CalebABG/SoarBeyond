using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SoarBeyond.Data;
using SoarBeyond.Data.Entities;
using SoarBeyond.Domain.Dto;
using SoarBeyond.Domain.Providers.Interfaces;
using SoarBeyond.Domain.Requests.Categories;

namespace SoarBeyond.Domain.Providers;

public class DbCategoryProvider : ICategoryProvider
{
    private readonly IMapper _mapper;
    private readonly IDbContextFactory<SoarBeyondDbContext> _contextFactory;

    public DbCategoryProvider
    (
        IMapper mapper,
        IDbContextFactory<SoarBeyondDbContext> contextFactory)
    {
        _mapper = mapper;
        _contextFactory = contextFactory;
    }

    public async Task<Category> CreateAsync(CreateCategoryRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var dbCategory = await context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => string.Equals(c.Name.ToLower(), request.Category.Name.ToLower(), StringComparison.Ordinal));

        if (dbCategory is not null)
            return null;

        var mappedCategory = _mapper.Map<Category, CategoryEntity>(request.Category);
        mappedCategory.UserId = request.UserId;

        var addedEntry = context.Categories.Add(mappedCategory);
        await context.SaveChangesAsync();

        return _mapper.Map<CategoryEntity, Category>(addedEntry.Entity);
    }

    public async Task<IEnumerable<Category>> GetAllAsync(GetAllCategoriesRequest request)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var categories = context.Categories
            .AsNoTracking()
            .Where(c => c.UserId == request.UserId)
            .ProjectTo<Category>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return await categories;
    }
}