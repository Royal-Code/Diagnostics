using System.Runtime.CompilerServices;

namespace RoyalCode.Diagnostics;

/// <summary>
/// <para>
///     Component to store shared items of a <see cref="DiagnosticOperation"/>.
/// </para>
/// <para>
///     An object of this class will be used by the <see cref="DiagnosticOperation"/>
///     for the fired events.
/// </para>
/// </summary>
public class OperationItems
{
    private readonly Dictionary<Type, object> items = new();
    
    /// <summary>
    /// Add the object as item of type <typeparamref name="TItem"/>.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="item">The item object.</param>
    public void AddItem<TItem>(TItem item) =>
        items.Add(typeof(TItem), item ?? throw new ArgumentNullException(nameof(item)));

    /// <summary>
    /// Get the item of the type, when not found, returns default.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <returns>The item object or default</returns>
    public TItem? TryGetItem<TItem>()
        => (TItem?) (items.TryGetValue(typeof(TItem), out var item) ? item : InternalTryGetItem(typeof(TItem)));

    /// <summary>
    /// Get the item of the type, when not found, returns default.
    /// </summary>
    /// <param name="type">The item type.</param>
    /// <returns>The item object or default</returns>
    public object? TryGetItem(Type type)
        => items.TryGetValue(type, out var item) ? item : InternalTryGetItem(type);

    /// <summary>
    /// Get the item of the type, when not found, create.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="creation">The item object.</param>
    /// <returns></returns>
    public TItem GetOrCreateItem<TItem>(Func<TItem> creation)
    {
        if (items.TryGetValue(typeof(TItem), out var item))
            return (TItem) item;

        item = creation();

        if (item is null)
            throw new InvalidOperationException("The item returned by creation function is null");

        items.Add(typeof(TItem), item);

        return (TItem) item;
    }

    /// <summary>
    /// Creates an object of <see cref="WithItem{TItem}"/> to add an instance as multiple item types.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <param name="item">The item instance.</param>
    /// <returns>Component for adding the item instance as multiple types.</returns>
    public WithItem<TItem> With<TItem>(TItem item)
    {
        return new WithItem<TItem>(item, items);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private object? InternalTryGetItem(Type type)
    {
        return items.Values.FirstOrDefault(type.IsInstanceOfType);
    }
}