namespace VyshKat.Domain;

/// <summary>
/// Способность «потреблять энергию». Реализуют только электрические устройства —
/// и транспорт (самокат, электровелосипед), и вещи (док-станция, зарядный шкаф).
/// </summary>
public interface IEnergyConsumer
{
    /// <summary>Суточный расход энергии, кВт·ч.</summary>
    double DailyEnergyKWh { get; }
}
