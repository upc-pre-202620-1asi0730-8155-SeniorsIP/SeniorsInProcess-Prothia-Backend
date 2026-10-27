namespace Prothia.API.Analytics.Domain.Model.ValueObjects;

/// <summary>
/// Encapsulates the time window evaluated in a clinical biomechanical assessment.
/// </summary>
public sealed record EvaluationPeriod
{
    public DateOnly StartDate { get; }
    public DateOnly EndDate { get; }

    public EvaluationPeriod(DateOnly startDate, DateOnly endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException($"Start date '{startDate:dd/MM/yyyy}' cannot be after end date '{endDate:dd/MM/yyyy}'.", nameof(startDate));

        StartDate = startDate;
        EndDate = endDate;
    }

    public int TotalDays => EndDate.DayNumber - StartDate.DayNumber + 1;

    public bool Includes(DateOnly date) => date >= StartDate && date <= EndDate;

    public override string ToString() => $"{StartDate:dd/MM/yyyy} - {EndDate:dd/MM/yyyy}";
}