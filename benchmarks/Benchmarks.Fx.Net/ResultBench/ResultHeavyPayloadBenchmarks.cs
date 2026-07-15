using System;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

using Benchmarks.Fx.Net.BenchFixtures;

using Fx.Net.Monads.Result;

namespace Benchmarks.Fx.Net.ResultBench;

[MemoryDiagnoser]
[HardwareCounters(HardwareCounter.TotalCycles, HardwareCounter.LlcMisses)]
public class ResultHeavyPayloadBenchmarks
{
    private readonly Guid _testId = Guid.NewGuid();
    private readonly string _testEmail = "engineer@example.com";
    private readonly string _testName = "System Architect";
    private readonly DateTime _testDate = DateTime.UtcNow;


    [Benchmark(Baseline = true)]
    public Task<Result<UserProfileClass>> Return_Class_In_Task()
    {
        var payload = new UserProfileClass(_testId, _testEmail, _testName, _testDate);
        return Task.FromResult(Result.Success(payload));
    }

    [Benchmark]
    public ValueTask<Result<UserProfileClass>> Return_Class_In_ValueTask()
    {
        var payload = new UserProfileClass(_testId, _testEmail, _testName, _testDate);

        return new ValueTask<Result<UserProfileClass>>(Result.Success(payload));
    }

    [Benchmark]
    public ValueTask<Result<UserProfileStruct>> Return_Struct_In_ValueTask()
    {
        var payload = new UserProfileStruct(_testId, _testDate, _testEmail, _testName);
        return new ValueTask<Result<UserProfileStruct>>(Result.Success(payload));
    }
}