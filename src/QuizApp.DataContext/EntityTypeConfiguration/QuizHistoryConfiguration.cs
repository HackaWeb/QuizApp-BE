using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Models;

namespace QuizApp.DataContext.EntityTypeConfiguration;

public class QuizHistoryConfiguration : IEntityTypeConfiguration<QuizHistory>
{
    public void Configure(EntityTypeBuilder<QuizHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(f => f.StartedAt).IsRequired();
        builder.Property(f => f.FinishedAt).IsRequired();
        builder.Property(f => f.UserId).IsRequired();
    }
}
