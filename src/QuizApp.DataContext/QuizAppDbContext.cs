using Microsoft.EntityFrameworkCore;
using QuizApp.Infrastructure;
using System.Reflection;

namespace QuizApp.DataContext;

public class QuizAppDbContext : DbContext, IUnitOfWork
{
    public const string DefaultSchemaName = "dbo";

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
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.HasDefaultSchema(DefaultSchemaName);

        base.OnModelCreating(modelBuilder);
    }
}
