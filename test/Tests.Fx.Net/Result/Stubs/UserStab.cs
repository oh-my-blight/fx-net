using System;

namespace Tests.Fx.Net.Result.Stubs;

internal class UserStab : IEquatable<UserStab>
{
    public int Id { get; }
    public string Name { get; }
    public int Age { get; }

    public UserStab(int id, string name, int age)
    {
        Id = id;
        Name = name;
        Age = age;
    }

    public bool Equals(UserStab? other)
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
        return Equals((UserStab)obj);
    }

    public static bool operator ==(UserStab left, UserStab right) => left.Equals(right);

    public static bool operator !=(UserStab left, UserStab right) => !(left == right);


    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Age);
    }
}