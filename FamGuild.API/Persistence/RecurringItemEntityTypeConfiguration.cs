using FamGuild.API.Domain.Treasury;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamGuild.API.Persistence;

public class RecurringItemEntityTypeConfiguration : IEntityTypeConfiguration<RecurringTransaction>
{
    public void Configure(EntityTypeBuilder<RecurringTransaction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(50);

        builder.OwnsOne(x => x.Amount, money =>
        {
            money.Property(m => m.Value).HasColumnName("AmountValue");
            money.Property(m => m.CurrencyCode).HasColumnName("CurrencyCode");
        });

        builder.OwnsOne(x => x.Recurrence, recurrence =>
        {
            recurrence.Property(r => r.StartDate).HasColumnName("StartDate");
            recurrence.Property(r => r.EndDate).HasColumnName("EndDate");
            recurrence.Property(r => r.Frequency)
                .HasColumnName("Frequency");
        });
    }
}