namespace VyshKat.Domain.Items;

/// <summary>
/// Семейство «вещи на балансе». Техосмотр не проходят и новичкам не выдаются,
/// поэтому простоты здесь нет. Общего родителя с <see cref="Vehicles.Vehicle"/> нет
/// сознательно: единый доступ к инвентарю даёт интерфейс <see cref="IInventoryItem"/>.
/// </summary>
public abstract class Item : IInventoryItem
{
    protected Item(string name)
    {
        Name = Guard.NotEmpty(name, nameof(name));
    }

    public string Name { get; }

    /// <summary>Выдаётся приёмкой при постановке на баланс.</summary>
    public string? InventoryNumber { get; private set; }

    internal void AssignInventoryNumber(string number)
    {
        if (InventoryNumber is not null)
        {
            throw new InvalidOperationException($"У «{Name}» уже есть инвентарный номер {InventoryNumber}.");
        }

        InventoryNumber = Guard.NotEmpty(number, nameof(number));
    }
}
