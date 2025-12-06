using FamGuild.Domain.Common.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace FamGuild.Domain.Treasury.Common;

[Owned]
public record Recurrence
{
    private Recurrence(DateOnly startDate, DateOnly? endDate, Frequencies frequency)
    {
        StartDate = startDate;
        EndDate = endDate;
        Frequency = frequency;
    }

    private Recurrence()
    {
    }

    public DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public Frequencies Frequency { get; init; }

    public static Result<Recurrence> Create(DateOnly startDate, DateOnly? endDate, Frequencies frequency)
    {
        if (endDate is not null &&
            IsEndDateEarlierThanFrequencyDate(startDate, endDate!.Value, frequency))
        {
            var error = new Error("BadRequest", "End date needs to be later than the frequency date");
            return Result.Failure<Recurrence>(error);
        }

        return Result.Success(new Recurrence(startDate, endDate, frequency));
    }

    private static bool IsEndDateEarlierThanFrequencyDate(DateOnly startDate, DateOnly endDate, Frequencies frequency)
    {
        return frequency switch
        {
            Frequencies.Weekly => endDate < startDate.AddDays(7),
            Frequencies.BiWeekly => endDate < startDate.AddDays(14),
            Frequencies.Monthly => endDate < startDate.AddMonths(1),
            Frequencies.Quarterly => endDate < startDate.AddMonths(3),
            Frequencies.Yearly => endDate < startDate.AddYears(1),
            _ => false
        };
    }
}