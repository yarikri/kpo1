using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VyshKat.Domain.Vehicles;
using VyshKat.Services;
using VyshKat.Tests.Fakes;

namespace VyshKat.Tests;

public class DependencyInjectionTests
{
    [Fact]
    public void ServiceCenter_CanBeReplacedInContainer()
    {
        var fake = FakeServiceCenter.Rejecting("подмена через DI");
        var services = new ServiceCollection().AddVyshKat(TextReader.Null, TextWriter.Null);
        services.Replace(ServiceDescriptor.Singleton<IServiceCenter>(fake));
        using var provider = services.BuildServiceProvider();

        // настоящий техосмотр велосипед бы пропустил
        provider.GetRequiredService<AcceptanceService>().Accept(new Bike("Stels", 9));

        Assert.Single(fake.Inspected);
        Assert.Equal(0, provider.GetRequiredService<ParkReport>().TotalCount);
    }
}
