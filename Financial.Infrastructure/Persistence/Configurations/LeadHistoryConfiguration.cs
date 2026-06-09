using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Financial.Infrastructure.Persistence.Configurations;

public class LeadHistoryConfiguration : IEntityTypeConfiguration<LeadHistory>
{
    public void Configure(EntityTypeBuilder<LeadHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.EventType)
            .IsRequired();

        builder.Property(x => x.OldValue)
            .HasMaxLength(500);

        builder.Property(x => x.NewValue)
            .HasMaxLength(500);

        builder.Property(x => x.Comment)
            .HasMaxLength(2000);

        builder.HasOne(x => x.Lead)
            .WithMany(x => x.History)
            .HasForeignKey(x => x.LeadId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}