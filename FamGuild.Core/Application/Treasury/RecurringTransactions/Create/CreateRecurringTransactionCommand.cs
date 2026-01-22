namespace FamGuild.Core.Application.Treasury.RecurringTransactions.Create;

public record CreateRecurringTransactionCommand
{
    public string Name { get; } = string.Empty;
    public required string Type { get; init; }
    public required decimal MoneyAmount { get; init; }
    public required string MoneyCurrency { get; init; }
    public string Category { get; init; } = string.Empty;
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public string Frequency { get; init; } =  string.Empty;
}