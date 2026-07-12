using System;
using System.Runtime.InteropServices;

namespace Benchmarks.Fx.Net.BenchFixtures;

[StructLayout(LayoutKind.Sequential)]
public record struct HeavyStructPayload
{
    public Guid Id;
    public DateTime CreatedAt;
    public long Flags;
    public string Email;
    public string Name;
    public string Role;

    public HeavyStructPayload(Guid id, DateTime createdAt, long flags, string email, string name, string role)
    {
        Id = id;
        CreatedAt = createdAt;
        Flags = flags;
        Email = email;
        Name = name;
        Role = role;
    }
}