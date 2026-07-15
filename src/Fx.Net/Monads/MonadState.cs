namespace Fx.Net.Monads;

/// <summary>
///     Определяет внутреннее дискретное состояние контейнера данных.
/// </summary>
public enum MonadState : byte
{
    /// <summary>
    ///     Контейнер пуст.
    /// </summary>
    None = 0,

    /// <summary>
    ///     Контейнер содержит валидное значение.
    /// </summary>
    Some = 1,

    /// <summary>
    ///     Контейнер находится в состоянии системной или вычислительной ошибки.
    /// </summary>
    Failure = 2
}