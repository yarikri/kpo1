namespace VyshKat.Domain.Vehicles;

/// <summary>Электровелосипед: транспорт, потребляющий энергию.</summary>
public sealed class EBike : Vehicle, IEnergyConsumer
{
    public EBike(string name, int simplicity, double dailyEnergyKWh)
        : base(name, simplicity)
    {
        DailyEnergyKWh = Guard.NonNegative(dailyEnergyKWh, nameof(dailyEnergyKWh));
    }

    public double DailyEnergyKWh { get; }
}
