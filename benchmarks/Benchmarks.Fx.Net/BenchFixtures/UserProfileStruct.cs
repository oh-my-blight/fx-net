using System;
using System.Runtime.InteropServices;

namespace Benchmarks.Fx.Net.BenchFixtures;

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