using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Fx.Net.Monads.Result;
using Fx.Net.Types;

namespace Benchmarks.Fx.Net;

[MemoryDiagnoser]
public class ResultBenchmarks
{
    [Benchmark(Baseline = true)]
    public Task<Result<Unit>> Return_New_Task()
    {
        return Task.FromResult(Result.Success());
    }

    [Benchmark]
    public Task<Result<Unit>> Returned_Cached_Task()
    {
        return Result.SuccessTask;
    }

    [Benchmark]
    public ValueTask<Result<Unit>> Return_New_ValueTask()
    {
        return new ValueTask<Result<Unit>>(Result.Success());
    }
}