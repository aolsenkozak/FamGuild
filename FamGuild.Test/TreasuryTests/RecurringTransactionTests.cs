using FamGuild.API.Domain.Treasury;
using FamGuild.API.Domain.Treasury.Common;

namespace FamGuild.Test.TreasuryTests;

public class RecurringTransactionTests
{
    [Test]
    public void RecurringItem_Should_Create_When_Provided_Valid_Information()
    {
        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var endDate = startDate.AddDays(14);
        var frequency = Frequencies.BiWeekly;
        var itemType = EntryClassification.Income;
        var testName = "TestIncomeItem";
        var testCategory = "TestCategory";

        var testAmount = new Money((decimal)100.00, "CAD");
        
        var recurringItemResult = RecurringTransaction.Create(itemType, testName, 
            testAmount, testCategory, startDate, endDate, frequency);
        
        Assert.That(recurringItemResult.IsSuccess, Is.True);
        
        var  recurringItem = recurringItemResult.Value;

        Assert.Multiple(() =>
        {
            Assert.That(recurringItem.Name, Is.EqualTo(testName));
            Assert.That(recurringItem.Category, Is.EqualTo(testCategory));
            Assert.That(recurringItem.Amount, Is.EqualTo(testAmount));
            Assert.That(recurringItem.Recurrence.StartDate, Is.EqualTo(startDate));
            Assert.That(recurringItem.Recurrence.EndDate, Is.EqualTo(endDate));
            Assert.That(recurringItem.Recurrence.Frequency, Is.EqualTo(frequency));
        });
    }
}