using VyshKat.Domain.Items;
using VyshKat.Domain.Vehicles;
using VyshKat.Services;
using VyshKat.Tests.Fakes;

namespace VyshKat.Tests;

public class AcceptanceServiceTests
{
    [Fact]
    public void RejectedVehicle_DoesNotGetIntoPark_AndGetsNoNumber()
    {
        var park = new Park();
        var acceptance = new AcceptanceService(FakeServiceCenter.Rejecting("тормоза"), park);
        var scooter = new Scooter("Ninebot", 7, 0.4);

        var result = acceptance.Accept(scooter);

        Assert.False(result.IsApproved);
        Assert.Equal("тормоза", result.Reason);
        Assert.Null(scooter.InventoryNumber);
        Assert.Empty(park.Inventory);
    }

    [Fact]
    public void ApprovedVehicle_GetsNumber_AndGetsIntoPark()
    {
        var park = new Park();
        var acceptance = new AcceptanceService(FakeServiceCenter.Approving(), park);
        var scooter = new Scooter("Ninebot", 7, 0.4);

        acceptance.Accept(scooter);

        Assert.Equal("VK-0001", scooter.InventoryNumber);
        Assert.Single(park.Inventory);
    }

    [Fact]
    public void Numbers_AreUniqueForVehiclesAndItems()
    {
        var acceptance = new AcceptanceService(FakeServiceCenter.Approving(), new Park());
        var bike = new Bike("Stels", 9);
        var helmet = new Helmet("Шлем", "M");

        acceptance.Accept(bike);
        acceptance.Accept(helmet);

        Assert.Equal("VK-0001", bike.InventoryNumber);
        Assert.Equal("VK-0002", helmet.InventoryNumber);
    }

    [Fact]
    public void Item_IsAcceptedWithoutInspection()
    {
        var center = FakeServiceCenter.Rejecting();
        var park = new Park();

        new AcceptanceService(center, park).Accept(new Helmet("Шлем", "M"));

        Assert.Empty(center.Inspected);
        Assert.Single(park.Inventory);
    }

    [Fact]
    public void SameVehicle_CannotBeAcceptedTwice()
    {
        var acceptance = new AcceptanceService(FakeServiceCenter.Approving(), new Park());
        var bike = new Bike("Stels", 9);
        acceptance.Accept(bike);

        Assert.Throws<InvalidOperationException>(() => acceptance.Accept(bike));
    }
}
