namespace VyshKat.Domain.Items;

/// <summary>Шлем. Выдаётся вместе с транспортом, но энергию не потребляет.</summary>
public sealed class Helmet : Item
{
    public Helmet(string name, string size)
        : base(name)
    {
        Size = Guard.NotEmpty(size, nameof(size));
    }

    /// <summary>Размер шлема (S, M, L…).</summary>
    public string Size { get; }
}
