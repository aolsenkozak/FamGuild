using FamGuild.Domain.Treasury;
using FamGuild.Domain.Treasury.Common;

namespace FamGuild.Test.TreasuryTests;

public class AccountTransactionTests
{
    [Test]
    public void LedgerEntry_ShouldGetCreated_WhenProvidedValidRecurringItem()
    {
        var testAmount = new Money((decimal)100.00, "CAD");
        var today = DateOnly.FromDateTime(DateTime.Today);
        var testRecurringTransaction = RecurringTransaction.Create(
            EntryClassification.Income,
            "testItem",
            testAmount,
            "Unknown",
            today,
            null,
            Frequencies.BiWeekly
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
        var testAmount = new Money((decimal)100.00, "CAD");
        var testName = "testExpense";
        var testCategory = "Unknown";
        var transactionStatus = AccountTransactionStatus.Confirmed;
        var entryClassification = EntryClassification.Expense;

        var testLedgerEntryResult =
            AccountTransaction.Create(EntryClassification.Expense,
                testName, testAmount, testCategory,
                DateTime.Now, transactionStatus);

        Assert.That(testLedgerEntryResult.IsSuccess, Is.True);

        var testLedgerEntry = testLedgerEntryResult.Value;

        Assert.Multiple(() =>
        {
            Assert.That(testLedgerEntry.Name, Is.EqualTo(testName));
            Assert.That(testLedgerEntry.Category, Is.EqualTo(testCategory));
            Assert.That(testLedgerEntry.Amount, Is.EqualTo(testAmount));
            Assert.That(testLedgerEntry.Classification, Is.EqualTo(entryClassification));
        });
    }
}