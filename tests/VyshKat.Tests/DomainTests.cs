using VyshKat.Domain.Items;
using VyshKat.Domain.Vehicles;

namespace VyshKat.Tests;

public class DomainTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void Vehicle_WithBadSimplicity_IsNotCreated(int simplicity)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Bike("Stels", simplicity));
    }

    [Fact]
    public void Item_WithoutName_IsNotCreated()
    {
        Assert.Throws<ArgumentException>(() => new Helmet(" ", "M"));
    }

    [Fact]
    public void Scooter_WithNegativeEnergy_IsNotCreated()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Scooter("Ninebot", 7, -0.1));
    }

    [Fact]
    public void NewDevice_HasNoInventoryNumberYet()
    {
        Assert.Null(new Scooter("Ninebot", 7, 0.4).InventoryNumber);
    }
}
