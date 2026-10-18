using VyshKat.Domain;
using VyshKat.Domain.Items;
using VyshKat.Domain.Vehicles;

namespace VyshKat.Services;

/// <summary>
/// Отчёты по парку. Только считает и отбирает данные, в консоль не пишет —
/// форматирование остаётся за консольным слоем.
/// </summary>
public sealed class ParkReport
{
    private readonly Park _park;
    private readonly INoviceRule _noviceRule;

    public ParkReport(Park park, INoviceRule noviceRule)
    {
        _park = park;
        _noviceRule = noviceRule;
    }

    public int TotalCount => _park.Inventory.Count;

    public int VehicleCount => _park.Inventory.OfType<Vehicle>().Count();

    public int ItemCount => _park.Inventory.OfType<Item>().Count();

    /// <summary>Суммарный суточный расход всех потребителей энергии: и транспорта, и вещей.</summary>
    public double TotalDailyEnergyKWh => _park.Inventory.OfType<IEnergyConsumer>().Sum(c => c.DailyEnergyKWh);

    public string NoviceRuleDescription => _noviceRule.Description;

    public IReadOnlyList<Vehicle> NoviceFriendlyVehicles() =>
        _park.Inventory.OfType<Vehicle>().Where(_noviceRule.IsSuitable).ToList();

    public IReadOnlyList<IInventoryItem> Inventory => _park.Inventory;
}
