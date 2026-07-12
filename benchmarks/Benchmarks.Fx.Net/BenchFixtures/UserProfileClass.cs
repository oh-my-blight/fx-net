using System;

namespace Benchmarks.Fx.Net.BenchFixtures;

public record class UserProfileClass(Guid Id, string Email, string FullName, DateTime CreatedAt);