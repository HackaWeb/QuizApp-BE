using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using System.Reflection;

namespace QuizApp.DataContext;

public class QuizAppDbContext : IdentityDbContext<User>, IUnitOfWork
{
    public const string DefaultSchemaName = "dbo";

    public QuizAppDbContext(DbContextOptions<QuizAppDbContext> options) 
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
