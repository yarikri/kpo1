using VyshKat.Domain.Items;
using VyshKat.Services;

namespace VyshKat.Tests;

public class ParkTests
{
    [Fact]
    public void ItemWithoutNumber_CannotBeAddedPastAcceptance()
    {
        var park = new Park();

        Assert.Throws<InvalidOperationException>(() => park.Add(new Helmet("Шлем", "M")));
        Assert.Empty(park.Inventory);
    }
}
