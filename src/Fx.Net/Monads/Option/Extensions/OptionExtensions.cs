using System;
using System.Runtime.CompilerServices;

namespace Fx.Net.Monads.Option.Extensions;

public static class OptionExtensions
{
    extension<T>(Option<T> option)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<U> Map<U>(Func<T, U> mapper)
        {
            if (option.IsNone) return Option.None;

            return Option.Some(mapper(option.Value!));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T? Unwrap()
        {
            return option.Value;
        }
    }
}