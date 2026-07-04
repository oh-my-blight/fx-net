using System.Threading.Tasks;

using Fx.Net.Errors;
using Fx.Net.Monads.Result;

namespace Tests.Fx.Net.ResultTests;

public class ResultCachingTests
{
    [Fact]
    public void Success_Void_Returns_CachedInstance()
    {
        var firstCall = Result.Success();
        var secondCall = Result.Success();

        Assert.Equal(firstCall, secondCall);
    }

    [Fact]
    public async Task SuccessTask_ReturnsCompletedTask_WithSuccessResult()
    {
        var result = Result.SuccessTask;
        var taskResult = await result;

        Assert.Equal(TaskStatus.RanToCompletion, result.Status);
        Assert.True(taskResult.IsSuccess);
    }

    [Fact]
    public void SuccessTask_MultipleCalls_ReturnsSameCachedInstanceReference()
    {
        var firstCall = Result.SuccessTask;
        var secondCall = Result.SuccessTask;

        Assert.Same(firstCall, secondCall);
    }

    [Fact]
    public void NullValueAsTask_MultipleCalls_ReturnsSameCachedInstanceReference()
    {
        var firstCall = Result.NullValueAsTask;
        var secondCall = Result.NullValueAsTask;

        Assert.Same(firstCall, secondCall);
    }


    [Fact]
    public void ChainBrokenTask_MultipleCall_ReturnsSameCachedInstanceReference()
    {
        var firstCall = Result.ChainBrokenTask;
        var secondCall = Result.ChainBrokenTask;

        Assert.Same(firstCall, secondCall);
    }


    [Fact]
    public async Task NullValue_Returns_ValueTask_WithNull_ValueError()
    {
        var result = Result.NullValue;

        var innerResult = await result;

        Assert.True(innerResult.IsFailure);
        Assert.Equal(ResultErrors.NullValue, innerResult.Error);
    }

    [Fact]
    public async Task NullValueAsTask_Returns_Task_WithNullValueError()
    {
        var result = Result.NullValueAsTask;

        var innerResult = await result;

        Assert.Equal(TaskStatus.RanToCompletion, result.Status);
        Assert.True(innerResult.IsFailure);
        Assert.Equal(ResultErrors.NullValue, innerResult.Error);
    }

    [Fact]
    public async Task ChainBrokenTask_Returns_Task_WithResultError()
    {
        var result = Result.ChainBrokenTask;

        var innerResult = await result;

        Assert.Equal(TaskStatus.RanToCompletion, result.Status);
        Assert.True(innerResult.IsFailure);
        Assert.Equal(ResultErrors.NullResult, innerResult.Error);
    }

    [Fact]
    public async Task ChainBrokenValueTask_Returns_ValueTask_WithNullResultError()
    {
        var result = Result.ChainBrokenValueTask;
        var innerResult = await result;

        Assert.True(innerResult.IsFailure);
        Assert.Equal(ResultErrors.NullResult, innerResult.Error);
    }
}