using Microsoft.Extensions.DependencyInjection;
using VyshKat.ConsoleUi;

namespace VyshKat.Tests;

public class ConsoleAppTests
{
    private static (ConsoleApp App, StringWriter Output) Create(string input)
    {
        var output = new StringWriter();
        var provider = new ServiceCollection()
            .AddVyshKat(new StringReader(input), output)
            .BuildServiceProvider();
        return (provider.GetRequiredService<ConsoleApp>(), output);
    }

    [Fact]
    public void Demo_RunsAllScenarios()
    {
        var (app, output) = Create(string.Empty);

        app.RunDemo();

        var text = output.ToString();
        Assert.Contains("Отклонён техосмотром", text);
        Assert.Contains("уже стоит на балансе под номером VK-0004", text);
        Assert.Contains("Всего на балансе: 6 (транспорт: 3, вещи: 3)", text);
    }

    [Fact]
    public void Menu_AddScooter_ShowsItInInventory()
    {
        // 1 -> самокат, ввод данных, 5 -> инвентарь, 0 -> выход
        var (app, output) = Create("1\n1\nNinebot\n7\n0,4\n5\n0\n");

        app.Run();

        var text = output.ToString();
        Assert.Contains("Принят, выдан номер VK-0001: Ninebot", text);
        Assert.Contains("Инвентарь (1):", text);
    }

    [Fact]
    public void Menu_WrongInput_IsAskedAgain()
    {
        var (app, output) = Create("9\n1\n2\nStels\n42\n8\n0\n");

        app.Run();

        var text = output.ToString();
        Assert.Contains("Неизвестная команда", text);
        Assert.Contains("Нужно целое число от 1 до 10", text);
    }

    [Theory]
    [InlineData("1\n0\n5\n0\n")]              // «0 — назад» при выборе вида
    [InlineData("1\n1\nNinebot\nназад\n5\n0\n")] // «назад» посреди ввода данных
    public void Menu_Back_CancelsAdding(string input)
    {
        var (app, output) = Create(input);

        app.Run();

        var text = output.ToString();
        Assert.Contains("Отменено, возврат в меню.", text);
        Assert.Contains("Инвентарь (0):", text);
    }
}
