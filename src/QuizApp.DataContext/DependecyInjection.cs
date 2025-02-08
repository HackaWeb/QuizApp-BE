using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizApp.DataContext.Repositories;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Repositories;

namespace QuizApp.DataContext;

public static class DependecyInjection
{
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("QuizAppConnection");

        services.AddDbContext<QuizAppDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IUnitOfWork, QuizAppDbContext>();
        services.AddScoped<IBlobStorageRepository, BlobStorageRepository>();

        return services;
    }
}
