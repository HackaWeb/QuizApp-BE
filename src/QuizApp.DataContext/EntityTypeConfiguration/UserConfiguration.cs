using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Domain.Models;

namespace QuizApp.DataContext.EntityTypeConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Id).HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(u => u.FirstName)
            .IsRequired(false);

        builder
            .Property(u => u.LastName)
            .IsRequired(false);

        builder
            .Property(u => u.AvatarUrl)
            .IsRequired(false);
    }
}
