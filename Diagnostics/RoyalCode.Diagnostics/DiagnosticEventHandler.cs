namespace RoyalCode.Diagnostics;

/// <summary>
/// <para>
///     Default implementation of <see cref="IDiagnosticEventHandler"/>.
/// </para>
/// <para>
///     To make it easier to create this object use <see cref="DiagnosticEventHandlers.For(string, Action)"/>.
/// </para>
/// </summary>
public class DiagnosticEventHandler : IDiagnosticEventHandler
{
    private readonly Action handler;

    /// <summary>
    /// Name of the event handled.
    /// </summary>
    public string EventName { get; }

    /// <summary>
    /// Applies the handling using delegates.
    /// </summary>
    /// <param name="eventArgs">Not used.</param>
    public void Handle(object eventArgs) => handler();

    /// <summary>
    /// Creates new handler with the event name and the handler delegate.
    /// </summary>
    /// <param name="eventName">The event name.</param>
    /// <param name="handler">The handler delegate.</param>
    public DiagnosticEventHandler(string eventName, Action handler)
    {
        this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        EventName = eventName ?? throw new ArgumentNullException(nameof(eventName));
    }
}