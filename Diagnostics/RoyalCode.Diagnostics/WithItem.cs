namespace RoyalCode.Diagnostics;

/// <summary>
/// Component to add an instance as multiple item types to a <see cref="DiagnosticOperation"/>
/// and <see cref="OperationItems"/>.
/// </summary>
/// <typeparam name="TItem">The item type.</typeparam>
public sealed class WithItem<TItem>
{
    private readonly TItem item;
    private readonly Dictionary<Type, object> items;

    /// <summary>
    /// Creates a new component
    /// </summary>
    /// <param name="item">The item instance.</param>
    /// <param name="items">items of type <see cref="DiagnosticOperation"/>.</param>
    internal WithItem(TItem item, Dictionary<Type, object> items)
    {
        this.item = item ?? throw new ArgumentNullException(nameof(item));
        this.items = items ?? throw new ArgumentNullException(nameof(items));
    }

    /// <summary>
    /// Self add as an item.
    /// </summary>
    /// <returns>The same instance for chained calls.</returns>
    public WithItem<TItem> Add()
    {
        items.Add(typeof(TItem), item!);
        return this;
    }

    /// <summary>
    /// Adds the item as the specified type.
    /// </summary>
    /// <typeparam name="TItemType">The item type to be added.</typeparam>
    /// <returns>The same instance for chained calls.</returns>
    public WithItem<TItem> AddAs<TItemType>()
    {
        if (item! is not TItemType)
        {
            throw new ArgumentException(
                $"The current item is of type '{item!.GetType().FullName}' " +
                $"and does not implement the other type informed that is '{typeof(TItemType).FullName}'.");
        }

        items.Add(typeof(TItemType), item!);
        return this;
    }
}