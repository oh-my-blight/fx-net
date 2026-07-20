using System.Threading.Tasks;

using Fx.Net.Errors;
using Fx.Net.Monads.Result;

namespace Fx.Net.Tests.ResultTests;

public class ResultCachingTests
{
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
    public void NullValueTask_MultipleCalls_ReturnsSameCachedInstanceReference()
    {
        var firstCall = Result.NullValueTask;
        var secondCall = Result.NullValueTask;

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
    public async Task NullValueTask_Returns_Task_WithNullValueError()
    {
        var result = Result.NullValueTask;

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
}