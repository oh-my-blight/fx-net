using Benchmarks.Fx.Net.BenchFixtures;

using Fx.Net.Monads.Option;

namespace Benchmarks.Fx.Net.OptionBench;

using System;
using System.Runtime.InteropServices;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

[MemoryDiagnoser]
[HardwareCounters(HardwareCounter.TotalCycles, HardwareCounter.LlcMisses)]
[DisassemblyDiagnoser(exportHtml: true, maxDepth: 3)]
public class OptionEqualsBenchmarks
{
    private static readonly Guid TargetGuid1 = Guid.NewGuid();
    private static readonly Guid TargetGuid2 = Guid.NewGuid();
    private static readonly DateTime TargetDate1 = DateTime.UtcNow;
    private static readonly DateTime TargetDate2 = DateTime.UtcNow;

    private readonly PureHeavyStruct _pureStruct1 = new(TargetGuid1, TargetGuid2, TargetDate1, TargetDate2, 42L);
    private readonly PureHeavyStruct _pureStruct2 = new(TargetGuid1, TargetGuid2, TargetDate1, TargetDate2, 42L);

    private readonly PureHeavyStruct
        _pureStructDiff =
            new(TargetGuid1, TargetGuid2, TargetDate1, TargetDate2, 99L);

    private readonly HeavyStructWithRefs _structWithRefs1 = new(TargetGuid1, TargetDate1, "test@fx.net", "Architect");
    private readonly HeavyStructWithRefs _structWithRefs2 = new(TargetGuid1, TargetDate1, "test@fx.net", "Architect");

    private Option<PureHeavyStruct> _pureSome1;
    private Option<PureHeavyStruct> _pureSome2;
    private Option<PureHeavyStruct> _pureSomeDiff;
    private Option<PureHeavyStruct> _pureNone1;
    private Option<PureHeavyStruct> _pureNone2;

    private Option<HeavyStructWithRefs> _refsSome1;
    private Option<HeavyStructWithRefs> _refsSome2;
    private Option<HeavyStructWithRefs> _refsNone;

    [GlobalSetup]
    public void Setup()
    {
        _pureSome1 = Option.Some(_pureStruct1);
        _pureSome2 = Option.Some(_pureStruct2);
        _pureSomeDiff = Option.Some(_pureStructDiff);
        _pureNone1 = Option.None;
        _pureNone2 = Option.None;

        _refsSome1 = Option.Some(_structWithRefs1);
        _refsSome2 = Option.Some(_structWithRefs2);
        _refsNone = Option.None;
    }


    [Benchmark(Baseline = true)]
    public bool PureStruct_None_Equals_None()
    {
        return _pureNone1 == _pureNone2;
    }

    [Benchmark]
    public bool StructWithRefs_None_Equals_None()
    {
        return _refsNone == Option.None;
    }


    [Benchmark]
    public bool PureStruct_Some_Equals_Some_Match()
    {
        return _pureSome1 == _pureSome2;
    }

    [Benchmark]
    public bool PureStruct_Some_Equals_Some_Mismatch()
    {
        return _pureSome1 == _pureSomeDiff;
    }


    [Benchmark]
    public bool StructWithRefs_Some_Equals_Some_Match()
    {
        return _refsSome1 == _refsSome2;
    }
}