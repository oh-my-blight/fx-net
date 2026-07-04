using Fx.Net.Errors;
using Fx.Net.Result;
using Fx.Net.Types;

using Tests.Fx.Net.Result.Fixtures;
using Tests.Fx.Net.Result.Stubs;

namespace Tests.Fx.Net.Result;

public class ResultEqualityTests
{
    private readonly ResultTestData _resultTestData = new ResultTestData();


    [Fact]
    public void Equals_WhenTwoResults_AreIdenticalSuccess_ReturnsTrue()
    {
        const int comparableValue = 42;

        var firstResult = global::Fx.Net.Result.Result.Success(comparableValue);
        var secondResult = global::Fx.Net.Result.Result.Success(comparableValue);

        var equalsMethodResult = firstResult.Equals(secondResult);
        var equalOperatorResult = firstResult == secondResult;

        Assert.True(equalsMethodResult);
        Assert.True(equalOperatorResult);
    }

    [Fact]
    public void Equals_WhenTwoSuccessResults_HaveDifferentValues_ReturnFalse()
    {
        var firstComparableValue = global::Fx.Net.Result.Result.Success(42);
        var secondComparableValue = global::Fx.Net.Result.Result.Success(100);

        var equalOperatorResult = secondComparableValue == firstComparableValue;
        var notEqualOperatorResult = firstComparableValue != secondComparableValue;

        Assert.False(equalOperatorResult);
        Assert.True(notEqualOperatorResult);
    }

    [Fact]
    public void Equals_WhenTwoResult_Are_IdenticalFailure_ReturnsTrue()
    {
        Result<Unit> firstResult = global::Fx.Net.Result.Result.Failure(_resultTestData.Error);
        Result<Unit> secondResult = global::Fx.Net.Result.Result.Failure(_resultTestData.Error);

        var compareResult = firstResult == secondResult;

        Assert.True(compareResult);
    }

    [Fact]
    public void Equals_WhenTwoFailureResults_HaveDifferentErrors_ReturnsFalse()
    {
        Result<Unit> firstResult =
            global::Fx.Net.Result.Result.Failure(new Error("Code.BetaTest", "Test error message"));
        Result<Unit> secondResult = global::Fx.Net.Result.Result.Failure(_resultTestData.Error);

        var compareResult = firstResult == secondResult;


        Assert.False(compareResult);
    }

    [Fact]
    public void Equals_WhenOneIsSuccessAndOtherIsFailure_ReturnsFalse()
    {
        const string defaultValue = default;
        var defaultSuccessResult = global::Fx.Net.Result.Result.Success(defaultValue);
        var failureResult = global::Fx.Net.Result.Result.Failure(_resultTestData.Error);

        var compareResult = defaultSuccessResult == failureResult;
        Assert.False(compareResult);
    }

    [Fact]
    public void Equals_WhenComparedWithNullOrDifferentType_ReturnsFalse()
    {
        const string expectedValue = null!;
        var result = global::Fx.Net.Result.Result.Success(expectedValue);

        Assert.False(result.Equals(expectedValue));
    }

    [Fact]
    public void Equals_WhenObjectIsNull_ReturnFalse()
    {
        const int expectedValue = 34;
        Result<Unit> fail = global::Fx.Net.Result.Result.Failure(_resultTestData.Error);
        var success = global::Fx.Net.Result.Result.Success(expectedValue);
        var failObj = (object)fail;
        var successObj = (object)success;

        var equalsSuccessResult = successObj.Equals(null);
        var equalsFailsResult = failObj.Equals(null);

        Assert.Multiple(
            () => Assert.False(equalsFailsResult),
            () => Assert.False(equalsSuccessResult)
        );
    }

    [Fact]
    public void Equals_WhenDefaultInstanceComparedWithAnotherDefaultInstance_ReturnsTrue()
    {
        Result<Unit> firstResult = default;
        Result<Unit> secondResult = default;

        var equalOperator = firstResult == secondResult;
        var equalsMethod = firstResult.Equals(secondResult);

        Assert.Multiple(
            () => Assert.True(equalOperator),
            () => Assert.True(equalsMethod),
            () => Assert.Equal(firstResult.GetHashCode(), secondResult.GetHashCode())
        );
    }

    [Fact]
    public void Equals_WhenDefaultInstanceComparedWithExplicitFailureOfSameError_ReturnsTrue()
    {
        Result<int> defaultResult = default;
        Result<int> failureResult = global::Fx.Net.Result.Result.Failure(ResultErrors.DefaultNullFailure);

        var equalResult = defaultResult == failureResult;

        Assert.Multiple(
            () => Assert.True(equalResult),
            () => Assert.True(defaultResult.IsFailure),
            () => Assert.True(failureResult.IsFailure),
            () => Assert.Equal(ResultErrors.DefaultNullFailure, defaultResult.Error),
            () => Assert.Equal(ResultErrors.DefaultNullFailure, failureResult.Error)
        );
    }

    [Fact]
    public void Equals_WhenValueTypeOverridesEquals_UsesCustomEqualityLogic()
    {
        var firstUser = new UserStab(1, "Dave", 23);
        var secondUser = new UserStab(1, "Gilbert", 45);
        var firstResult = global::Fx.Net.Result.Result.Success(firstUser);
        var secondResult = global::Fx.Net.Result.Result.Success(secondUser);

        var equalOperatorResult = firstResult == secondResult;

        Assert.True(equalOperatorResult);
    }

    [Fact]
    public void GetHashCode_ForIdenticalResults_ReturnsSameHashCode()
    {
        var expectedUnit = global::Fx.Net.Result.Result.Success();
        var actualUnit = global::Fx.Net.Result.Result.Success();

        var expected = global::Fx.Net.Result.Result.Success(100);
        var actual = global::Fx.Net.Result.Result.Success(100);

        Assert.Multiple(
            () => Assert.Equal(expectedUnit.GetHashCode(), actualUnit.GetHashCode()),
            () => Assert.Equal(expected.GetHashCode(), actual.GetHashCode())
        );
    }

    [Fact]
    public void GetHashCode_ForDifferentResults_ReturnsDifferentHashCode()
    {
        var firstResult = global::Fx.Net.Result.Result.Success(50);
        var secondResult = global::Fx.Net.Result.Result.Success(100);

        Assert.NotEqual(firstResult.GetHashCode(), secondResult.GetHashCode());
    }
}