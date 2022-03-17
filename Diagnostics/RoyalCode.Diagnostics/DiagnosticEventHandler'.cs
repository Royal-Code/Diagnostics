namespace RoyalCode.Diagnostics;

/// <summary>
/// <para>
///     Default implementation of <see cref="IDiagnosticEventHandler"/>.
/// </para>
/// <para>
///     This component will get a specific argument from the properties of the diagnostic event argument object
///     via a delegate <see cref="EventArgumentGetter{TArgument}"/> and execute a delegate handler.
/// </para>
/// <para>
///     To make it easier to create this object use <see cref="DiagnosticEventHandlers.For{TArgument}(string, Action{TArgument}, string)"/>.
/// </para>
/// </summary>
/// <typeparam name="TArgument">Type of the required argument.</typeparam>
public class DiagnosticEventHandler<TArgument> : IDiagnosticEventHandler
{
    private readonly Action<TArgument> handler;
    private readonly string? propertyName;
    private EventArgumentGetter<TArgument>? getter;

    /// <summary>
    /// Name of the event handled.
    /// </summary>
    public string EventName { get; }

    /// <summary>
    /// Applies the handling using delegates.
    /// </summary>
    /// <param name="eventArgs">Object of the event arguments.</param>
    public void Handle(object eventArgs)
    {
        if (eventArgs is null)
            throw new ArgumentNullException(nameof(eventArgs));

        getter ??= EventArgumentGetterFactory.Get<TArgument>(eventArgs.GetType(), propertyName);

        var argument = getter(eventArgs);
        if (argument is not null)
            handler(argument);
    }

    /// <summary>
    /// Creates new handler with the event name and the handler delegate.
    /// </summary>
    /// <param name="eventName">The event name.</param>
    /// <param name="handler">The handler delegate.</param>
    public DiagnosticEventHandler(string eventName, Action<TArgument> handler)
    {
        this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        EventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
    }

    /// <summary>
    /// Creates new handler with the event name, the handler delegate and argument property name.
    /// </summary>
    /// <param name="eventName">The event name.</param>
    /// <param name="handler">The handler delegate.</param>
    /// <param name="propertyName">The name of the argument property.</param>
    public DiagnosticEventHandler(string eventName, Action<TArgument> handler, string propertyName)
    {
        this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        this.propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        EventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
    }
}