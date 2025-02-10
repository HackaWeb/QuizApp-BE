using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Models;
using System.Text.Json;

namespace QuizApp.DataContext.EntityTypeConfiguration;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.Type).IsRequired();
    }
}
