using Fx.Net.Monads.Option;

namespace Tests.Fx.Net.OptionTests;

public class OptionCreationTests
{
    [Fact]
    public void Some_WhenValueIsNotNull_HasValueIsTrueAndIsNoneIsFalse()
    {
        var expectedValue = 20;

        var option = Option.Some(expectedValue);

        Assert.True(option.HasValue);
        Assert.False(option.IsNone);
    }


    [Fact]
    public void Some_WhenValueIsNull_IsNoneIsTrueAndHasValueIsFalse()
    {
        string expectedValue = null!;

        var option = Option.Some(expectedValue);

        Assert.True(option.IsNone);
        Assert.False(option.HasValue);
    }

    [Fact]
    public void None_WhenAssigned_IsNoneIsTrueAndHasValueIsFalse()
    {
        Option<string> option = Option.None;

        Assert.True(option.IsNone);
        Assert.False(option.HasValue);
    }

    [Fact]
    public void Some_Parameterless_ReturnsValidUnit()
    {
        var option = Option.Some();

        Assert.True(option.HasValue);
        Assert.False(option.IsNone);
    }


    [Fact]
    public void Some_WhenNullableValueTypeIsNull_ReturnsNone()
    {
        int? nullValue = null;
        var option = Option.Some(nullValue);

        Assert.True(option.IsNone);
    }

    [Fact]
    public void Some_WhenNullableValueTypesHasValue_ReturnsSome()
    {
        int? validValue = 42;

        var option = Option.Some(validValue);

        Assert.True(option.HasValue);
    }

    [Fact]
    public void Default_WhenCreatedAsDefaultStruct_IsNoneIsTrue_And_HasValueIsFalse()
    {
        Option<int> option = default;

        Assert.True(option.IsNone);
        Assert.False(option.HasValue);
    }
}