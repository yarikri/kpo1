using System.Globalization;
using VyshKat.Domain;
using VyshKat.Domain.Items;
using VyshKat.Domain.Vehicles;
using VyshKat.Services;

namespace VyshKat.ConsoleUi;

/// <summary>
/// Консольный интерфейс: меню, ввод данных и вывод отчётов.
/// Ввод и вывод приходят через конструктор (TextReader/TextWriter), поэтому класс тестируется
/// без настоящей консоли.
/// </summary>
public sealed class ConsoleApp
{
    private const string BackCommand = "назад";

    private readonly AcceptanceService _acceptance;
    private readonly ParkReport _report;
    private readonly TextReader _input;
    private readonly TextWriter _output;

    public ConsoleApp(AcceptanceService acceptance, ParkReport report, TextReader input, TextWriter output)
    {
        _acceptance = acceptance;
        _report = report;
        _input = input;
        _output = output;
    }

    public void Run()
    {
        _output.WriteLine("ВышКат — учёт парка. «Доедем до дедлайна»");

        while (true)
        {
            PrintMenu();
            var command = _input.ReadLine();
            if (command is null)
            {
                return; // ввод закончился
            }

            try
            {
                switch (command.Trim())
                {
                    case "1": AddVehicleInteractive(); break;
                    case "2": AddItemInteractive(); break;
                    case "3": PrintCounts(); break;
                    case "4": PrintNoviceFriendly(); break;
                    case "5": PrintInventory(); break;
                    case "6": RunDemo(); break;
                    case "0":
                        _output.WriteLine("До встречи!");
                        return;
                    default:
                        _output.WriteLine("Неизвестная команда, введите число из меню.");
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                _output.WriteLine($"  Ошибка данных: {UserMessage(ex)}");
            }
            catch (BackException)
            {
                _output.WriteLine("  Отменено, возврат в меню.");
            }
            catch (InputEndedException)
            {
                return; // ввод оборвался посреди операции — тихо выходим
            }
        }
    }

    public void RunDemo()
    {
        Section("Демо 1. Приём транспорта через техосмотр");
        TryAccept(new Scooter("Ninebot Max G30", 7, 0.45));
        TryAccept(new EBike("Eltreco Ultra", 5, 0.9));
        TryAccept(new Bike("Stels Navigator", 9));

        Section("Демо 2. Техосмотр отклоняет устройство — номер не выдаётся");
        TryAccept(new Scooter("Прототип «Ракета»", 4, 7.5));

        Section("Демо 3. Некорректные данные (простота вне 1–10)");
        try
        {
            TryAccept(new Scooter("Kugoo S3", 11, 0.4));
        }
        catch (ArgumentException ex)
        {
            _output.WriteLine($"  Ошибка данных: {UserMessage(ex)}");
        }

        Section("Демо 4. Вещи на балансе (без техосмотра)");
        var helmet = new Helmet("Шлем городской", "M");
        TryAccept(helmet);
        TryAccept(new DockStation("Док-станция у корпуса на Покровке", 1.2, 10));
        TryAccept(new ChargingLocker("Зарядный шкаф на складе", 2.4, 12));

        Section("Демо 5. Повторная приёмка уже принятой вещи");
        TryAccept(helmet);

        Section("Демо 6. Отчёты");
        PrintCounts();
        PrintNoviceFriendly();
        PrintInventory();
    }

    private void PrintMenu()
    {
        _output.WriteLine();
        _output.WriteLine("1 — принять транспорт (через техосмотр)");
        _output.WriteLine("2 — поставить вещь на баланс");
        _output.WriteLine("3 — отчёт: число единиц и суммарная энергия");
        _output.WriteLine("4 — транспорт для новичков");
        _output.WriteLine("5 — все инвентарные номера");
        _output.WriteLine("6 — демо-сценарий");
        _output.WriteLine("0 — выход");
        _output.Write("> ");
    }

    private void AddVehicleInteractive()
    {
        PrintBackHint();
        var kind = AskChoice("Вид: 1 — самокат, 2 — велосипед, 3 — электровелосипед, 0 — назад", 3);
        var name = AskText("Название");
        var simplicity = AskInt("Простота для новичка (1–10)", Vehicle.MinSimplicity, Vehicle.MaxSimplicity);

        // Единственное место, которое знает обо всех видах транспорта:
        // новый вид = новый класс + одна ветка здесь.
        Vehicle vehicle = kind switch
        {
            1 => new Scooter(name, simplicity, AskEnergy()),
            2 => new Bike(name, simplicity),
            _ => new EBike(name, simplicity, AskEnergy()),
        };

        TryAccept(vehicle);
    }

    private void AddItemInteractive()
    {
        PrintBackHint();
        var kind = AskChoice("Вещь: 1 — шлем, 2 — док-станция, 3 — зарядный шкаф, 0 — назад", 3);
        var name = AskText("Название");

        Item item = kind switch
        {
            1 => new Helmet(name, AskText("Размер (S/M/L)")),
            2 => new DockStation(name, AskEnergy(), AskInt("Количество мест", 1, 100)),
            _ => new ChargingLocker(name, AskEnergy(), AskInt("Количество ячеек", 1, 100)),
        };

        TryAccept(item);
    }

    private void TryAccept(Vehicle vehicle)
    {
        try
        {
            var result = _acceptance.Accept(vehicle);
            _output.WriteLine(result.IsApproved
                ? $"  Принят, выдан номер {vehicle.InventoryNumber}: {Describe(vehicle)}"
                : $"  Отклонён техосмотром: {vehicle.Name} — {result.Reason}");
        }
        catch (InvalidOperationException ex)
        {
            _output.WriteLine($"  Не принят: {ex.Message}");
        }
    }

    private void TryAccept(Item item)
    {
        try
        {
            _acceptance.Accept(item);
            _output.WriteLine($"  На балансе, выдан номер {item.InventoryNumber}: {Describe(item)}");
        }
        catch (InvalidOperationException ex)
        {
            _output.WriteLine($"  Не принято: {ex.Message}");
        }
    }

    private void PrintCounts()
    {
        _output.WriteLine($"Всего на балансе: {_report.TotalCount} (транспорт: {_report.VehicleCount}, вещи: {_report.ItemCount})");
        _output.WriteLine($"Суммарный суточный расход энергии: {_report.TotalDailyEnergyKWh:0.##} кВт·ч");
    }

    private void PrintNoviceFriendly()
    {
        var vehicles = _report.NoviceFriendlyVehicles();
        _output.WriteLine($"Транспорт для новичков ({_report.NoviceRuleDescription}): {vehicles.Count}");
        foreach (var vehicle in vehicles)
        {
            _output.WriteLine($"  {vehicle.InventoryNumber,-10} {vehicle.Name} — простота {vehicle.Simplicity}");
        }
    }

    private void PrintInventory()
    {
        var inventory = _report.Inventory;
        _output.WriteLine($"Инвентарь ({inventory.Count}):");
        foreach (var item in inventory)
        {
            _output.WriteLine($"  {item.InventoryNumber,-10} {item.Name}");
        }
    }

    private static string Describe(IInventoryItem item)
    {
        var text = item.Name;
        if (item is Vehicle vehicle)
        {
            text += $", простота {vehicle.Simplicity}";
        }

        if (item is IEnergyConsumer consumer)
        {
            text += $", {consumer.DailyEnergyKWh:0.##} кВт·ч/сут";
        }

        return text;
    }

    // .NET дописывает к Message «(Parameter 'x')» и «Actual value was ...» — пользователю это не нужно.
    private static string UserMessage(ArgumentException ex)
    {
        var text = ex.Message.Split('\n')[0];
        return ex.ParamName is null ? text.Trim() : text.Replace($"(Parameter '{ex.ParamName}')", "").Trim();
    }

    private void Section(string title)
    {
        _output.WriteLine();
        _output.WriteLine($"=== {title} ===");
    }

    private void PrintBackHint() =>
        _output.WriteLine($"  (в любой момент введите «{BackCommand}», чтобы вернуться в меню)");

    // Методы ввода переспрашивают, пока не получат корректное значение.
    // «назад» бросает BackException — Run() ловит его и возвращает в меню.
    // Если ввод закончился (EOF), бросается InputEndedException — Run() завершает работу.

    private string AskText(string prompt)
    {
        while (true)
        {
            _output.Write($"{prompt}: ");
            var line = _input.ReadLine() ?? throw new InputEndedException();
            if (string.Equals(line.Trim(), BackCommand, StringComparison.OrdinalIgnoreCase))
            {
                throw new BackException();
            }

            if (!string.IsNullOrWhiteSpace(line))
            {
                return line.Trim();
            }

            _output.WriteLine("  Значение не может быть пустым.");
        }
    }

    private int AskInt(string prompt, int min, int max)
    {
        while (true)
        {
            var line = AskText(prompt);
            if (int.TryParse(line, out var value) && value >= min && value <= max)
            {
                return value;
            }

            _output.WriteLine($"  Нужно целое число от {min} до {max}.");
        }
    }

    // Выбор пункта: 1..max, «0» — назад.
    private int AskChoice(string prompt, int max)
    {
        var choice = AskInt(prompt, 0, max);
        return choice == 0 ? throw new BackException() : choice;
    }

    private double AskEnergy()
    {
        while (true)
        {
            var line = AskText("Суточный расход энергии, кВт·ч").Replace(',', '.');
            if (double.TryParse(line, NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
                && value >= 0 && double.IsFinite(value))
            {
                return value;
            }

            _output.WriteLine("  Нужно неотрицательное число, например 0.5");
        }
    }

    private sealed class InputEndedException : Exception
    {
    }

    private sealed class BackException : Exception
    {
    }
}
