using FamGuild.API.Domain.Treasury;
using FamGuild.API.Domain.Treasury.Common;

namespace FamGuild.API.Features.RecurringItems.Create;

public record CreateRecurringItemCommand
{
    public string Name { get; init; }  = string.Empty;
    public RecurringItemType Type { get; init; }
    public Money Amount { get; init; }
    public string Category { get; init; } = string.Empty;
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public Frequencies Frequency { get; init; }
}