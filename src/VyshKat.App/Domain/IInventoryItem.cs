namespace VyshKat.Domain;

/// <summary>
/// Всё, что ставится на баланс. Инвентарный номер выдаёт приёмка, поэтому
/// до постановки на баланс его нет (<c>null</c>).
/// </summary>
public interface IInventoryItem
{
    string Name { get; }

    string? InventoryNumber { get; }
}
