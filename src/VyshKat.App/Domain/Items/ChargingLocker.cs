namespace VyshKat.Domain.Items;

/// <summary>Зарядный шкаф для съёмных аккумуляторов.</summary>
public sealed class ChargingLocker : Item, IEnergyConsumer
{
    public ChargingLocker(string name, double dailyEnergyKWh, int cells)
        : base(name)
    {
        DailyEnergyKWh = Guard.NonNegative(dailyEnergyKWh, nameof(dailyEnergyKWh));
        Cells = Guard.InRange(cells, 1, 100, nameof(cells));
    }

    public double DailyEnergyKWh { get; }

    /// <summary>Количество ячеек под аккумуляторы.</summary>
    public int Cells { get; }
}
