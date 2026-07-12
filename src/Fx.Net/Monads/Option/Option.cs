using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Fx.Net.Types;

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
    private static readonly Option<T> _none = default;
    internal static readonly Task<Option<T>> _noneTask = Task.FromResult(_none);

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Option(in T value)
    {
        _value = value;
        _monadState = MonadState.Some;
    }

    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // private Option(in MonadState monadState)
    // {
    //     _value = default;
    //     _monadState = monadState;
    // }


    /// <summary>
    ///     Выполняет неявное преобразование токена отсутствия значения в пустой <see cref="Option{T}"/>.
    /// </summary>
    /// <param name="_">Токен отсутствия значения.</param>
    /// <returns>Пустой экземпляр <see cref="Option{T}"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Option<T>(NoneToken _) => _none;

    /// <inheritdoc />
    public bool Equals(Option<T> other)
    {
        if (_monadState != other._monadState) return false;

        if (_monadState == MonadState.None) return true;

        return EqualityComparer<T?>.Default.Equals(_value, other._value);
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
        return _monadState == MonadState.Some
            ? EqualityComparer<T?>.Default.GetHashCode(_value!)
            : (int)MonadState.None;
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
    ///     Кэшированный экземпляр успешного выполнения, не содержащий полезной нагрузки.
    /// </summary>
    private static readonly Option<Unit> SomeUnit = new(Unit.Value);

    /// <summary>
    ///     Кэшированная задача, содержащая успешный пустой результат <see cref="Option{Unit}"/>.
    /// </summary>
    /// <value>
    ///     Объект <see cref="Task{T}"/>.
    /// </value>
    /// <remarks>
    ///     Исключает повторные аллокации объектов <see cref="Task{T}"/> в управляемой куче при частом 
    ///     синхронном завершении асинхронных операций, не возвращающих значения.
    /// </remarks>
    public static Task<Option<Unit>> SomeTask { get; } = Task.FromResult(SomeUnit);

    /// <summary>
    ///     Возвращает кэшированный успешный экземпляр <see cref="Option{Unit}"/>.
    /// </summary>
    /// <returns>Экземпляр <see cref="Option{Unit}"/> в состоянии <see cref="MonadState.Some"/>.</returns>
    public static Option<Unit> Some() => SomeUnit;

    /// <summary>
    ///     Создает экземпляр <see cref="Option{T}"/>, содержащий указанное значение.
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого значения.</typeparam>
    /// <param name="value">Значение, помещаемое в контейнер.</param>
    /// <returns>
    ///     Объект <see cref="Option{T}"/> со значением, если параметр <paramref name="value"/> не равен <see langword="null"/>; 
    ///     в противном случае — пустой <see cref="Option{T}"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some<T>(in T value)
    {
        if (value is not null) return new Option<T>(in value);

        return None;
    }

    /// <summary>
    ///     Возвращает маркер отсутствия значения.
    /// </summary>
    /// <value>
    ///     Экземпляр <see cref="NoneToken"/>, готовый к неявному приведению.
    /// </value>
    public static NoneToken None => new();


    /// <summary>
    ///     Возвращает кэшированную задачу, содержащую пустой <see cref="Option{T}"/>.
    /// </summary>
    /// <typeparam name="T">Тип инкапсулированного значения.</typeparam>
    /// <returns>
    ///     Повторно используемый экземпляр <see cref="Task{T}"/>, содержащий <see cref="Option{T}"/> 
    ///     в состоянии <see cref="MonadState.None"/>.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         Данный метод спроектирован для обеспечения нулевых алокация в высоконагруженных асинхронных конвейерах. 
    ///         Когда асинхронная операция завершается синхронно по причине отсутствия данных (например, промах мимо кэша, 
    ///         пустой ответ из репозитория или провал валидации), возврат этого кэша полностью исключает 
    ///         выделение памяти под объект <see cref="Task"/> в управляемой куче
    ///     </para>
    ///     <para>
    ///         Экземпляр задачи инициализируется лениво — ровно один раз в рамках статического конструктора 
    ///         обобщенной структуры <see cref="Option{T}"/> для каждого уникального закрытого типа <typeparamref name="T"/>.
    ///     </para>
    /// </remarks>
    public static Task<Option<T>> NoneTask<T>() => Option<T>._noneTask;
}