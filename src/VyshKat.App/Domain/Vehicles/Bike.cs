namespace VyshKat.Domain.Vehicles;

/// <summary>Обычный велосипед: транспорт без потребления энергии.</summary>
public sealed class Bike : Vehicle
{
    public Bike(string name, int simplicity)
        : base(name, simplicity)
    {
    }
}
