﻿using Microsoft.EntityFrameworkCore;
using QuizApp.Infrastructure.Extensions;
using QuizApp.Infrastructure.Repositories;
using QuizApp.Infrastructure.Specifications;

namespace QuizApp.DataContext.Repositories;

public class Repository<T>(QuizAppDbContext context) : IRepository<T> where T : class
{
    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Task.Run(() => context.Set<T>().Add(entity), cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await context.Set<T>().AddRangeAsync(entities, cancellationToken);
    }

    public async Task RemoveAsync(Guid id)
    {
        var sql = $"DELETE FROM dbo.\"Quiz\" WHERE \"Id\" = \"{id}\"";
        await context.Database.ExecuteSqlRawAsync(sql, id);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<List<T>> GetByConditionAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => context.Set<T>().Where(predicate).ToList(), cancellationToken);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Set<T>().FindAsync([id], cancellationToken);
    }

    public async Task<List<T>> GetBySpecification(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        return await context.Set<T>()
            .AsQueryable()
            .GetQuery(specification)
            .ToListAsync(cancellationToken);
    }

    public async Task<T?> GetSingleBySpecification(Specification<T> specification, CancellationToken cancellationToken = default)
    {
        return await context.Set<T>()
            .AsQueryable()
            .GetQuery(specification)
            .SingleOrDefaultAsync(cancellationToken);
    }
}
