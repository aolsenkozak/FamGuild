using FamGuild.Core.Domain.Treasury;

namespace FamGuild.Test.TreasuryTests;

[TestFixture]
public class RecurrenceTests
{
    [Test]
    public void Recurrence_Should_Get_Created_When_Provided_Valid_Info()
    {
        //Arrange
        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var frequency = Frequencies.Weekly;
        //Act
        var testRecurrenceResult = Recurrence.Create(startDate, null, frequency);
        var testRecurrence = testRecurrenceResult.Value;
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(testRecurrenceResult.IsSuccess, Is.True);
            Assert.That(testRecurrence.StartDate, Is.EqualTo(startDate));
            Assert.That(testRecurrence.Frequency, Is.EqualTo(frequency));
        });
    }

    private static object[] earlyEndDateTestCases =
    {
        new object[] { Frequencies.Weekly, 6 },
        new object[] { Frequencies.BiWeekly, 10 },
        new object[] { Frequencies.Monthly, 25 },
        new object[] { Frequencies.Quarterly, 70 }
    };

    [TestCaseSource(nameof(earlyEndDateTestCases))]
    public void Recurrence_Should_Return_Error_When_End_Date_Earlier_Than_Frequency(Frequencies frequency,
        int daysToAdd)
    {
        //Arrange
        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var endDate = startDate.AddDays(daysToAdd);

        //Act
        var recurrenceResult = Recurrence.Create(startDate, endDate, frequency);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(recurrenceResult.IsFailure, Is.True);
            Assert.That(recurrenceResult.Error.Message, Is.Not.Empty);
        });
    }
}