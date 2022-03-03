namespace RoyalCode.Diagnostics;

/// <summary>
/// <para>
///     Default implementation of <see cref="IDiagnosticEventHandler"/>.
/// </para>
/// <para>
///     This component will get three specific arguments from the diagnostic event argument object properties
///     via a delegate <see cref="EventArgumentGetter{TArgument}"/> and execute a delegate handler.
/// </para>
/// <para>
///     To make it easier to create this object use
///     <see cref="DiagnosticEventHandlers.For{TArgument1, TArgument2, TArgument3}(string, Action{TArgument1, TArgument2, TArgument3}, string, string, string)"/>.
/// </para>
/// </summary>
/// <typeparam name="TArgument1">Type of the required argument 1.</typeparam>
/// <typeparam name="TArgument2">Type of the required argument 2.</typeparam>
/// <typeparam name="TArgument3">Type of the required argument 3.</typeparam>
public class DiagnosticEventHandler<TArgument1, TArgument2, TArgument3> : IDiagnosticEventHandler
{
    private readonly Action<TArgument1, TArgument2, TArgument3> handler;
    private readonly string?[] propertyNames;
    private EventArgumentGetter<TArgument1>? getter1;
    private EventArgumentGetter<TArgument2>? getter2;
    private EventArgumentGetter<TArgument3>? getter3;

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

        if (getter1 == null)
            getter1 = EventArgumentGetterFactory.Get<TArgument1>(eventArgs.GetType(), propertyNames[0]);

        if (getter2 == null)
            getter2 = EventArgumentGetterFactory.Get<TArgument2>(eventArgs.GetType(), propertyNames[1]);

        if (getter3 == null)
            getter3 = EventArgumentGetterFactory.Get<TArgument3>(eventArgs.GetType(), propertyNames[2]);

        var argument1 = getter1(eventArgs);
        var argument2 = getter2(eventArgs);
        var argument3 = getter3(eventArgs);
        if (argument1 != null || argument2 != null || argument3 != null)
            handler(argument1, argument2, argument3);
    }

    /// <summary>
    /// Creates new handler with the event name and the handler delegate.
    /// </summary>
    /// <param name="eventName">The event name.</param>
    /// <param name="handler">The handler delegate.</param>
    public DiagnosticEventHandler(string eventName, Action<TArgument1, TArgument2, TArgument3> handler)
    {
        this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        EventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
        propertyNames = new string[3];
    }

    /// <summary>
    /// Creates new handler with the event name, the handler delegate and argument property name.
    /// </summary>
    /// <param name="eventName">The event name.</param>
    /// <param name="handler">The handler delegate.</param>
    /// <param name="propertyName1">The name of the argument property 1.</param>
    /// <param name="propertyName2">The name of the argument property 2.</param>
    /// <param name="propertyName3">The name of the argument property 3.</param>
    public DiagnosticEventHandler(string eventName, Action<TArgument1, TArgument2, TArgument3> handler,
        string? propertyName1, string? propertyName2, string? propertyName3)
    {
        this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        EventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
        propertyNames = new string?[] {propertyName1, propertyName2, propertyName3};
    }
}