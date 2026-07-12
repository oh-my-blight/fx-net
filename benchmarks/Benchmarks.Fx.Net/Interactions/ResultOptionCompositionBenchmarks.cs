using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Fx.Net.Monads.Option;
using Fx.Net.Monads.Result;
using Fx.Net.Types;

namespace Benchmarks.Fx.Net.Interactions;

[MemoryDiagnoser]
public class ResultOptionCompositionBenchmarks
{
    private static readonly Error CriticalError = new("SYS001", "Database timeout");
    private static readonly Result<Option<Unit>> CachedNestedSuccess = Result.Success<Option<Unit>>(Option.Some());


    [Benchmark(Baseline = true, Description = "Nested: Success + Some(Unit)")]
    public Result<Option<Unit>> Return_Success_With_SomeUnit()
    {
        return Result.Success(Option.Some());
    }

    [Benchmark(Description = "Nested: Success + None")]
    public Result<Option<Unit>> Return_Success_With_None()
    {
        return Result.Success<Option<Unit>>(Option.None);
    }

    [Benchmark(Description = "Nested: Failure(Error)")]
    public Result<Option<Unit>> Return_FailureState()
    {
        return Result.Failure(CriticalError);
    }

    [Benchmark(Description = "Nested Task: Success + SomeTask(Unit)")]
    public Result<Task<Option<Unit>>> Return_Success_Task_With_SomeTask()
    {
        return Result.Success(Option.SomeTask());
    }

    [Benchmark(Description = "Nested Task: Success + None")]
    public Result<Task<Option<Unit>>> Return_Success_Task_WitNone()
    {
        return Result.Success(Option.NoneTask<Unit>());
    }

    [Benchmark(Description = "Nested ValueTask: Success + SomeTask(Unit)")]
    public Result<ValueTask<Option<Unit>>> Return_Success_ValueTaskWithUnit()
    {
        return Result.Success(ValueTask.FromResult(Option.Some()));
    }


    [Benchmark(Description = "Flat Option: Some(Unit)")]
    public Option<Unit> Flat_Option_Some()
    {
        return Option.Some();
    }

    [Benchmark(Description = "Flat Option: None")]
    public Option<Unit> Flat_Option_None()
    {
        return Option.None;
    }

    [Benchmark(Description = "Flat Result: Success(Unit)")]
    public Result<Unit> Flat_Result_Success()
    {
        return Result.Success();
    }

    [Benchmark(Description = "Flat Result: Failure(Unit)")]
    public Result<Unit> Flat_Result_Failure()
    {
        return Result.Failure(CriticalError);
    }


    [Benchmark(Description = "Cache: Unit.SuccessfulTask")]
    public Task<Unit> Cache_Unit_Task()
    {
        return Unit.SuccessfulTask;
    }

    [Benchmark(Description = "Cache: Unit.SuccessfulValueTask")]
    public ValueTask<Unit> Cache_Unit_ValueTask()
    {
        return ValueTask.FromResult(Unit.Value);
    }

    [Benchmark(Description = "Cache: Result.SuccessTask")]
    public Task<Result<Unit>> Cache_Result_SuccessTask()
    {
        return Result.SuccessTask;
    }

    [Benchmark(Description = "Cache: Result.NullValueTask")]
    public Task<Result<Unit>> Cache_Result_NullValueTask()
    {
        return Result.NullValueTask;
    }

    [Benchmark(Description = "Cache: Result.ChainBrokenTask")]
    public Task<Result<Unit>> Cache_Result_ChainBrokenTask()
    {
        return Result.ChainBrokenTask;
    }


    [Benchmark(Description = "Outer Task: Task<Result<Option<Unit>>>")]
    public Task<Result<Option<Unit>>> Outer_Task_Nested_Success()
    {
        return Task.FromResult(CachedNestedSuccess);
    }

    [Benchmark(Description = "Outer ValueTask: ValueTask<Result<Option<Unit>>>")]
    public ValueTask<Result<Option<Unit>>> Outer_ValueTask_Nested_Success()
    {
        return new ValueTask<Result<Option<Unit>>>(CachedNestedSuccess);
    }
}