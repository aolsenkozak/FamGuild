using FamGuild.API.Domain.Common.ResultPattern;
using FamGuild.API.Domain.Treasury.Common;

namespace FamGuild.API.Domain.Treasury;

public class AccountTransaction
{
    public Guid Id { get; private set; }
    public EntryClassification Classification { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Money Amount { get; private set; }
    public string Category { get; private set; } =  string.Empty;
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

    public static Result<AccountTransaction> Create(EntryClassification classification,
        string name, Money amount,
        string category, DateTime dateOccurred, AccountTransactionStatus status)
    {
        if (amount.Value < 0)
        {
            var error = new Error("BadRequest", "Amount must be greater than or equal to zero.");
            return Result.Failure<AccountTransaction>(error);
        }
        
        return Result.Success(new AccountTransaction(
            classification, 
            name, 
            amount, 
            category,
            dateOccurred,
            status
        ));
        
    }
    
    

    private AccountTransaction()
    {
    }
}