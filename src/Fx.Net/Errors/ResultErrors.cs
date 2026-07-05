using Fx.Net.Monads.Result;
using Fx.Net.Types;

namespace Fx.Net.Errors;

/// <summary>
///     Набор предопределенных объектов ошибок для работы c контейнером <see cref="Result{T}"/>
/// </summary>
public static class ResultErrors
{
    /// <summary>
    ///     Ошибка, указывающая на прерывание цепочки вычислений из-за возврата некорректного или пустого состояния.
    /// </summary>
    public static readonly Error NullResult = new(
        "Result.ChainBroken",
        "Цепочка вычислений прервана: один из этапов вернул пустое или некорректное состояние.");


    /// <summary>
    ///     Ошибка, возникающая при попытке инициализировать успешный результат значением <see langword="null"/>.
    /// </summary>
    public static readonly Error NullValue = new(
        "Result.NullValue",
        "Попытка инициализировать успешный результат null значением.");

    /// <summary>
    ///     Ошибка, указывающая на обращение к неинициализированному экземпляру Result, созданному через <c>default</c>.
    /// </summary>
    public static readonly Error DefaultNullFailure = new(
        "Result.InvalidInitialization",
        "Использование неинициализированного объекта Result, созданного через 'default'.");
}