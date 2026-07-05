using System;
using System.Runtime.InteropServices;

namespace Fx.Net.Types;

/// <summary>
///     Неизменяемая структура, представляющая код ошибки и её текстовое описание.
/// </summary>
/// <remarks>
///     Значение по умолчанию (<c>default(Error)</c>) инициализирует поля <see cref="Code"/> и <see cref="Message"/> 
///     значениями <see langword="null"/>, что отличается от предопределенного состояния <see cref="None"/>.
/// </remarks>
/// <example>
///     Для централизованного хранения и исключения динамических аллокаций рекомендуется объявлять статические контейнеры:
/// <code>
/// public static class SystemErrors    
/// {
///     public static readonly Error InvalidValidation = new("VALIDATION_001", "Неверный формат данных.");
/// }
/// </code>
/// </example>
[StructLayout(LayoutKind.Sequential)]
public readonly struct Error : IEquatable<Error>
{
    /// <summary>
    ///     Строковый код ошибки.
    /// </summary>
    /// <remarks>
    ///     Может возвращать <see langword="null"/>, если структура была создана через конструктор по умолчанию (<c>default</c>).
    /// </remarks>
    public string Code { get; }


    /// <summary>
    ///     Текстовое описание ошибки.
    /// </summary>
    /// <remarks>
    ///     Может возвращать <see langword="null"/>, если структура была создана через конструктор по умолчанию (<c>default</c>).
    /// </remarks>
    public string Message { get; }


    /// <summary>
    ///     Инициализирует новый экземпляр структуры <see cref="Error"/> с заданным кодом и сообщением.
    /// </summary>
    /// <param name="code">Строковый код ошибки.</param>
    /// <param name="message">Текстовое описание ошибки.</param>
    /// <remarks>
    ///     Конструктор не выполняет валидацию параметров на <see langword="null"/>. Передача значений 
    /// <see langword="null"/> не вызывает исключений при создании, но может привести к 
    /// <see cref="NullReferenceException"/> при последующем вызове методов сравнения или хэширования.
    /// </remarks>
    public Error(string code, string message) => (Code, Message) = (code, message);

    /// <summary>
    ///     Пустая ошибка (отсутствие ошибки) со значениями <see cref="string.Empty"/>.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);


    /// <summary>
    ///     Определяет, равен ли текущий экземпляр другой структуре <see cref="Error"/>.
    /// </summary>
    /// <param name="other">Экземпляр ошибки для сравнения.</param>
    /// <returns><see langword="true"/>, если значения <see cref="Code"/> и <see cref="Message"/> совпадают; в противном случае — <see langword="false"/>.</returns>
    /// <exception cref="NullReferenceException">
    ///     Выбрасывается, если у текущего экземпляра или параметра <paramref name="other"/> 
    ///     хотя бы одно из полей равно <see langword="null"/>.
    /// </exception>
    public bool Equals(Error other)
    {
        return Code == other.Code && Message == other.Message;
    }

    /// <inheritdoc cref="Equals(Error)"/>
    public override bool Equals(object? obj)
    {
        return obj is Error other && Equals(other);
    }

    /// <summary>
    ///     Возвращает хэш-код текущего экземпляра.
    /// </summary>
    /// <returns>   32-битный знаковый хэш-код на основе значений <see cref="Code"/> и <see cref="Message"/>.</returns>
    /// <exception cref="NullReferenceException">
    ///     Выбрасывается, если <see cref="Code"/> или <see cref="Message"/> равны <see langword="null"/>.
    /// </exception>
    public override int GetHashCode()
    {
        return HashCode.Combine(Code, Message);
    }

    /// <summary>
    ///     Сравнивает два экземпляра <see cref="Error"/> на равенство.
    /// </summary>
    /// <param name="left"> Первое сравниваемое значение.</param>
    /// <param name="right">    Второе сравниваемое значение.</param>
    /// <returns>   <see langword="true"/>, если структуры идентичны; иначе — <see langword="false"/>.</returns>
    public static bool operator ==(Error left, Error right) => left.Equals(right);

    /// <summary>
    ///     Сравнивает два экземпляра <see cref="Error"/> на неравенство.
    /// </summary>
    /// <param name="left"> Первое сравниваемое значение.</param>
    /// <param name="right">    Второе сравниваемое значение.</param>
    /// <returns>   <see langword="true"/>, если структуры отличаются; иначе — <see langword="false"/>.</returns>
    public static bool operator !=(Error left, Error right) => !(left == right);
}