using FamGuild.API.Common.ResultPattern;
using FamGuild.API.Treasury.RecurringTransactions;
using FamGuild.Domain.Treasury.Common;

namespace FamGuild.API.Treasury.AccountTransactions;

public class AccountTransaction
{
    public Guid Id { get; private set; }
    public EntryClassification Classification { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Money Amount { get; private set; }
    public string Category { get; private set; } = string.Empty;
    public DateTime DateOccurred { get; private set; }
    public AccountTransactionStatus Status { get; private set; } = AccountTransactionStatus.Pending;
    public Guid? RecurringTransactionId { get; private set; }
    
    private AccountTransaction(EntryClassification classification, string name, Money amount,
        string category, DateTime dateOccurred, AccountTransactionStatus status,
        Guid? recurringTransactionId = null)
    {
        Id = Guid.NewGuid();
        Classification = classification;
        Name = name;
        Amount = amount;
        Category = category;
        DateOccurred = dateOccurred;
        Status = status;
        RecurringTransactionId = recurringTransactionId;
    }


    private AccountTransaction()
    {
    }



    public static Result<AccountTransaction> CreateFromRecurringTransaction(
        RecurringTransaction recurringTransaction,
        DateTime dateOccurred, AccountTransactionStatus status)
    {
        return Result.Success(new AccountTransaction(
            recurringTransaction.Classification,
            recurringTransaction.Name,
            recurringTransaction.Amount,
            recurringTransaction.Category,
            dateOccurred,
            status,
            recurringTransaction.Id
        ));
    }

    public static Result<AccountTransaction> Create(string classification,
        string name, decimal amount, string currencyCode,
        string category, DateTime dateOccurred, string status)
    {
        if (amount < 0)
        {
            var error = new Error("BadRequest", "Amount must be greater than or equal to zero.");
            return Result.Failure<AccountTransaction>(error);
        }
        
        var classificationEnumResult = Enum.TryParse<EntryClassification>(
            classification, out var entryClassification);

        var money = new Money(amount, currencyCode);
        
        var statusEnumResult = Enum.TryParse<AccountTransactionStatus>(
            status, out var accountStatus);

        return Result.Success(new AccountTransaction(
            entryClassification,
            name,
            money,
            category,
            dateOccurred,
            accountStatus
        ));
    }
}