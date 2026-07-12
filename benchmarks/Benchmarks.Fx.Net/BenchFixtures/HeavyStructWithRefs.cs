using System;
using System.Runtime.InteropServices;

namespace Benchmarks.Fx.Net.BenchFixtures;
[StructLayout(LayoutKind.Sequential)]
public record struct HeavyStructWithRefs(Guid Id, DateTime Date, string Email, string Name);