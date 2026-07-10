using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Fx.Net.Monads.Option;
using Fx.Net.Types;

namespace Benchmarks.Fx.Net.OptionBench;

[MemoryDiagnoser]
public class OptionBenchmarks
{
    [Benchmark(Baseline = true)]
    public Task<Option<Unit>> Return_New_Task()
    {
        return Task.FromResult(Option.Some());
    }

    [Benchmark]
    public Task<Option<Unit>> Returned_Cached_Task()
    {
        return Option.SomeTask;
    }

    [Benchmark]
    public ValueTask<Option<Unit>> Return_New_ValueTask()
    {
        return new ValueTask<Option<Unit>>(Option.SomeTask);
    }
}