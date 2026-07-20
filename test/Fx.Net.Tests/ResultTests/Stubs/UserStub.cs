using System;

namespace Fx.Net.Tests.ResultTests.Stubs;

internal class UserStub : IEquatable<UserStub>
{
    public int Id { get; }
    public string Name { get; }
    public int Age { get; }

    public UserStub(int id, string name, int age)
    {
        Id = id;
        Name = name;
        Age = age;
    }

    public bool Equals(UserStub? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return (Id == other.Id) || Name == other.Name && Age == other.Age;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((UserStub)obj);
    }

    public static bool operator ==(UserStub left, UserStub right) => left.Equals(right);

    public static bool operator !=(UserStub left, UserStub right) => !(left == right);


    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Age);
    }
}