using FamGuild.API.Common.ResultPattern;
using FamGuild.Domain.Treasury.Common;

namespace FamGuild.API.Treasury.RecurringTransactions;

public class RecurringTransaction
{
    private RecurringTransaction(EntryClassification type, string name, Money amount, string category,
        Recurrence recurrence)
    {
        Id = Guid.NewGuid();
        Classification = type;
        Name = name;
        Amount = amount;
        Category = category;
        Recurrence = recurrence;
    }

    private RecurringTransaction()
    {
    }

    public Guid Id { get; private set; }
    public EntryClassification Classification { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Money Amount { get; private set; }
    public string Category { get; private set; } = string.Empty;
    public Recurrence Recurrence { get; private set; }

    public static Result<RecurringTransaction> Create(string type, string name,
        decimal amount, string currencyCode, string category, DateOnly startDate, 
        DateOnly? endDate, string frequencyString)
    {
        if (amount < 0)
        {
            var error = new Error("BadRequest", "Amount must be greater than or equal to zero.");
            return Result.Failure<RecurringTransaction>(error);
        }

        var entryClassification = Enum.TryParse<EntryClassification>(type, out var classification);
        
        var money = new Money(amount, currencyCode);
        
        var frequencyParseResult = Enum.TryParse<Frequencies>(frequencyString, out var frequency);
        
        var recurrenceResult = Recurrence.Create(startDate, endDate, frequency);

        if (recurrenceResult.IsFailure) return Result.Failure<RecurringTransaction>(recurrenceResult.Error);

        var recurrence = recurrenceResult.Value;
        


        return Result.Success(new RecurringTransaction(classification, name, money, category, recurrence));
    }
}