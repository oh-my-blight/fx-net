using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

using Fx.Net.Monads.Result;

namespace Benchmarks.Fx.Net.ResultBench;

[MemoryDiagnoser]
[HardwareCounters(HardwareCounter.TotalCycles, HardwareCounter.LlcMisses)]
public class ResultHeavyPayloadBenchmarks
{
    public record class UserProfileClass(Guid Id, string Email, string FullName, DateTime CreatedAt);

    [StructLayout(LayoutKind.Sequential)]
    public record struct UserProfileStruct
    {
        public Guid Id;
        public DateTime Date;
        public string Email;
        public string Name;

        public UserProfileStruct(Guid id, DateTime date, string email, string name)
        {
            Id = id;
            Date = date;
            Email = email;
            Name = name;
        }
    };

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