using VyshKat.Domain.Vehicles;
using VyshKat.Services;

namespace VyshKat.Tests;

public class NoviceRuleTests
{
    [Theory]
    [InlineData(5, false)]
    [InlineData(6, true)]
    public void SimplicityFromSix_IsOkForNovice(int simplicity, bool expected)
    {
        var rule = new SimplicityNoviceRule();

        Assert.Equal(expected, rule.IsSuitable(new Bike("Stels", simplicity)));
    }
}
