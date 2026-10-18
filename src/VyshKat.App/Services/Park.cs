using VyshKat.Domain;

namespace VyshKat.Services;

/// <summary>
/// Хранилище всего, что стоит на балансе. Ничего не решает, только хранит.
/// Принимает только объекты с инвентарным номером, а номер выдаёт <see cref="AcceptanceService"/>,
/// поэтому положить сюда что-то мимо приёмки не получится.
/// </summary>
public sealed class Park
{
    private readonly List<IInventoryItem> _inventory = new();

    /// <summary>Весь инвентарь в порядке постановки на баланс, только для чтения.</summary>
    public IReadOnlyList<IInventoryItem> Inventory => _inventory.AsReadOnly();

    public void Add(IInventoryItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (item.InventoryNumber is null)
        {
            throw new InvalidOperationException($"У «{item.Name}» нет инвентарного номера — сначала приёмка.");
        }

        if (_inventory.Any(i => i.InventoryNumber == item.InventoryNumber))
        {
            throw new InvalidOperationException($"Инвентарный номер {item.InventoryNumber} уже стоит на балансе.");
        }

        _inventory.Add(item);
    }
}
