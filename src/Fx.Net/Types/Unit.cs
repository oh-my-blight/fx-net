using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Fx.Net.Types;

/// <summary>
///     Обеспечивает представление типа <see cref="System.Void"/> в сценариях, 
///     где использование встроенного ключевого слова <see langword="void"/> синтаксически невозможно.
/// </summary>
/// <remarks>
///     Структура имеет минимально допустимый размер (1 байт, принудительно заданный через <see cref="StructLayoutAttribute"/>).
///     За счет применения <see cref="SkipLocalsInitAttribute"/> и передачи по readonly-ссылке минимизируются 
///     накладные расходы на стек и полностью исключаются аллокации в управляемой куче
///     при работе с высоконагруженными асинхронными конечными автоматами.
/// </remarks>
[SkipLocalsInit]
[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
{
    private static readonly Unit _value = new();

    /// <summary>
    ///     Возвращает глобальную readonly-ссылку на единственный статический экземпляр <see cref="Unit"/>.
    /// </summary>
    public static ref readonly Unit Value => ref _value;

    /// <summary>
    ///     Кэшированная задача для синхронного завершения асинхронных операций, требующих возврата <see cref="Unit"/>.
    /// </summary>
    /// <remarks>
    ///     Исключает повторные аллокации объектов <see cref="Task{T}"/> в куче при частых синхронных возвратах из асинхронных методов.
    /// </remarks>
    public static Task<Unit> SuccessfulTask { get; } = Task.FromResult(_value);

    /// <summary>
    ///     Вычисляемый экземпляр <see cref="ValueTask{T}"/> для оптимизации асинхронных путей выполнения.
    /// </summary>
    /// <remarks>
    ///     Применяется в высоконагруженных сценариях, где метод завершается синхронно в большинстве случаев, 
    ///     гарантируя нулевое выделение памяти.
    /// </remarks>
    public static ValueTask<Unit> SuccessfulValueTask => new(_value);


    /// <summary>
    ///     Указывает, равен ли текущий экземпляр другому экземпляру того же типа.
    /// </summary>
    /// <remarks>
    ///     Так как тип <see cref="Unit"/> не имеет внутреннего состояния, любые два экземпляра всегда эквивалентны. Метод всегда возвращает <see langword="true"/>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Unit other) => true;

    /// <inheritdoc cref="Equals(Unit)"/>
    public override bool Equals(object? obj) => obj is Unit;


    /// <summary>
    ///     Возвращает хэш-код для текущего экземпляра.
    /// </summary>
    /// <returns>   Постоянное значение <c>0</c>, так как тип не имеет изменяемого состояния.</returns>
    public override int GetHashCode() => 0;

    /// <summary>
    ///     Сравнивает текущий экземпляр с другим экземпляром <see cref="Unit"/>.
    /// </summary>
    /// <returns>   Всегда возвращает <c>0</c>, так как все экземпляры <see cref="Unit"/> эквивалентны.</returns>
    public int CompareTo(Unit other) => 0;

    /// <inheritdoc cref="CompareTo(Unit)"/>
    public int CompareTo(object? obj) => 0;

    /// <inheritdoc cref="Equals(Unit)"/>
    public static bool operator ==(Unit left, Unit right) => true;

    /// <summary>
    ///     Сравнивает два экземпляра типа <see cref="Unit"/> на неравенство.
    /// </summary>
    /// <remarks>
    ///     Так как любые два экземпляра типа <see cref="Unit"/> всегда равны, данный оператор всегда возвращает <see langword="false"/>.
    /// </remarks>
    public static bool operator !=(Unit left, Unit right) => false;

    /// <summary>
    ///     Возвращает строковое представление текущего экземпляра <see cref="Unit"/>.
    /// </summary>
    /// <returns>   Строковая константа <c>"()"</c>, представляющая отсутствие значения.</returns>
    public override string ToString() => "()";
}