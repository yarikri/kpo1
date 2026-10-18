using VyshKat.Domain.Vehicles;
using VyshKat.Services;

namespace VyshKat.Tests.Fakes;

// Фейковый техосмотр: всегда отвечает одно и то же и запоминает, кого проверял
internal sealed class FakeServiceCenter : IServiceCenter
{
    private readonly InspectionResult _verdict;

    private FakeServiceCenter(InspectionResult verdict) => _verdict = verdict;

    public List<Vehicle> Inspected { get; } = new();

    public static FakeServiceCenter Approving() => new(InspectionResult.Approve());

    public static FakeServiceCenter Rejecting(string reason = "тестовый отказ") => new(InspectionResult.Reject(reason));

    public InspectionResult Inspect(Vehicle vehicle)
    {
        Inspected.Add(vehicle);
        return _verdict;
    }
}
