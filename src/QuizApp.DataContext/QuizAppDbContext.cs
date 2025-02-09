using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizApp.DataContext.Repositories;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Repositories;
using System.Reflection;

namespace QuizApp.DataContext;

public class QuizAppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    public const string DefaultSchemaName = "dbo";

    public QuizAppDbContext(DbContextOptions<QuizAppDbContext> options) 
        : base(options)
    {
    }

    public IRepository<Quiz> QuizRepository => new Repository<Quiz>(this);

    public IRepository<QuizHistory> QuizHistoryRepository => new Repository<QuizHistory>(this);

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
