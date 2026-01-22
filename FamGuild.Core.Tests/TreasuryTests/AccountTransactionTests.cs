using FamGuild.Core.Domain.Treasury;

namespace FamGuild.Test.TreasuryTests;

public class AccountTransactionTests
{
    [Test]
    public void LedgerEntry_ShouldGetCreated_WhenProvidedValidRecurringItem()
    {
        var testAmount = (decimal)100.00;
        var testCurrencyCode = "CAD";
        var today = DateOnly.FromDateTime(DateTime.Today);
        var testRecurringTransaction = RecurringTransaction.Create(
            nameof(EntryClassification.Income),
            "testItem",
            testAmount,
            testCurrencyCode,
            "Unknown",
            today,
            null,
            nameof(Frequencies.BiWeekly)
        ).Value;

        var testLedgerEntryResult =
            AccountTransaction.CreateFromRecurringTransaction(testRecurringTransaction,
                DateTime.Now.AddDays(14), AccountTransactionStatus.Pending);

        Assert.That(testLedgerEntryResult.IsSuccess, Is.True);

        var testLedgerEntry = testLedgerEntryResult.Value;

        Assert.Multiple(() =>
        {
            Assert.That(testLedgerEntry.Name, Is.EqualTo(testRecurringTransaction.Name));
            Assert.That(testLedgerEntry.Category, Is.EqualTo(testRecurringTransaction.Category));
            Assert.That(testLedgerEntry.Amount, Is.EqualTo(testRecurringTransaction.Amount));
            Assert.That(testLedgerEntry.Classification, Is.EqualTo(testRecurringTransaction.Classification));
            Assert.That(testLedgerEntry.RecurringTransactionId, Is.EqualTo(testRecurringTransaction.Id));
        });
    }

    [Test]
    public void LedgerEntry_ShouldGetCreated_WhenProvidedValidInformation()
    {
        var testAmount = (decimal)100.00;
        var testCurrencyCode = "CAD";
        var testName = "testExpense";
        var testCategory = "Unknown";
        var transactionStatus = AccountTransactionStatus.Confirmed;
        var entryClassification = EntryClassification.Expense;

        var testLedgerEntryResult =
            AccountTransaction.Create(nameof(EntryClassification.Expense),
                testName, testAmount, testCurrencyCode, testCategory,
                DateTime.Now, nameof(transactionStatus));

        Assert.That(testLedgerEntryResult.IsSuccess, Is.True);

        var testLedgerEntry = testLedgerEntryResult.Value;

        Assert.Multiple(() =>
        {
            Assert.That(testLedgerEntry.Name, Is.EqualTo(testName));
            Assert.That(testLedgerEntry.Category, Is.EqualTo(testCategory));
            Assert.That(testLedgerEntry.Amount.Value, Is.EqualTo(testAmount));
            Assert.That(testLedgerEntry.Classification, Is.EqualTo(entryClassification));
        });
    }
}