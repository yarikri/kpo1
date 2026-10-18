namespace VyshKat.Domain.Vehicles;

/// <summary>
/// Семейство «транспорт». Хранит общие для всех видов данные и их проверку.
/// Простота для новичка лежит здесь, потому что в системе арендуется только транспорт.
/// </summary>
public abstract class Vehicle : IInventoryItem
{
    public const int MinSimplicity = 1;
    public const int MaxSimplicity = 10;

    protected Vehicle(string name, int simplicity)
    {
        Name = Guard.NotEmpty(name, nameof(name));
        Simplicity = Guard.InRange(simplicity, MinSimplicity, MaxSimplicity, nameof(simplicity));
    }

    public string Name { get; }

    /// <summary>Выдаётся приёмкой после успешного техосмотра.</summary>
    public string? InventoryNumber { get; private set; }

    /// <summary>Простота для новичка, 1–10.</summary>
    public int Simplicity { get; }

    // internal: номер может выдать только код этого проекта (приёмка), а не кто угодно снаружи.
    internal void AssignInventoryNumber(string number)
    {
        if (InventoryNumber is not null)
        {
            throw new InvalidOperationException($"У «{Name}» уже есть инвентарный номер {InventoryNumber}.");
        }

        InventoryNumber = Guard.NotEmpty(number, nameof(number));
    }
}
