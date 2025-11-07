namespace FamGuild.API.Domain.Treasury.Common;

public class Recurrence
{
    public DateOnly StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public Frequencies Frequency { get; private set; }

    private Recurrence(DateOnly startDate, DateOnly? endDate, Frequencies frequency)
    {
        StartDate = startDate;
        EndDate = endDate;
        Frequency = frequency;
    }

    public static Recurrence Create(DateOnly startDate, DateOnly? endDate, Frequencies frequency)
    {
        return new Recurrence(startDate, endDate, frequency);
    }
}