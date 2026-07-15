using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Fx.Net.Errors;
using Fx.Net.Types;

namespace Fx.Net.Monads.Result;

/// <summary>
///     Фиксированный контейнер, инкапсулирующий результат выполнения операции, содержащий либо успешно вычисленное значение, либо объект ошибки.
/// </summary>
/// <typeparam name="T">    Тип успешного значения, инкапсулированного в контейнер.</typeparam>
/// <remarks>
/// <para>
///     Гарантирует, что объект всегда находится в одном из двух взаимоисключающих состояний: <see cref="IsSuccess"/> или <see cref="IsFailure"/>.
/// </para>
/// <para>
///     <b>Поведение по умолчанию:</b> Инициализация структуры через <c>default(Result&lt;T&gt;)</c> или конструктор без параметров 
///     приводит к созданию невалидного провального состояния. При этом свойство <see cref="Error"/> автоматически вернет 
///     предопределенную ошибку <see cref="ResultErrors.DefaultNullFailure"/>.
/// </para>
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
public readonly struct Result<T> : IEquatable<Result<T>>
{
    private readonly T? _value;
    private readonly Error _error;
    private readonly MonadState _state;


    /// <summary>
    ///     Возвращает значение, указывающее, завершилась ли операция успешно.
    /// </summary>
    public bool IsSuccess => _state == MonadState.Some;

    /// <summary>
    ///     Возвращает значение, указывающее, завершилась ли операция с ошибкой.
    /// </summary>
    public bool IsFailure => _state != MonadState.Some;

    /// <summary>
    ///     Возвращает объект ошибки, описывающий причину сбоя операции.
    /// </summary>
    /// <remarks>
    ///     Если <see cref="IsSuccess"/> равен <see langword="true"/>, всегда возвращает <see cref="Error.None"/>. 
    ///     Если контейнер был инициализирован некорректно (через <c>default</c>), возвращает <see cref="ResultErrors.DefaultNullFailure"/>.
    /// </remarks>
    public Error Error => _state switch
    {
        MonadState.Some => Error.None,
        MonadState.Failure => _error,
        _ => ResultErrors.DefaultNullFailure
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Result(in T value)
    {
        _value = value;
        _error = default;
        _state = MonadState.Some;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Result(Error error)
    {
        _value = default;
        _error = error.Code is not null ? error : ResultErrors.DefaultNullFailure;
        _state = MonadState.Failure;
    }

    /// <summary>
    ///     Выполняет безопасное извлечение значения из контейнера в случае успешного завершения операции.
    /// </summary>
    /// <param name="value">    При успешном завершении содержит извлеченное значение; в противном случае — значение по умолчанию для типа <typeparamref name="T"/>.</param>
    /// <returns>   <see langword="true"/>, если операция завершилась успешно (<see cref="IsSuccess"/> равен <see langword="true"/>); в противном случае — <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetSuccess([NotNullWhen(true)] out T? value)
    {
        value = _value;
        return IsSuccess;
    }


    /// <summary>
    ///     Разбивает экземпляр <see cref="Result{T}"/> на составляющие компоненты.
    /// </summary>
    /// <param name="isSuccess">    Флаг успешного завершения операции.</param>
    /// <param name="value">    Инкапсулированное значение (доступно, если <paramref name="isSuccess"/> равен <see langword="true"/>).</param>
    /// <param name="error">    Объект ошибки (доступен, если <paramref name="isSuccess"/> равен <see langword="false"/>).</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Deconstruct(out bool isSuccess, [NotNullWhen(true)] out T? value, out Error error)
    {
        isSuccess = IsSuccess;
        value = _value;
        error = Error;
    }


    /// <summary>
    ///     Выполняет неявное приведение типа <see cref="FailedResult"/> к объекту контейнера <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="failedResult"> Объект промежуточного неудачного результата.</param>
    /// <returns>   Новый экземпляр <see cref="Result{T}"/> в состоянии сбоя со скопированным объектом ошибки.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<T>(FailedResult failedResult) => new(failedResult.Error);

    /// <summary>
    ///     Определяет, равен ли текущий экземпляр <see cref="Result{T}"/> другому объекту того же типа.
    /// </summary>
    /// <param name="other">    Контейнер для сравнения с текущим экземпляром.</param>
    /// <returns>   <see langword="true"/>, если сравниваемые объекты имеют одинаковый флаг успеха, эквивалентные значения и ошибки; иначе — <see langword="false"/>.</returns>
    public bool Equals(Result<T> other)
    {
        return IsSuccess == other.IsSuccess &&
               EqualityComparer<T>.Default.Equals(_value, other._value) &&
               Error == other.Error;
    }

    /// <inheritdoc cref="Equals(Result{T})"/>
    public override bool Equals(object? obj) => obj is Result<T> other && Equals(other);

    /// <summary>
    ///     Возвращает хэш-код текущего экземпляра.
    /// </summary>
    /// <returns>   32-битный знаковый хэш-код, вычисленный на основе флага успеха, значения и ошибки.</returns>
    public override int GetHashCode() => HashCode.Combine(_value, IsSuccess, Error);

    /// <summary>
    ///     Сравнивает два экземпляра <see cref="Result{T}"/> на равенство.
    /// </summary>
    /// <param name="left"> Первый экземпляр для сравнения.</param>
    /// <param name="right">    Второй экземпляр для сравнения.</param>
    /// <returns>   <see langword="true"/>, если экземпляры эквивалентны; в противном случае — <see langword="false"/>.</returns>
    public static bool operator ==(Result<T> left, Result<T> right) => left.Equals(right);

    /// <summary>
    ///     Сравнивает два экземпляра <see cref="Result{T}"/> на неравенство.
    /// </summary>
    /// <param name="left"> Первый экземпляр для сравнения.</param>
    /// <param name="right">    Второе экземпляр для сравнения.</param>
    /// <returns>   <see langword="true"/>, если экземпляры не эквивалентны; в противном случае — <see langword="false"/>.</returns>
    public static bool operator !=(Result<T> left, Result<T> right) => !(left == right);
}

/// <summary>
///     Вспомогательная структура-маркер для передачи типизированной ошибки через неявное приведение типов.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct FailedResult
{
    internal Error Error { get; }
    internal FailedResult(Error error) => Error = error;
}

/// <summary>
///     Статическая фабрика для инициализации и работы с объектами <see cref="Result{T}"/>.
/// </summary>
public static class Result
{
    private static readonly Result<Unit> SuccessUnit = new(Unit.Value);


    /// <summary>
    ///     Кэшированный объект <see cref="Task{T}"/>, содержащий успешный пустой результат <see cref="Result{Unit}"/>.
    /// </summary>
    /// <remarks>
    ///     Исключает повторные аллокации в управляемой куче при частых синхронных возвратах из асинхронных методов.
    /// </remarks>
    public static Task<Result<Unit>> SuccessTask { get; } = Task.FromResult(SuccessUnit);

    /// <summary>
    ///     Возвращает кэшированный экземпляр успешного выполнения, не содержащий полезной нагрузки.
    /// </summary>
    /// <returns>   Экземпляр <see cref="Result{Unit}"/> в успешном состоянии.</returns>
    public static Result<Unit> Success() => SuccessUnit;

    /// <summary>
    ///     Формирует промежуточный неудачный результат на основе переданного объекта ошибки.
    /// </summary>
    /// <param name="error">    Объект ошибки, описывающий причину сбоя.</param>
    /// <returns>   Экземпляр <see cref="FailedResult"/>, готовый к неявному приведению в контейнер <see cref="Result{T}"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FailedResult Failure(in Error error) => new(error);


    /// <summary>
    ///     Создает успешный контейнер <see cref="Result{T}"/>, инкапсулирующий полезную нагрузку.
    /// </summary>
    /// <typeparam name="T">    Тип возвращаемого значения.</typeparam>
    /// <param name="value">    Значение успешной операции.</param>
    /// <returns>
    ///     Успешный экземпляр <see cref="Result{T}"/>, если <paramref name="value"/> инициализирован; 
    ///     в противном случае — неудачный контейнер с ошибкой <see cref="ResultErrors.NullValue"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T> Success<T>(in T value)
    {
        if (value is null) return Failure(ResultErrors.NullValue);

        return new Result<T>(in value);
    }

    /// <summary>
    ///     Кэшированный объект <see cref="Task{T}"/>, содержащий сбой инициализации из-за значения <see langword="null"/>.
    /// </summary>
    public static Task<Result<Unit>> NullValueTask { get; } =
        Task.FromResult<Result<Unit>>(Failure(ResultErrors.NullValue));

    /// <summary>
    ///     Кэшированный объект <see cref="Task{T}"/>, сигнализирующий о прерывании цепочки вычислений.
    /// </summary>
    public static Task<Result<Unit>> ChainBrokenTask { get; } =
        Task.FromResult<Result<Unit>>(Failure(ResultErrors.NullResult));
}