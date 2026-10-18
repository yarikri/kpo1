using Microsoft.Extensions.DependencyInjection;
using VyshKat.ConsoleUi;
using VyshKat.Services;

namespace VyshKat;

/// <summary>
/// Единственное место, где решается, какие реализации подставлять (composition root).
/// Вынесено из Program.cs, чтобы тесты собирали тот же контейнер и подменяли в нём техосмотр.
/// </summary>
public static class ServiceRegistration
{
    public static IServiceCollection AddVyshKat(this IServiceCollection services, TextReader input, TextWriter output)
    {
        services.AddSingleton<Park>();
        services.AddSingleton<IServiceCenter, ServiceCenter>();
        services.AddSingleton<INoviceRule, SimplicityNoviceRule>();
        services.AddSingleton<AcceptanceService>();
        services.AddSingleton<ParkReport>();

        services.AddSingleton(input);
        services.AddSingleton(output);
        services.AddSingleton<ConsoleApp>();

        return services;
    }
}
