using System;

namespace Benchmarks.Fx.Net.BenchFixtures;

public record class HeavyClassPayload(
    Guid Id,
    string Email,
    string Name,
    string Role,
    DateTime CreatedAt,
    long Flags);