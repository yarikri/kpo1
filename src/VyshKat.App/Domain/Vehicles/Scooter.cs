namespace VyshKat.Domain.Vehicles;

/// <summary>Электросамокат: транспорт, потребляющий энергию.</summary>
public sealed class Scooter : Vehicle, IEnergyConsumer
{
    public Scooter(string name, int simplicity, double dailyEnergyKWh)
        : base(name, simplicity)
    {
        DailyEnergyKWh = Guard.NonNegative(dailyEnergyKWh, nameof(dailyEnergyKWh));
    }

    public double DailyEnergyKWh { get; }
}
