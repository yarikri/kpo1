namespace VyshKat.Services;

/// <summary>Вердикт техосмотра: принято или отклонено с причиной.</summary>
public sealed record InspectionResult
{
    private InspectionResult(bool isApproved, string? reason)
    {
        IsApproved = isApproved;
        Reason = reason;
    }

    public bool IsApproved { get; }

    /// <summary>Причина отказа; у принятого устройства — <c>null</c>.</summary>
    public string? Reason { get; }

    public static InspectionResult Approve() => new(true, null);

    public static InspectionResult Reject(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("У отказа должна быть причина.", nameof(reason));
        }

        return new InspectionResult(false, reason);
    }
}
