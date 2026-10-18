namespace VyshKat.Domain.Items;

/// <summary>Док-станция: место парковки и подзарядки транспорта.</summary>
public sealed class DockStation : Item, IEnergyConsumer
{
    public DockStation(string name, double dailyEnergyKWh, int slots)
        : base(name)
    {
        DailyEnergyKWh = Guard.NonNegative(dailyEnergyKWh, nameof(dailyEnergyKWh));
        Slots = Guard.InRange(slots, 1, 100, nameof(slots));
    }

    public double DailyEnergyKWh { get; }

    /// <summary>Сколько единиц транспорта помещается на станции.</summary>
    public int Slots { get; }
}
