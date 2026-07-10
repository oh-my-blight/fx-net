namespace Fx.Net.Monads;

/// <summary>
///     Состояние контектса внутри монады
/// </summary>
public enum State : byte
{
    /// <summary>
    ///     Обозначает отсуствие значения
    /// </summary>
    None = 0,

    /// <summary>
    ///     Обозначает состояние присуствия значение
    /// </summary>
    Some = 1,

    /// <summary>
    ///     Обозначает состояние ошибки монады 
    /// </summary>
    Failure = 2
}