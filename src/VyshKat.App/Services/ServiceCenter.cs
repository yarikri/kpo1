using VyshKat.Domain;
using VyshKat.Domain.Vehicles;

namespace VyshKat.Services;

/// <summary>
/// Регламент техосмотра «ВышКат»: электрический транспорт с суточным расходом
/// выше лимита в парк не допускается. Неэлектрический транспорт по энергии не проверяется.
/// </summary>
public sealed class ServiceCenter : IServiceCenter
{
    public const double DefaultMaxDailyEnergyKWh = 5.0;

    private readonly double _maxDailyEnergyKWh;

    public ServiceCenter()
        : this(DefaultMaxDailyEnergyKWh)
    {
    }

    public ServiceCenter(double maxDailyEnergyKWh)
    {
        _maxDailyEnergyKWh = Guard.NonNegative(maxDailyEnergyKWh, nameof(maxDailyEnergyKWh));
    }

    public InspectionResult Inspect(Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        if (vehicle is IEnergyConsumer consumer && consumer.DailyEnergyKWh > _maxDailyEnergyKWh)
        {
            return InspectionResult.Reject(
                $"суточный расход {consumer.DailyEnergyKWh:0.##} кВт·ч превышает допустимые {_maxDailyEnergyKWh:0.##} кВт·ч");
        }

        return InspectionResult.Approve();
    }
}
