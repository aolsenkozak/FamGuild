using FamGuild.Domain.Treasury.Common;

namespace FamGuild.Shared.Treasury.Features.RecurringTransactions;

public record CreateRecurringTransactionCommand
{
    public string Name { get; } = string.Empty;
    public required EntryClassification Type { get; init; }
    public required Money Amount { get; init; }
    public string Category { get; init; } = string.Empty;
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public Frequencies Frequency { get; init; }
}