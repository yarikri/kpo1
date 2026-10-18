using VyshKat.Domain.Vehicles;

namespace VyshKat.Services;

/// <summary>
/// Правило «можно выдавать новичку». Отдельная абстракция, потому что заказчик
/// прямо допускает усложнение правила: новое правило — новый класс, отчёт не меняется.
/// </summary>
public interface INoviceRule
{
    /// <summary>Описание правила для вывода пользователю.</summary>
    string Description { get; }

    bool IsSuitable(Vehicle vehicle);
}
