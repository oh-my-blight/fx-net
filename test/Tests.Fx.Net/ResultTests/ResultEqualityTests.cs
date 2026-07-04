using Fx.Net.Errors;
using Fx.Net.Monads.Result;
using Fx.Net.Types;

using Tests.Fx.Net.ResultTests.Fixtures;
using Tests.Fx.Net.ResultTests.Stubs;

namespace Tests.Fx.Net.ResultTests;

public class ResultEqualityTests
{
    private readonly ResultTestData _resultTestData = new ResultTestData();


    [Fact]
    public void Equals_WhenTwoResults_AreIdenticalSuccess_ReturnsTrue()
    {
        const int comparableValue = 42;

        var firstResult = Result.Success(comparableValue);
        var secondResult = Result.Success(comparableValue);

        var equalsMethodResult = firstResult.Equals(secondResult);
        var equalOperatorResult = firstResult == secondResult;

        Assert.True(equalsMethodResult);
        Assert.True(equalOperatorResult);
    }

    [Fact]
    public void Equals_WhenTwoSuccessResults_HaveDifferentValues_ReturnFalse()
    {
        var firstComparableValue = Result.Success(42);
        var secondComparableValue = Result.Success(100);

        var equalOperatorResult = secondComparableValue == firstComparableValue;
        var notEqualOperatorResult = firstComparableValue != secondComparableValue;

        Assert.False(equalOperatorResult);
        Assert.True(notEqualOperatorResult);
    }

    [Fact]
    public void Equals_WhenTwoResult_Are_IdenticalFailure_ReturnsTrue()
    {
        Result<Unit> firstResult = Result.Failure(_resultTestData.Error);
        Result<Unit> secondResult = Result.Failure(_resultTestData.Error);

        var compareResult = firstResult == secondResult;

        Assert.True(compareResult);
    }

    [Fact]
    public void Equals_WhenTwoFailureResults_HaveDifferentErrors_ReturnsFalse()
    {
        Result<Unit> firstResult = Result.Failure(new Error("Code.BetaTest", "Test error message"));
        Result<Unit> secondResult = Result.Failure(_resultTestData.Error);

        var compareResult = firstResult == secondResult;


        Assert.False(compareResult);
    }

    [Fact]
    public void Equals_WhenOneIsSuccessAndOtherIsFailure_ReturnsFalse()
    {
        const string defaultValue = default;
        var defaultSuccessResult = Result.Success(defaultValue);
        var failureResult = Result.Failure(_resultTestData.Error);

        var compareResult = defaultSuccessResult == failureResult;
        Assert.False(compareResult);
    }

    [Fact]
    public void Equals_WhenComparedWithNullOrDifferentType_ReturnsFalse()
    {
        const string expectedValue = null!;
        var result = Result.Success(expectedValue);

        Assert.False(result.Equals(expectedValue));
    }

    [Fact]
    public void Equals_WhenObjectIsNull_ReturnFalse()
    {
        const int expectedValue = 34;
        Result<Unit> fail = Result.Failure(_resultTestData.Error);
        var success = Result.Success(expectedValue);
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
        Result<int> failureResult = Result.Failure(ResultErrors.DefaultNullFailure);

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
        var firstResult = Result.Success(firstUser);
        var secondResult = Result.Success(secondUser);

        var equalOperatorResult = firstResult == secondResult;

        Assert.True(equalOperatorResult);
    }


    [Fact]
    public void Equals_WithErrorHavingNullFields_DoesNotThrowsNullReferenceException()
    {
        var brokenError = new Error(null!, null!);

        Result<Unit> firstFailure = Result.Failure(brokenError);
        Result<Unit> secondFailure = Result.Failure(brokenError);

        var exception = Record.Exception(() => firstFailure.Equals(secondFailure));
        Assert.Null(exception);
    }


    [Fact]
    public void Equals_WhenValuesAreArraysWithSameElements_ReturnsFalseDueToReferenceEquality()
    {
        var array1 = new int[] { 1, 2, 3 };
        var array2 = new int[] { 1, 2, 3 };

        var firstResult = Result.Success(array1);
        var secondResult = Result.Success(array2);

        var isEqual = firstResult == secondResult;

        Assert.False(isEqual);
    }

    [Fact]
    public void GetHashCode_ForIdenticalResults_ReturnsSameHashCode()
    {
        var expectedUnit = Result.Success();
        var actualUnit = Result.Success();

        var expected = Result.Success(100);
        var actual = Result.Success(100);

        Assert.Multiple(
            () => Assert.Equal(expectedUnit.GetHashCode(), actualUnit.GetHashCode()),
            () => Assert.Equal(expected.GetHashCode(), actual.GetHashCode())
        );
    }

    [Fact]
    public void GetHashCode_ForDifferentResults_ReturnsDifferentHashCode()
    {
        var firstResult = Result.Success(50);
        var secondResult = Result.Success(100);

        Assert.NotEqual(firstResult.GetHashCode(), secondResult.GetHashCode());
    }
}