namespace VyshKat.Domain;

/// <summary>
/// Общие проверки входных данных. Вынесены сюда, чтобы не дублировать логику
/// в каждом конструкторе (композиция вместо лишнего уровня наследования).
/// </summary>
internal static class Guard
{
    public static string NotEmpty(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Значение не может быть пустым.", paramName);
        }

        return value.Trim();
    }

    public static double NonNegative(double value, string paramName)
    {
        if (value < 0 || double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new ArgumentOutOfRangeException(paramName, value, "Значение должно быть неотрицательным числом.");
        }

        return value;
    }

    public static int InRange(int value, int min, int max, string paramName)
    {
        if (value < min || value > max)
        {
            throw new ArgumentOutOfRangeException(paramName, value, $"Значение должно быть от {min} до {max}.");
        }

        return value;
    }
}
