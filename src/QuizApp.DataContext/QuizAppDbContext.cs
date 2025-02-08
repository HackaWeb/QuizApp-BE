using Microsoft.EntityFrameworkCore;
using QuizApp.Infrastructure;

namespace QuizApp.DataContext;

public class QuizAppDbContext : DbContext, IUnitOfWork
{
    public QuizAppDbContext(DbContextOptions options) 
        : base(options)
    {
    }

    public async Task SaveEntitiesAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
