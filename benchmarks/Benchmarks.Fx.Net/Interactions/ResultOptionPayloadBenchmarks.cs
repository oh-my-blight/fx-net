using Benchmarks.Fx.Net.BenchFixtures;

using Fx.Net.Monads.Option;
using Fx.Net.Monads.Result;

namespace Benchmarks.Fx.Net.Interactions;

using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;


[MemoryDiagnoser]
public class ResultOptionPayloadBenchmarks
{
    private static readonly HeavyClassPayload SharedClass = new(
        Guid.NewGuid(), 
        "user@fx.net", 
        "John Doe", 
        "Admin", 
        DateTime.UtcNow, 
        0x_7FFF_FFFF
    );

    private static readonly HeavyStructPayload SharedStruct = new(
        Guid.NewGuid(), 
        DateTime.UtcNow, 
        0x_7FFF_FFFF, 
        "user@fx.net", 
        "John Doe", 
        "Admin"
    );

   
    [Benchmark(Baseline = true, Description = "Class: Direct Return (No Sugar)")]
    public Result<Option<HeavyClassPayload>> Class_Direct()
    {
        return Result.Success(Option.Some(SharedClass));
    }

    [Benchmark(Description = "Class: Async ValueTask (With Sugar)")]
    public async ValueTask<Result<Option<HeavyClassPayload>>> Class_AsyncSugar()
    {
        return Result.Success(Option.Some(SharedClass));
    }



    [Benchmark(Description = "Struct: Direct Return (No Sugar)")]
    public Result<Option<HeavyStructPayload>> Struct_Direct()
    {
        return Result.Success(Option.Some(SharedStruct));
    }

    [Benchmark(Description = "Struct: Async ValueTask (With Sugar)")]
    public async ValueTask<Result<Option<HeavyStructPayload>>> Struct_AsyncSugar()
    {
        return Result.Success(Option.Some(SharedStruct));
    }
        

    [Benchmark(Description = "Class: async ValueTask")]
    public async ValueTask<Result<Option<HeavyClassPayload>>> Class_AsyncValueTask()
    {
        return Result.Success(Option.Some(SharedClass));
    }

    [Benchmark(Description = "Class: async Task")]
    public async Task<Result<Option<HeavyClassPayload>>> Class_AsyncTask()
    {
        return Result.Success(Option.Some(SharedClass));
    }
}