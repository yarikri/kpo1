using VyshKat.Domain.Vehicles;

namespace VyshKat.Services;

/// <summary>
/// Техосмотр: только выносит вердикт, ничего не меняет.
/// Интерфейс нужен, чтобы подменять техосмотр в тестах и при смене регламента.
/// </summary>
public interface IServiceCenter
{
    InspectionResult Inspect(Vehicle vehicle);
}
