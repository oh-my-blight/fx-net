using System.Threading.Tasks;

using Fx.Net.Errors;
using Fx.Net.Result;
using Fx.Net.Types;

namespace Tests.Fx.Net.Result;

public class ResultCreationTest
{
    private readonly Error _testError = new Error("Code.Test", "Test error message");
    private readonly string _testValue = "Hello, world!";

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
        var result = global::Fx.Net.Result.Result.Success(_testValue);

        Assert.True(result.IsSuccess);
        Assert.Equal(Error.None, result.Error);

        var hasValue = result.TryGetSuccess(out var value);
        Assert.True(hasValue);
        Assert.Equal(_testValue, value);
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
    public void Success_Void_Returns_CachedInstance()
    {
        var firstCall = global::Fx.Net.Result.Result.Success();
        var secondCall = global::Fx.Net.Result.Result.Success();

        Assert.Equal(firstCall, secondCall);
    }

    [Fact]
    public async Task SuccessTask_ReturnsCompletedTask_WithSuccessResult()
    {
        var result = global::Fx.Net.Result.Result.SuccessTask;
        var taskResult = await result;

        Assert.True(result.IsCompletedSuccessfully);
        Assert.True(taskResult.IsSuccess);
    }


    [Fact]
    public void Failure_WhenCreatedWithError_SetsIsFailureToTrueAndStoresError()
    {
        Result<Unit> actual = global::Fx.Net.Result.Result.Failure(_testError);

        Assert.True(actual.IsFailure);
        Assert.Equal(_testError, actual.Error);
    }


    [Fact]
    public void Failure_WhenCreatedWithError_OutputsFalseAndDefaultValue_InTryGetSuccess()
    {
        // Arrange
        Result<int> actualValueType = global::Fx.Net.Result.Result.Failure(_testError);
        Result<string> actualRefType = global::Fx.Net.Result.Result.Failure(_testError);

        //Action
        var isValueTypeAction = actualValueType.TryGetSuccess(out var valueType);
        var isRefTypeAction = actualRefType.TryGetSuccess(out var refType);

        // Assert
        Assert.Null(refType);
        Assert.Equal(0, valueType);
        Assert.False(isValueTypeAction);
        Assert.False(isRefTypeAction);
    }
}