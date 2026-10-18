using VyshKat.Domain;
using VyshKat.Domain.Vehicles;

namespace VyshKat.Services;

/// <summary>Базовое правило: простота для новичка не ниже порога (по умолчанию 6).</summary>
public sealed class SimplicityNoviceRule : INoviceRule
{
    public const int DefaultMinSimplicity = 6;

    private readonly int _minSimplicity;

    public SimplicityNoviceRule()
        : this(DefaultMinSimplicity)
    {
    }

    public SimplicityNoviceRule(int minSimplicity)
    {
        _minSimplicity = Guard.InRange(minSimplicity, Vehicle.MinSimplicity, Vehicle.MaxSimplicity, nameof(minSimplicity));
    }

    public string Description => $"простота для новичка ≥ {_minSimplicity}";

    public bool IsSuitable(Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);
        return vehicle.Simplicity >= _minSimplicity;
    }
}
