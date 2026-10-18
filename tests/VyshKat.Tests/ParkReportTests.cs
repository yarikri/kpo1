using VyshKat.Domain.Items;
using VyshKat.Domain.Vehicles;
using VyshKat.Services;
using VyshKat.Tests.Fakes;

namespace VyshKat.Tests;

public class ParkReportTests
{
    private readonly ParkReport _report;

    public ParkReportTests()
    {
        var park = new Park();
        var acceptance = new AcceptanceService(FakeServiceCenter.Approving(), park);
        acceptance.Accept(new Scooter("Ninebot", 7, 0.5));
        acceptance.Accept(new EBike("Eltreco", 5, 1.0));
        acceptance.Accept(new Bike("Stels", 9));
        acceptance.Accept(new Helmet("Шлем", "M"));
        acceptance.Accept(new ChargingLocker("Шкаф", 2.0, 8));
        _report = new ParkReport(park, new SimplicityNoviceRule());
    }

    [Fact]
    public void Counts_AreCorrect()
    {
        Assert.Equal(3, _report.VehicleCount);
        Assert.Equal(2, _report.ItemCount);
    }

    [Fact]
    public void TotalEnergy_CountsVehiclesAndItems()
    {
        Assert.Equal(3.5, _report.TotalDailyEnergyKWh, precision: 6);
    }

    [Fact]
    public void NoviceList_HasOnlySimpleVehicles()
    {
        var names = _report.NoviceFriendlyVehicles().Select(v => v.Name);

        Assert.Equal(new[] { "Ninebot", "Stels" }, names);
    }
}
