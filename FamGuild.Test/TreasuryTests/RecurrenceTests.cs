using FamGuild.API.Domain.Treasury.Common;

namespace FamGuild.Test.TreasuryTests;

[TestFixture]
public class RecurrenceTests
{
    [Test]
    public void Recurrence_Should_Get_Created_When_Provided_Valid_Info()
    {
        //Arrange
        DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
        Frequencies frequency = Frequencies.Weekly;
        //Act
        var testRecurrence = Recurrence.Create(startDate, endDate:null, frequency);
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(testRecurrence.StartDate, Is.EqualTo(startDate));
            Assert.That(testRecurrence.Frequency, Is.EqualTo(frequency));
        });

    }
}