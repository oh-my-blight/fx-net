using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Fx.Net.Monads.Option;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Option<T> : IEquatable<Option<T>>
{
    private readonly T? _value;
    private readonly State State;

    public bool HasValue => State == State.Some;
    public bool IsNone => State == State.None;


    public Option(in T value)
    {
        _value = value;
        State = State.Some;
    }

    private Option(in State state)
    {
        _value = default;
        State = state;
    }


    public static implicit operator Option<T>(NoneToken _) => new(State.None);

    public bool Equals(Option<T> other)
    {
        return EqualityComparer<T?>.Default.Equals(_value, other._value) && State == other.State;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Option<T> other && Equals(other);
    }

    public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);

    public static bool operator !=(Option<T> left, Option<T> right) => !(left == right);

    public override int GetHashCode()
    {
        return HashCode.Combine(_value, (int)State);
    }
}

public readonly struct NoneToken
{
}

public static class Option
{
    public static Option<T> Some<T>(in T value)
    {
        if (value is not null) return new(value);

        return None;
    }

    public static NoneToken None => new();
}