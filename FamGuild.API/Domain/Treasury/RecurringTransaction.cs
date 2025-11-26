using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Domain.Treasury.Common;

namespace FamGuild.API.Domain.Treasury;

public class RecurringTransaction
{
    public Guid Id { get; private set; }
    public EntryClassification Classification { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Money Amount { get; private set; }
    public string Category { get; private set; } =  string.Empty;
    public Recurrence Recurrence { get; private set; }

    private RecurringTransaction(EntryClassification type, string name, Money amount, string category, Recurrence recurrence)
    {
        Id = Guid.NewGuid();
        Classification = type;
        Name = name;
        Amount = amount;
        Category = category;
        Recurrence = recurrence;
    }

    public static Result<RecurringTransaction> Create(EntryClassification type, string name, 
        Money amount, string category, DateOnly startDate, DateOnly? endDate, Frequencies frequencies)
    {
        if (amount.Value < 0)
        {
            var error = new Error("BadRequest", "Amount must be greater than or equal to zero.");
            return Result.Failure<RecurringTransaction>(error);
        }
        
        var recurrenceResult = Recurrence.Create(startDate, endDate, frequencies);

        if (recurrenceResult.IsFailure)
        {
            return  Result.Failure<RecurringTransaction>(recurrenceResult.Error);
        }
        
        var recurrence = recurrenceResult.Value;

        return Result.Success((new RecurringTransaction(type, name, amount, category, recurrence)));
    }

    private RecurringTransaction()
    {
    }
}