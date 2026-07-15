using System;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

using Benchmarks.Fx.Net.BenchFixtures;

using Fx.Net.Monads.Option;

namespace Benchmarks.Fx.Net.OptionBench;

[MemoryDiagnoser]
[HardwareCounters(HardwareCounter.TotalCycles, HardwareCounter.LlcMisses)]
[DisassemblyDiagnoser(exportHtml: true, maxDepth: 3)]
public class OptionHeavyPayloadBenchmarks
{
    private readonly Guid _testId = Guid.NewGuid();
    private readonly DateTime _testDate = DateTime.UtcNow;
    private readonly long _testFlags = 0xCAFEEBABE;
    private readonly string _testEmail = "example@gmail.com";
    private readonly string _testName = "John";
    private readonly string _testRole = "Contributor";


    [Benchmark(Baseline = true)]
    public Task<Option<HeavyClassPayload>> Class_In_Task()
    {
        var payload = new HeavyClassPayload(_testId, _testEmail, _testName, _testRole, _testDate, _testFlags);
        return Task.FromResult(Option.Some(payload));
    }

    [Benchmark]
    public ValueTask<Option<HeavyClassPayload>> Class_In_ValueTask()
    {
        var payload = new HeavyClassPayload(_testId, _testEmail, _testName, _testRole, _testDate, _testFlags);
        return new ValueTask<Option<HeavyClassPayload>>(Option.Some(payload));
    }

    [Benchmark]
    public ValueTask<Option<HeavyStructPayload>> Struct_InValueTask()
    {
        var payload = new HeavyStructPayload(_testId, _testDate, _testFlags, _testEmail, _testName, _testRole);

        return new ValueTask<Option<HeavyStructPayload>>(Option.Some(payload));
    }

    [Benchmark]
    public ValueTask<Option<HeavyClassPayload>> None_Class_ValueTask()
    {
        return new ValueTask<Option<HeavyClassPayload>>(Option.None);
    }

    [Benchmark]
    public ValueTask<Option<HeavyStructPayload>> None_Struct_ValueTask()
    {
        return new ValueTask<Option<HeavyStructPayload>>(Option.None);
    }
}