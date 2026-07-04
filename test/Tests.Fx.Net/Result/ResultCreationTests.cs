using Fx.Net.Errors;
using Fx.Net.Result;
using Fx.Net.Types;

using Tests.Fx.Net.Result.Fixtures;

namespace Tests.Fx.Net.Result;

public class ResultCreationTests
{
    private readonly ResultTestData _resultTestData = new ResultTestData();


    [Fact]
    public void Success_WhenValueIsValueType_SetsIsSuccessToTrueAndStoresValue()
    {
        const int expectedValue = 1;

        var result = global::Fx.Net.Result.Result.Success(expectedValue);

        Assert.True(result.IsSuccess);
        Assert.Equal(Error.None, result.Error);

        var hasValue = result.TryGetSuccess(out var value);
        Assert.True(hasValue);
        Assert.Equal(expectedValue, value);
    }

    [Fact]
    public void Success_WhenValueIsReferenceType_SetsIsSuccessToTrueAndStoresValue()
    {
        var result = global::Fx.Net.Result.Result.Success(_resultTestData.Value);

        Assert.True(result.IsSuccess);
        Assert.Equal(Error.None, result.Error);

        var hasValue = result.TryGetSuccess(out var value);
        Assert.True(hasValue);
        Assert.Equal(_resultTestData.Value, value);
    }


    [Fact]
    public void Success_WhenValueIsNull_ReturnsFailureWithNullValueContext()
    {
        string expectedValue = null!;

        var result = global::Fx.Net.Result.Result.Success(expectedValue);

        Assert.True(result.IsFailure);
        Assert.Equal(ResultErrors.NullValue, result.Error);

        var hasValue = result.TryGetSuccess(out var value);

        Assert.False(hasValue);
        Assert.Null(value);
    }

    [Fact]
    public void Success_Void_ReturnsSuccessUnit_WithIsSuccessTrue()
    {
        var actual = global::Fx.Net.Result.Result.Success();

        Assert.True(actual.IsSuccess);
        Assert.Equal(Error.None, actual.Error);
    }

    [Fact]
    public void Success_WhenValueIsNullNullableValueType_ReturnsFailureWithNullValueContext()
    {
        int? nullValue = null!;

        var result = global::Fx.Net.Result.Result.Success(nullValue);

        Assert.False(result.IsSuccess);
        Assert.Equal(default, nullValue);
        Assert.Equal(ResultErrors.NullValue, result.Error);
    }

    [Fact]
    public void Failure_WhenCreatedWithError_SetsIsFailureToTrueAndStoresError()
    {
        Result<Unit> actual = global::Fx.Net.Result.Result.Failure(_resultTestData.Error);

        Assert.True(actual.IsFailure);
        Assert.Equal(_resultTestData.Error, actual.Error);
    }


    [Fact]
    public void Failure_WhenCreatedWithError_OutputsFalseAndDefaultValue_InTryGetSuccess()
    {
        // Arrange
        Result<int> actualValueType = global::Fx.Net.Result.Result.Failure(_resultTestData.Error);
        Result<string> actualRefType = global::Fx.Net.Result.Result.Failure(_resultTestData.Error);

        //Action
        var isValueTypeAction = actualValueType.TryGetSuccess(out var valueType);
        var isRefTypeAction = actualRefType.TryGetSuccess(out var refType);

        // Assert
        Assert.Null(refType);
        Assert.Equal(0, valueType);
        Assert.False(isValueTypeAction);
        Assert.False(isRefTypeAction);
    }

    [Fact]
    public void Failure_WhenCreatedWithDefaultError_RetainsDefaultErrorWithoutCrashing()
    {
        Result<Unit> result = global::Fx.Net.Result.Result.Failure(default(Error));

        Assert.Equal(ResultErrors.DefaultNullFailure, result.Error);
    }

    [Fact]
    public void Default_ShouldInitialize_WithDefaultFailureError()
    {
        Result<int> result = default;

        Assert.Equal(ResultErrors.DefaultNullFailure, result.Error);
    }

    [Fact]
    public void TryGetSuccess_WhenDefaultInstanceWithReferenceType_ReturnsFalseAndNull()
    {
        Result<string> defaultResult = default;

        var hasValue = defaultResult.TryGetSuccess(out var value);

        Assert.False(hasValue);
        Assert.Null(value);
    }
}