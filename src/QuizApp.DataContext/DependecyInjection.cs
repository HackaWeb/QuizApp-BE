using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizApp.DataContext.Repositories;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Repositories;

namespace QuizApp.DataContext;

public static class DependecyInjection
{
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
            ?? configuration.GetConnectionString("QuizAppConnection");

        services.AddDbContext<QuizAppDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IUnitOfWork, QuizAppDbContext>();
        services.AddScoped<IBlobStorageRepository, BlobStorageRepository>();

        return services;
    }
}
