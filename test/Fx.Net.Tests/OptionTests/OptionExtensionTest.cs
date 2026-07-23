using System;

using Fx.Net.Monads.Option;
using Fx.Net.Monads.Option.Extensions;

namespace Fx.Net.Tests.OptionTests;

public class OptionExtensionTest
{
    [Fact]
    public void Map_WhenHasValue_ReturnedMappedValue()
    {
        const int expectedValue = 80;
        var option = Option.Some(40);

        var actual = option.Map(x => x * 2);

        Assert.True(option.HasValue);
        Assert.Equal(expectedValue, actual.UnwrapOr(10));
    }

    [Fact]
    public void Unwrap_WheValueIsNull_ReturnTrue()
    {
        string expectedValue = null!;
        
        var option = Option.Some(expectedValue);

         Assert.True(option.IsNone);
    }
}