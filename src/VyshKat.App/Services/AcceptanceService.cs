using VyshKat.Domain.Items;
using VyshKat.Domain.Vehicles;

namespace VyshKat.Services;

/// <summary>
/// Приёмка на баланс. Транспорт — только через техосмотр, вещи — напрямую.
/// Инвентарный номер выдаётся здесь и только тому, что принято.
/// Две перегрузки вместо одной с проверкой типа: компилятор сам не даст
/// провести транспорт мимо техосмотра.
/// </summary>
public sealed class AcceptanceService
{
    private readonly IServiceCenter _serviceCenter;
    private readonly Park _park;
    private int _lastNumber;

    public AcceptanceService(IServiceCenter serviceCenter, Park park)
    {
        _serviceCenter = serviceCenter;
        _park = park;
    }

    public InspectionResult Accept(Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);
        EnsureNotOnBalance(vehicle.InventoryNumber, vehicle.Name);

        var result = _serviceCenter.Inspect(vehicle);
        if (result.IsApproved)
        {
            vehicle.AssignInventoryNumber(NextNumber());
            _park.Add(vehicle);
        }

        return result;
    }

    public void Accept(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);
        EnsureNotOnBalance(item.InventoryNumber, item.Name);

        item.AssignInventoryNumber(NextNumber());
        _park.Add(item);
    }

    private static void EnsureNotOnBalance(string? inventoryNumber, string name)
    {
        if (inventoryNumber is not null)
        {
            throw new InvalidOperationException($"«{name}» уже стоит на балансе под номером {inventoryNumber}.");
        }
    }

    private string NextNumber() => $"VK-{++_lastNumber:D4}";
}
