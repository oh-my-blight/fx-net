using System.Threading.Tasks;

using Fx.Net.Monads.Option;

namespace Fx.Net.Tests.OptionTests;

public class OptionCachingTests
{
    [Fact]
    public async Task NoneTask_CalledMultipleTimes_ReturnsExactlySameTaskInstance()
    {
        var firstTask = Option.NoneTask<string>();
        var secondTask = Option.NoneTask<string>();

        var result = await secondTask;

        Assert.Same(firstTask, secondTask);
        Assert.True(firstTask.IsCompleted);
        Assert.True(result.IsNone);
    }

    [Fact]
    public async Task SomeTask_CalledMultipleTimes_ReturnsExactlySameTaskInstance()
    {
        var firstTask = Option.SomeTask();
        var secondTask = Option.SomeTask();

        var firstResult = await firstTask;
        var secondResult = await secondTask;

        Assert.Same(firstTask, secondTask);
        Assert.True(firstTask.IsCompleted);
        Assert.True(firstResult.HasValue);
    }
}