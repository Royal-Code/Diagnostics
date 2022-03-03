using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RoyalCode.Diagnostics;

/// <summary>
/// <para>
///     This component encapsulates an instance of <see cref="System.Diagnostics.Activity"/>
///     and of <see cref="DiagnosticListener"/>, performing the initialization and stopping of the operation.
/// </para>
/// <para>
///     It is also possible to add items to share with listeners.
/// </para>
/// </summary>
public sealed class DiagnosticOperation : IDisposable
{
    private readonly bool isDiagnosticsEnabled;
    private readonly Dictionary<Type, object> items;
    private bool isStarted;
    private bool isDisposed;

    /// <summary>
    /// Instance of <see cref="System.Diagnostics.Activity"/> for this operation.
    /// </summary>
    public Activity Activity { get; }

    /// <summary>
    /// Diagnostic listener.
    /// </summary>
    public DiagnosticListener Listener { get; }

    /// <summary>
    /// The operation name, from the <see cref="Activity"/>.
    /// </summary>
    public string OperationName => Activity.OperationName;

    /// <summary>
    /// Creates a new operation.
    /// </summary>
    /// <param name="listener">Diagnostic listener.</param>
    /// <param name="operationName">The operation name</param>
    public DiagnosticOperation(DiagnosticListener listener, string operationName)
    {
        Listener = listener ?? throw new ArgumentNullException(nameof(listener));

        Activity = new Activity(operationName);

        isDiagnosticsEnabled = listener.IsEnabled(operationName);

        items = new Dictionary<Type, object>();
    }

    /// <summary>
    /// Fire the event.
    /// </summary>
    /// <param name="name">The event name.</param>
    public void FireEvent(string name)
    {
        if (isDiagnosticsEnabled)
        {
            Listener.Write(name, this);
        }
    }

    /// <summary>
    /// Fires an error event from an exception that, if set, will be added to the items.
    /// </summary>
    /// <typeparam name="TException">Type of exception captured.</typeparam>
    /// <param name="ex">Exception, optional.</param>
    public void FireError<TException>(TException? ex = null)
        where TException : Exception
    {
        if (!isDiagnosticsEnabled) 
            return;
        
        if (ex is not null)
            if (typeof(Exception) == typeof(TException))
                AddItem(ex);
            else
                With(ex).Add().AddAs<Exception>();

        Listener.Write($"{Activity.OperationName}.Error", this);
    }

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

    /// <summary>
    /// Creates a child operation, with options to share items and initialise it.
    /// </summary>
    /// <param name="operationName">The operation name.</param>
    /// <param name="copyItems">If you should copy the items, by default it is true.</param>
    /// <param name="start">Whether to start the operation.</param>
    /// <returns>New instance of <see cref="DiagnosticOperation"/>.</returns>
    public DiagnosticOperation Child(string operationName, bool copyItems = true, bool start = false)
    {
        var newOperation = new DiagnosticOperation(Listener, operationName);
        if (copyItems)
            foreach (var item in items)
                newOperation.items.Add(item.Key, item.Value);

        if (start)
            newOperation.Start();

        return newOperation;
    }

    /// <summary>
    /// Creates an error child operation, using the same name as the operation with the suffix "Error".
    /// </summary>
    /// <param name="ex">Error Exception.</param>
    /// <param name="start">Whether to start the operation.</param>
    /// <returns>New instance of <see cref="DiagnosticOperation"/>.</returns>
    public DiagnosticOperation ChildError<TException>(TException ex, bool start = true)
        where TException : Exception
    {
        if (ex is null)
            throw new ArgumentNullException(nameof(ex));

        var newOperation = new DiagnosticOperation(Listener, $"{OperationName}Error");
        foreach (var item in items)
            newOperation.items.Add(item.Key, item.Value);

        if (typeof(Exception) == typeof(TException))
            newOperation.AddItem(ex);
        else
            newOperation.With(ex).Add().AddAs<Exception>();

        if (start)
            newOperation.Start();

        return newOperation;
    }

    /// <summary>
    /// Starts the <see cref="Activity"/> using the <see cref="DiagnosticListener"/> when enabled.
    /// </summary>
    public void Start()
    {
        GuardDisposed();

        if (isStarted)
            return;

        isStarted = true;

        if (isDiagnosticsEnabled)
        {
            Listener.StartActivity(Activity, this);
        }
        else
        {
            Activity.Start();
        }
    }

    /// <summary>
    /// <para>
    ///     Stop the <see cref="Activity"/> when started, using the <see cref="DiagnosticListener"/> when enabled.
    /// </para>
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <para>
    ///     When disposed previously.
    /// </para>
    /// </exception>
    public void Dispose()
    {
        GuardDisposed();

        isDisposed = true;

        if (!isStarted)
            return;

        if (isDiagnosticsEnabled)
        {
            Listener.StopActivity(Activity, this);
        }
        else
        {
            Activity.Stop();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GuardDisposed()
    {
        if (isDisposed)
        {
            throw new InvalidOperationException("The operation was discarded and stopped previously.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private object? InternalTryGetItem(Type type)
    {
        return items.Values.FirstOrDefault(type.IsInstanceOfType);
    }

    /// <summary>
    /// Component to add an instance as multiple item types to a <see cref="DiagnosticOperation"/>.
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
}