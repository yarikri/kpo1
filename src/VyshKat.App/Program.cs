using System.Text;
using Microsoft.Extensions.DependencyInjection;
using VyshKat;
using VyshKat.ConsoleUi;

try
{
    // На Windows с UTF-8 на входе ReadLine теряет кириллицу; с Unicode .NET читает консоль через ReadConsoleW.
    if (OperatingSystem.IsWindows() && !Console.IsInputRedirected)
    {
        Console.InputEncoding = Encoding.Unicode;
    }

    Console.OutputEncoding = Encoding.UTF8; // чтобы «—», «≥» и кавычки не превращались в «?»
}
catch (IOException)
{
    // Консоль не даёт сменить кодировку — работаем как есть.
}

using var provider = new ServiceCollection()
    .AddVyshKat(Console.In, Console.Out)
    .BuildServiceProvider();

var app = provider.GetRequiredService<ConsoleApp>();

if (args.Contains("--demo"))
{
    app.RunDemo();
}
else
{
    app.Run();
}
