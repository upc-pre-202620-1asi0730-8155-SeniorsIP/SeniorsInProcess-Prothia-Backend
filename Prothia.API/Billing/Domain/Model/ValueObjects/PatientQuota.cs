namespace Prothia.API.Billing.Domain.Model.ValueObjects;

public sealed record PatientQuota
{
    public int UsedQuota { get; }
    
    public int TotalQuota { get; }

    public PatientQuota(int usedQuota, int totalQuota)
    {
        if (totalQuota <= 0)
            throw new ArgumentOutOfRangeException(nameof(totalQuota), totalQuota, "Total quota must be greater than zero.");
        
        if(usedQuota < 0)
            throw  new ArgumentOutOfRangeException(nameof(usedQuota), usedQuota, "Used quota cannot be negative.");
        
        if (usedQuota > totalQuota)
            throw new ArgumentException($"Used quota ({usedQuota}) cannot exceed total cantracted quota ({totalQuota}.", nameof(usedQuota));
        UsedQuota = usedQuota;
        TotalQuota = totalQuota;
    }
    
    public int AvaibleQuota => Math.Max(0, TotalQuota - UsedQuota);
    
    public int OccupancyPercentage => (int)Math.Round((double)UsedQuota / TotalQuota * 100);
    
    public bool IsNearCapacity => OccupancyPercentage >= 80;
    public bool IsFull => UsedQuota >= TotalQuota;

    public PatientQuota IncrementUSed(int count = 1)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Increment count must be greater than zero.");
        return new PatientQuota(UsedQuota + count, TotalQuota);
        
    }

    public PatientQuota DecrementUSed(int count = 1)
    {
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count), count, "Decrement count must be greater than zero.");
        return new PatientQuota(Math.Max(0, UsedQuota - count), TotalQuota);
    }
    
    public override string ToString() => $"{UsedQuota}/{TotalQuota} slots ({OccupancyPercentage}%)";
        
}