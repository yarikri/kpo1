using VyshKat.Domain.Vehicles;
using VyshKat.Services;

namespace VyshKat.Tests;

public class ServiceCenterTests
{
    [Fact]
    public void ScooterWithinLimit_IsApproved()
    {
        var result = new ServiceCenter(1.0).Inspect(new Scooter("Ninebot", 7, 1.0));

        Assert.True(result.IsApproved);
    }

    [Fact]
    public void EBikeOverLimit_IsRejectedWithReason()
    {
        var result = new ServiceCenter(1.0).Inspect(new EBike("Eltreco", 5, 1.5));

        Assert.False(result.IsApproved);
        Assert.Contains("превышает", result.Reason);
    }

    [Fact]
    public void Bike_IsAlwaysApproved()
    {
        var result = new ServiceCenter(0).Inspect(new Bike("Stels", 9));

        Assert.True(result.IsApproved);
    }
}
