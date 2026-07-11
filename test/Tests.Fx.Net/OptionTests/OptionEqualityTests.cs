using Fx.Net.Monads.Option;

namespace Tests.Fx.Net.OptionTests;

public class OptionEqualityTests
{
    [Fact]
    public void Equals_TwoSomeWithSamePrimitiveValues_ReturnsTrue()
    {
        const int expectedValue = 42;
        var firstOption = Option.Some(expectedValue);
        var secondOption = Option.Some(expectedValue);

        Assert.Multiple(
            () => Assert.True(firstOption.Equals(secondOption)),
            () => Assert.True(firstOption.Equals((object)secondOption)),
            () => Assert.True(firstOption == secondOption),
            () => Assert.False(firstOption != secondOption),
            () => Assert.Equal(firstOption.GetHashCode(), secondOption.GetHashCode())
        );
    }


    [Fact]
    public void Equals_TwoSomeWithDifferentPrimitiveValues_ReturnsFalse()
    {
        const int firstNum = 25;
        const int secondNum = 99;
        var firstOption = Option.Some(firstNum);
        var secondOption = Option.Some(secondNum);

        Assert.Multiple(
            () => Assert.False(firstOption.Equals(secondOption)),
            () => Assert.False(firstOption.Equals((object)secondOption)),
            () => Assert.False(firstOption == secondOption),
            () => Assert.True(firstOption != secondOption),
            () => Assert.NotEqual(firstOption.GetHashCode(), secondOption.GetHashCode())
        );
    }

    [Fact]
    public void Equals_TwoSomeWithContentEqualReferenceTypes_ReturnsTrue()
    {
        var firstStr = new string(new[] { 'a', 'b', 'c' });
        var secondStr = new string(new[] { 'a', 'b', 'c' });

        var firstOption = Option.Some(firstStr);
        var secondOption = Option.Some(secondStr);

        Assert.False(ReferenceEquals(firstStr, secondStr));
        Assert.True(firstOption.Equals(secondOption));
        Assert.Equal(firstOption.GetHashCode(), secondOption.GetHashCode());
    }


    [Fact]
    public void Equals_ExplicitNoneAndDefaultStruct_ReturnsTrue()
    {
        Option<string> noneExplicit = Option.None;
        Option<string> noneDefault = default;

        Assert.True(noneExplicit.Equals(noneDefault));
        Assert.True(noneExplicit == noneDefault);
        Assert.Equal(noneExplicit.GetHashCode(), noneDefault.GetHashCode());
    }

    [Fact]
    public void Equals_SomeAndNone_ReturnsFalse()
    {
        var someOption = Option.Some("Value");
        Option<string> noneOption = Option.None;

        Assert.False(someOption.Equals(noneOption));
        Assert.False(noneOption.Equals(someOption));
        Assert.True(someOption != noneOption);
    }

    [Fact]
    public void Equals_SomeWithDefaultValueDefaultStruct_ReturnsFalse()
    {
        var someZero = Option.Some(0);
        Option<int> noneDefault = default;

        Assert.False(someZero.Equals(noneDefault));
        Assert.True(someZero != noneDefault);
    }


    [Fact]
    public void Equals_WithNullOrDifferentTypeObject_ReturnsFalse()
    {
        const string value = "Value";
        const string testValue = "10";
        var option = Option.Some(10);

        Assert.False(option.Equals((object?)null));
        Assert.False(option.Equals(value));
        Assert.False(option.Equals(Option.Some(testValue)));
    }


    [Fact]
    public void GetHashCode_ForVariousNoneRepresentations_ReturnsSameHash()
    {
        Option<string> explicitNone = Option.None;
        Option<string> defaultStruct = default;
        Option<string> fromToken = (NoneToken)default;

        var hash1 = explicitNone.GetHashCode();
        var hash2 = defaultStruct.GetHashCode();
        var hash3 = fromToken.GetHashCode();

        Assert.Equal(hash1, hash2);
        Assert.Equal(hash2, hash3);
    }
}