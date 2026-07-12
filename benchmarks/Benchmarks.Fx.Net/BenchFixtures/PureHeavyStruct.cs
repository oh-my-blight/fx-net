using System;
using System.Runtime.InteropServices;

namespace Benchmarks.Fx.Net.BenchFixtures;

[StructLayout(LayoutKind.Sequential)]
public record struct PureHeavyStruct
{
    public Guid Id1;
    public Guid Id2;
    public DateTime Date1;
    public DateTime Date2;
    public long Counter;

    public PureHeavyStruct(Guid id1, Guid id2, DateTime date1, DateTime date2, long counter)
    {
        Id1 = id1;
        Id2 = id2;
        Date1 = date1;
        Date2 = date2;
        Counter = counter;
    }
}