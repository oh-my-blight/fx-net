using System;
using System.Runtime.CompilerServices;

namespace Fx.Net.Monads.Option.Extensions;

public static class OptionExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> ToOption<T>(this T? value) where T : class
    {
        return value is null
            ? Option.None
            : Option.Some(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> ToOption<T>(in this T? value) where T : struct
    {
        return value.HasValue
            ? Option.Some(value.Value)
            : Option.None;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<U> Map<T, U>(
        in this Option<T> option, 
        in Func<T, U> mapper)
    {
        if (option.IsNone) return Option.None;

        return Option.Some(mapper(option.Value!));
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TNextValue> Bind<T, TNextValue>(
        in this Option<T> option, 
        in Func<T, Option<TNextValue>> binder)
    {
        if (option.IsNone) return Option.None;

        return binder(option.Value!);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Do<T>(in this Option<T> option, in Action<T> action)
    {
        if (option.HasValue)
        {
            action(option.Value!);
        }

        return option;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? UnwrapOr<T>(in this Option<T> option, in T defaultValue)
    {
        if (option.IsNone) return defaultValue;

        return option.Value;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Match<T, TResult>(
        in this Option<T> option,
        in Func<T, TResult> onSome,
        in Func<TResult> onNone)
    {
        if (option.HasValue) return onSome(option.Value!);

        return onNone();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> IfNone<T>(this Option<T> option, Action onNone)
    {
        if (option.IsNone)
        {
            onNone();
        }

        return option;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> IfNone<T>(this Option<T> option, in Option<T> fallbackOption)
    {
        if (option.HasValue) return option;

        return fallbackOption;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> IfNone<T>(this Option<T> option, in Func<Option<T>> fallbackFactory)
    {
        if (option.HasValue) return option;
        return fallbackFactory();
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Filter<T>(this in Option<T> option, in Func<T, bool> predicate)
    {
        if (!option.HasValue || !predicate(option.Value))
        {
            return Option.None;
        }

        return option;
    }
}