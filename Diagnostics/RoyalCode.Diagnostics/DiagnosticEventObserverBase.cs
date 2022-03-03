namespace RoyalCode.Diagnostics;

/// <summary>
/// <para>
///     Abstract implementation of <see cref="IDiagnosticEventObserver"/>.
/// </para>
/// <para>
///     Implement the <see cref="CreateHandlers"/> method to create multiple <see cref="IDiagnosticEventHandler"/>.
/// </para>
/// <para>
///     Each handler handles a different event.
/// </para>
/// <para>
///     See also <see cref="DiagnosticEventHandlers"/> and <see cref="DiagnosticEventHandler{TArgument}"/>
///     for easy creation of handlers.
/// </para>
/// </summary>
public abstract class DiagnosticEventObserverBase : IDiagnosticEventObserver
{
    private readonly IEnumerable<IDiagnosticEventHandler> eventHandlers;
    private readonly string[] enabledOperationsNames;
    private readonly string[] ignoreOperationNames;

    /// <summary>
    /// Initialise the observer.
    /// </summary>
    protected DiagnosticEventObserverBase()
    {
        eventHandlers = CreateHandlers().ToArray()
            ?? throw new InvalidOperationException($"{nameof(CreateHandlers)} returns null");

        enabledOperationsNames = GetEnabledOperationsNames().ToArray()
            ?? throw new InvalidOperationException(
                $"{nameof(GetEnabledOperationsNames)} returns null");

        ignoreOperationNames = GetIgnoreOperationNames().ToArray()
            ?? throw new InvalidOperationException(
                $"{nameof(GetIgnoreOperationNames)} returns null");
    }

    /// <summary>
    /// <para>
    ///     Listener and event names.
    /// </para>
    /// <para>
    ///     A listener can handle several events.
    /// </para>
    /// </summary>
    public abstract string DiagnosticListenerName { get; }

    /// <summary>
    /// <para>
    ///     If observations of a certain event are activated.
    /// </para>
    /// <para>
    ///     This method can be overridden.
    ///     By default, if a handler exists for the event, it will be true, otherwise false.
    /// </para>
    /// </summary>
    /// <param name="eventName">Event name.</param>
    /// <returns>True if activated, false otherwise.</returns>
    public virtual bool IsEnabled(string eventName)
    {
        if (ignoreOperationNames.Length != 0)
        {
            for (int i = 0; i < ignoreOperationNames.Length; i++)
            {
                if (ignoreOperationNames[i] == eventName)
                    return false;
            }
        }

        if (enabledOperationsNames.Length == 0)
            return true;

        for (int i = 0; i < enabledOperationsNames.Length; i++)
        {
            if (enabledOperationsNames[i] == eventName)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Nop.
    /// </summary>
    public void OnCompleted()
    {
    }

    /// <summary>
    /// Nop.
    /// </summary>
    /// <param name="error"></param>
    public void OnError(Exception error)
    {
    }

    /// <summary>
    /// Execute <see cref="OnNext(string, object)"/>.
    /// </summary>
    /// <param name="value">Values of the diagnostic event that occurred.</param>
    public virtual void OnNext(KeyValuePair<string, object?> value) => OnNext(value.Key, value.Value);

    /// <summary>
    /// Scrolls through the handlers and delegates the event to which the name attends.
    /// </summary>
    /// <param name="eventName">Event name.</param>
    /// <param name="eventArgs">Event arguments.</param>
    protected virtual void OnNext(string eventName, object? eventArgs)
    {
        if (eventArgs is null)
            return;
        foreach (var handler in eventHandlers)
        {
            if (handler.EventName == eventName)
                handler.Handle(eventArgs);
        }
    }

    /// <summary>
    /// <para>
    ///     Creates the event handlers of this observer.
    /// </para>
    /// <para>
    ///     Use <c>yield</c> with
    ///     <see cref="DiagnosticEventHandlers.For{TArgument}(string, Action{TArgument}, string)"/>.
    /// </para>
    /// <para>
    ///     Create methods in the class that inherits <see cref="DiagnosticEventObserverBase"/>
    ///     to be used as delegates when creating the event handler.
    /// </para>
    /// </summary>
    /// <returns>A handler collection.</returns>
    protected abstract IEnumerable<IDiagnosticEventHandler> CreateHandlers();

    /// <summary>
    /// Names of the operations (events) that will be active.
    /// </summary>
    /// <returns>A collection with the names of the operations that must be observed.</returns>
    protected virtual IEnumerable<string> GetEnabledOperationsNames() => Array.Empty<string>();

    /// <summary>
    /// Names of the operations (events) that will be ignored.
    /// </summary>
    /// <returns>A collection with the names of the operations that should be ignored.</returns>
    protected virtual IEnumerable<string> GetIgnoreOperationNames() => Array.Empty<string>();
}