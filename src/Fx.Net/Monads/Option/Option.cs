using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Fx.Net.Monads.Option;

/// <summary>
///     Представляет неизменяемый предикативный контейнер, который может содержать или не содержать значение.
/// </summary>
/// <typeparam name="T">Тип инкапсулированного значения.</typeparam>
/// <remarks>
///     <para>
///         Структура гарантирует атомарность своего состояния: объект всегда находится либо в состоянии 
///         <see cref="HasValue"/>, либо <see cref="IsNone"/>. Прямое выделение памяти в куче при создании отсутствует.
///     </para>
///     <para>
///         Для инициализации структуры рекомендуется использовать статический класс <see cref="Option"/>.
///     </para>
/// </remarks>
/// <example>
///     <code>
///     // 1. Инициализация через фабрику
///     Option&lt;string&gt; format = Option.Some("json");
///     Option&lt;string&gt; empty = Option.None;
///     
///     // 2. Неявное приведение из NoneToken
///     Option&lt;int&gt; GetUserAge(string id)
///     {
///         if (id == null) return Option.None; // Неявное приведение NoneToken -> Option&lt;T&gt;
///         return Option.Some(25);
///     }
///     </code>
/// </example>
[StructLayout(LayoutKind.Sequential)]
public readonly struct Option<T> : IEquatable<Option<T>>
{
    private readonly T? _value;
    private readonly MonadState _monadState;

    /// <summary>
    ///     Указывает, содержит ли текущий контейнер валидное значение.
    /// </summary>
    /// <value>
    ///     <see langword="true"/>, если контейнер содержит значение; иначе — <see langword="false"/>.
    /// </value>
    public bool HasValue => _monadState == MonadState.Some;

    /// <summary>
    ///     Указывает, пуст ли текущий контейнер.
    /// </summary>
    /// <value>
    ///     <see langword="true"/>, если контейнер пуст; иначе — <see langword="false"/>.
    /// </value>
    public bool IsNone => _monadState == MonadState.None;


    internal Option(in T value)
    {
        _value = value;
        _monadState = MonadState.Some;
    }

    private Option(in MonadState monadState)
    {
        _value = default;
        _monadState = monadState;
    }


    /// <summary>
    ///     Выполняет неявное преобразование токена отсутствия значения в пустой <see cref="Option{T}"/>.
    /// </summary>
    /// <param name="_">Токен отсутствия значения.</param>
    /// <returns>Пустой экземпляр <see cref="Option{T}"/>.</returns>
    public static implicit operator Option<T>(NoneToken _) => new(MonadState.None);

    /// <inheritdoc />
    public bool Equals(Option<T> other)
    {
        return EqualityComparer<T?>.Default.Equals(_value, other._value) && _monadState == other._monadState;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Option<T> other && Equals(other);
    }

    /// <summary>
    ///     Определяет равенство двух экземпляров <see cref="Option{T}"/>.
    /// </summary>
    public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);

    /// <summary>
    ///     Определяет неравенство двух экземпляров <see cref="Option{T}"/>.
    /// </summary>
    public static bool operator !=(Option<T> left, Option<T> right) => !(left == right);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(_value, (int)_monadState);
    }
}

/// <summary>
///     Структурный маркер для обозначения пустого состояния <see cref="Option{T}"/>.
/// </summary>
/// <remarks>
///     Используется для возврата пустого значения через неявное приведение типов.
/// </remarks>
public readonly struct NoneToken
{
}

/// <summary>
///     Статический класс для инициализации и работы с контейнером <see cref="Option{T}"/>.
/// </summary>
public static class Option
{
    /// <summary>
    ///     Создает экземпляр <see cref="Option{T}"/>, содержащий указанное значение.
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого значения.</typeparam>
    /// <param name="value">Значение, помещаемое в контейнер.</param>
    /// <returns>
    ///     Объект <see cref="Option{T}"/> со значением, если параметр <paramref name="value"/> не равен <see langword="null"/>; 
    ///     в противном случае — пустой <see cref="Option{T}"/>.
    /// </returns>
    public static Option<T> Some<T>(in T value)
    {
        if (value is not null) return new Option<T>(value);

        return None;
    }

    /// <summary>
    ///     Возвращает маркер отсутствия значения.
    /// </summary>
    /// <value>
    ///     Экземпляр <see cref="NoneToken"/>, готовый к неявному приведению.
    /// </value>
    public static NoneToken None => new();
}