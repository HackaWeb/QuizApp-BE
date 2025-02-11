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

    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }
    public DbSet<Feedback> Feedback { get; set; }

    public DbSet<QuizHistory> QuizHistory { get; set; }

    public IRepository<Question> QuestionsRepository => new Repository<Question>(this);

    public IOptionRepository OptionsRepository => new OptionRepository(this);

    public IQuizHistoryRepository QuizHistoryRepository => new QuizHistoryRepository(this);

    public IQuizRepository QuizRepository => new QuizRepository(this);

    public IQuestionRepository QuestionRepository => new QuestionRepository(this);

    public IFeedbackRepository FeedbackRepository => new FeedbackRepository(this);

    public async Task SaveEntitiesAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.HasDefaultSchema(DefaultSchemaName);

        modelBuilder.Entity<Question>()
            .HasOne(q => q.Quiz)
            .WithMany(q => q.Questions)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AnswerOption>()
            .HasOne(a => a.Question)
            .WithMany(q => q.ChoiceOptions)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Quiz)
            .WithMany(q => q.Feedbacks)
            .HasForeignKey(f => f.QuizId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}
