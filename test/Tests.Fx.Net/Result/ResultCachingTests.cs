using System.Threading.Tasks;

using Fx.Net.Errors;

namespace Tests.Fx.Net.Result;

public class ResultCachingTests
{
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

        Assert.Equal(TaskStatus.RanToCompletion, result.Status);
        Assert.True(taskResult.IsSuccess);
    }

    [Fact]
    public void SuccessTask_MultipleCalls_ReturnsSameCachedInstanceReference()
    {
        var firstCall = global::Fx.Net.Result.Result.SuccessTask;
        var secondCall = global::Fx.Net.Result.Result.SuccessTask;

        Assert.Same(firstCall, secondCall);
    }

    [Fact]
    public void NullValueAsTask_MultipleCalls_ReturnsSameCachedInstanceReference()
    {
        var firstCall = global::Fx.Net.Result.Result.NullValueAsTask;
        var secondCall = global::Fx.Net.Result.Result.NullValueAsTask;

        Assert.Same(firstCall, secondCall);
    }


    [Fact]
    public void ChainBrokenTask_MultipleCall_ReturnsSameCachedInstanceReference()
    {
        var firstCall = global::Fx.Net.Result.Result.ChainBrokenTask;
        var secondCall = global::Fx.Net.Result.Result.ChainBrokenTask;

        Assert.Same(firstCall, secondCall);
    }


    [Fact]
    public async Task NullValue_Returns_ValueTask_WithNull_ValueError()
    {
        var result = global::Fx.Net.Result.Result.NullValue;

        var innerResult = await result;

        Assert.True(innerResult.IsFailure);
        Assert.Equal(ResultErrors.NullValue, innerResult.Error);
    }

    [Fact]
    public async Task NullValueAsTask_Returns_Task_WithNullValueError()
    {
        var result = global::Fx.Net.Result.Result.NullValueAsTask;

        var innerResult = await result;

        Assert.Equal(TaskStatus.RanToCompletion, result.Status);
        Assert.True(innerResult.IsFailure);
        Assert.Equal(ResultErrors.NullValue, innerResult.Error);
    }

    [Fact]
    public async Task ChainBrokenTask_Returns_Task_WithResultError()
    {
        var result = global::Fx.Net.Result.Result.ChainBrokenTask;

        var innerResult = await result;

        Assert.Equal(TaskStatus.RanToCompletion, result.Status);
        Assert.True(innerResult.IsFailure);
        Assert.Equal(ResultErrors.NullResult, innerResult.Error);
    }

    [Fact]
    public async Task ChainBrokenValueTask_Returns_ValueTask_WithNullResultError()
    {
        var result = global::Fx.Net.Result.Result.ChainBrokenValueTask;
        var innerResult = await result;

        Assert.True(innerResult.IsFailure);
        Assert.Equal(ResultErrors.NullResult, innerResult.Error);
    }
}